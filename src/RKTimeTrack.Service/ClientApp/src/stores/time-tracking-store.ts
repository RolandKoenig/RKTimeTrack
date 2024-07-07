import {ref, type Ref, inject, computed, watch} from 'vue'
import {defineStore} from 'pinia'
import {
    TimeTrackClient,
    TimeTrackingDay,
    TimeTrackingDayType,
    TimeTrackingEntry,
    TimeTrackingWeek,
    TimeTrackingTopicReference, 
    TimeTrackingTopic,
    UpdateDayRequest
} from "@/services/time-track-client.generated";

export const useTimeTrackingStore = defineStore('timeTrackingStore', () =>{
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
    
    const topicData: Ref<TimeTrackingTopic[]> = ref([]); 
    const currentWeek: Ref<TimeTrackingWeek | null | undefined> = ref(null);
    const selectedDay: Ref<TimeTrackingDay | null | undefined> = ref(null);
    const selectedEntry: Ref<TimeTrackingEntry | null | undefined> = ref(null);
    
    const isLoading: Ref<boolean> = ref(false);

    const dayTypeValues = ref([
        TimeTrackingDayType.CompensatoryTimeOff,
        TimeTrackingDayType.Holiday,
        TimeTrackingDayType.Ill,
        TimeTrackingDayType.OwnEducation,
        TimeTrackingDayType.PublicHoliday,
        TimeTrackingDayType.Training,
        TimeTrackingDayType.Weekend,
        TimeTrackingDayType.WorkingDay
    ]);

    // Save changes directly
    watch(
        selectedDay,
        (newValue, oldValue) =>{
            if(!oldValue){ return; }

            timeTrackClient.updateDay(new UpdateDayRequest({
                date: oldValue.date,
                entries: oldValue.entries ? [...oldValue.entries] : [],
                type: oldValue.type
            }));
        }, {
            deep: true
        });
    
    const availableTopicCategories = computed(() =>{
       return topicData.value
           .map(x => x.category)
           .filter(onlyUnique);
    });
    
    const availableTopicNames = computed(() =>{
        if(!selectedEntry.value){ return []; }
        if(!selectedEntry.value.topic.category){ return []; }
        
        const filterCategory = selectedEntry.value.topic.category;
        return topicData.value
            .filter(x => x.category === filterCategory)
            .map(x => x.name)
            .filter(onlyUnique);
    })
    
    function selectedEntryCategoryChanged(){
        if(!selectedEntry.value){ return; }
        selectedEntry.value.topic.name = "";
    }
    
    function selectMonday(){
        selectedDay.value = currentWeek.value?.monday;
        selectedEntry.value = null;
    }

    function selectTuesday(){
        selectedDay.value = currentWeek.value?.tuesday;
        selectedEntry.value = null;
    }

    function selectWednesday(){
        selectedDay.value = currentWeek.value?.wednesday;
        selectedEntry.value = null;
    }

    function selectThursday(){
        selectedDay.value = currentWeek.value?.thursday;
        selectedEntry.value = null;
    }

    function selectFriday(){
        selectedDay.value = currentWeek.value?.friday;
        selectedEntry.value = null;
    }

    function selectSaturday(){
        selectedDay.value = currentWeek.value?.saturday;
        selectedEntry.value = null;
    }

    function selectSunday(){
        selectedDay.value = currentWeek.value?.sunday;
        selectedEntry.value = null;
    }
    
    async function fetchCurrentWeek() {
        await wrapLoadingCall(async () =>{
            currentWeek.value = await timeTrackClient.getCurrentWeek();
            selectedDay.value = currentWeek.value.monday;
            selectedEntry.value = null;
        })
    }

    async function fetchInitialData() {
        await wrapLoadingCall(async () =>{
            topicData.value = await timeTrackClient.getAllTopics();
            
            currentWeek.value = await timeTrackClient.getCurrentWeek();
            selectedDay.value = currentWeek.value.monday;
            selectedEntry.value = null;
        })
    }

    /**
     * Go one week backward
     */
    async function fetchWeekBeforeThisWeek(){
        if(!currentWeek.value){
            await fetchCurrentWeek();
            return;
        }
        
        const year = currentWeek.value.year;
        const weekNumber = currentWeek.value.weekNumber;
        await wrapLoadingCall(async () =>{
            if(weekNumber > 1){
                currentWeek.value = await timeTrackClient.getWeek(
                    year, weekNumber - 1);
                selectedDay.value = currentWeek.value.monday;
                selectedEntry.value = null;
            }else{
                const previousYearMetadata = await timeTrackClient.getYearMetadata(year - 1);
                currentWeek.value = await timeTrackClient.getWeek(
                    year - 1,
                    previousYearMetadata.maxWeekNumber);
                selectedDay.value = currentWeek.value.monday;
                selectedEntry.value = null;
            }
        })
    }

    /**
     * Move one week forward
     */
    async function fetchWeekAfterThisWeek(){
        if(!currentWeek.value){
            await fetchCurrentWeek();
            return;
        }

        const year = currentWeek.value.year;
        const weekNumber = currentWeek.value.weekNumber;
        await wrapLoadingCall(async () =>{
            if(weekNumber === 53){
                currentWeek.value = await timeTrackClient.getWeek(
                    year + 1,
                    1);
                selectedDay.value = currentWeek.value.monday;
                selectedEntry.value = null;
            } else if(weekNumber === 52){
                const actYearMetadata = await timeTrackClient.getYearMetadata(year);
                if(actYearMetadata.maxWeekNumber === 53){
                    currentWeek.value = await timeTrackClient.getWeek(
                        year,
                        weekNumber + 1);
                    selectedDay.value = currentWeek.value.monday;
                    selectedEntry.value = null;
                }else{
                    currentWeek.value = await timeTrackClient.getWeek(
                        year + 1,
                        1);
                    selectedDay.value = currentWeek.value.monday;
                    selectedEntry.value = null;
                }
            } else {
                currentWeek.value = await timeTrackClient.getWeek(
                    year,
                    weekNumber + 1);
                selectedDay.value = currentWeek.value.monday;
                selectedEntry.value = null;
            }
        })
    }

    function addNewEntry(){
        if(isLoading.value){ return; }
        if(!selectedDay.value){ return; }
        if(!selectedDay.value.entries){ return; }

        const newEntry = new TimeTrackingEntry({
            description: "",
            topic: new TimeTrackingTopicReference({
                category: "",
                name: ""
            }),
            effortInHours: 0,
            effortBilled: 0,
        });
        
        selectedDay.value.entries.push(newEntry);
        selectedEntry.value = newEntry;
    }
    
    function copySelectedEntry(){
        if(isLoading.value){ return; }
        if(!selectedEntry.value){ return; }
        if(!selectedDay.value){ return; }
        if(!selectedDay.value.entries){ return; }

        const newEntry = new TimeTrackingEntry({
            description: selectedEntry.value.description,
            topic: new TimeTrackingTopicReference({
                category: selectedEntry.value.topic.category,
                name: selectedEntry.value.topic.name
            }),
            effortInHours: selectedEntry.value.effortInHours,
            effortBilled: selectedEntry.value.effortBilled,
        });

        selectedDay.value.entries.push(newEntry);
        selectedEntry.value = newEntry;
    }
    
    function deleteSelectedEntry(){
        if(isLoading.value){ return; }
        if(!selectedDay.value){ return; }
        if(!selectedEntry.value){ return; }
        if(!selectedDay.value.entries){ return; }

        const index = selectedDay.value.entries.indexOf(selectedEntry.value);
        if(index < 0){ return; }

        selectedDay.value.entries.splice(index, 1);
        
        if(selectedDay.value.entries.length > index){
            selectedEntry.value = selectedDay.value.entries[index];
        } else if(selectedDay.value.entries.length > 0){
            selectedEntry.value = selectedDay.value.entries[index -1];
        } else {
            selectedEntry.value = null;
        }
    }
    
    /**
     * Private helper function to ensure, that we only call one loading function in parallel
     */
    async function wrapLoadingCall(wrappedFunction: () => Promise<void>){
        if(isLoading.value){ return; }
        try{
            await wrappedFunction();
        }finally {
            isLoading.value = false;
        }
    }

    /**
     * Helper method for getting distinct entries of an array
     */
    function onlyUnique<T>(value: T, index: number, array: T[]) {
        return array.indexOf(value) === index;
    }
    
    return{
        currentWeek, 
        selectedDay, selectedEntry,
        dayTypeValues,
        selectMonday, selectTuesday, selectWednesday, selectThursday, selectFriday, selectSaturday, selectSunday,
        fetchInitialData, fetchCurrentWeek, fetchWeekBeforeThisWeek, fetchWeekAfterThisWeek,
        availableTopicCategories, availableTopicNames, selectedEntryCategoryChanged,
        addNewEntry, copySelectedEntry, deleteSelectedEntry
    }
});