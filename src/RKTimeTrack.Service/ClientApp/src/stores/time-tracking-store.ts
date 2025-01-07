import {ref, type Ref, inject, computed, watch} from 'vue'
import {defineStore} from 'pinia'
import {
    TimeTrackClient,
    TimeTrackingDayType, 
    TimeTrackingEntryType,
    UpdateDay_Request
} from "@/services/time-track-client.generated";
import {useTopicStore} from "@/stores/topic-store";
import {UiTimeTrackingWeek} from "@/stores/models/ui-time-tracking-week";
import {UiTimeTrackingDay} from "@/stores/models/ui-time-tracking-day";
import {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";
import { useToast } from 'primevue/usetoast';

export const useTimeTrackingStore = defineStore('timeTrackingStore', () =>{
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
    
    const topicStore = useTopicStore();
    const toast = useToast();
    
    const currentWeek: Ref<UiTimeTrackingWeek | null> = ref(null);
    const selectedDay: Ref<UiTimeTrackingDay | null> = ref(null);
    const selectedEntry: Ref<UiTimeTrackingEntry | null> = ref(null);
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
    const entryTypeValues = ref([
        TimeTrackingEntryType.Default,
        TimeTrackingEntryType.Training,
        TimeTrackingEntryType.OnCall
    ])
    
    const availableTopicCategories = computed(() =>{
       return topicStore.topics
           .map(x => x.category)
           .filter(onlyUnique);
    });
    
    const availableTopicNames = computed(() =>{
        if(!selectedEntry.value){ return []; }
        if(!selectedEntry.value.topicCategory){ return []; }
        
        const filterCategory = selectedEntry.value.topicCategory;
        return topicStore.topics
            .filter(x => x.category === filterCategory)
            .map(x => x.name)
            .filter(onlyUnique);
    })

    const canCurrentTopicBeInvoiced = computed(() => {
        if(!selectedEntry.value){ return false; }
        if(!selectedEntry.value?.topicName){ return false; }

        const topic = topicStore.topics.find(x =>
            x.category == selectedEntry.value!.topicCategory &&
            x.name == selectedEntry.value!.topicName);

        // noinspection RedundantIfStatementJS
        if(!topic || !topic.canBeInvoiced){ return false; }
        return true;
    });
    
    watch(
        canCurrentTopicBeInvoiced,
        (newValue) => {
            if(!selectedEntry.value){ return; }
            if(!newValue){
                selectedEntry.value.effortBilled = 0;
            }
        });
    
    // Save changes directly
    watch(
        selectedDay,
        (newValue, oldValue) =>{
            if(!oldValue){ return; }
            if(newValue !== oldValue){ return; } // <-- We don't want to react, when user is just selecting another day
            if(!isDayValid(oldValue)){ return; }
            
            // TODO: async handling of updates
            timeTrackClient.updateDay(new UpdateDay_Request(oldValue.toBackendModel()))
                .catch(() =>{
                    toast.add({
                        severity: 'error',
                        summary: 'Communication Error',
                        detail: 'Unable to send data to backend!',
                        life: 3000
                    });
                });
        }, {
            deep: true
        });
    
    function isDayValid(day: UiTimeTrackingDay): Boolean{
        if(!day){ return false; }
        if(!day.date){ return false; }
        if(!day.type){ return false; }
        if(!day.entries){ return false; }
        
        for(let loop=0; loop<day.entries.length; loop++){
            if(day.entries[loop].effortInHours < 0){ return false; }
            if(day.entries[loop].effortBilled < 0){ return false; }
        }
        
        return true;
    }
    
    function selectedEntryCategoryChanged(){
        if(!selectedEntry.value){ return; }
        selectedEntry.value.topicName = "";
    }
    
    function selectMonday(){
        selectedDay.value = currentWeek.value?.monday ?? null;
        selectedEntry.value = null;
    }

    function selectTuesday(){
        selectedDay.value = currentWeek.value?.tuesday ?? null;
        selectedEntry.value = null;
    }

    function selectWednesday(){
        selectedDay.value = currentWeek.value?.wednesday ?? null;
        selectedEntry.value = null;
    }

    function selectThursday(){
        selectedDay.value = currentWeek.value?.thursday ?? null;
        selectedEntry.value = null;
    }

    function selectFriday(){
        selectedDay.value = currentWeek.value?.friday ?? null;
        selectedEntry.value = null;
    }

    function selectSaturday(){
        selectedDay.value = currentWeek.value?.saturday ?? null;
        selectedEntry.value = null;
    }

    function selectSunday(){
        selectedDay.value = currentWeek.value?.sunday ?? null;
        selectedEntry.value = null;
    }
    
    async function fetchCurrentWeek() {
        await wrapLoadingCall(async () =>{
            currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getCurrentWeek());
            selectedDay.value = currentWeek.value.monday;
            selectedEntry.value = null;
        })
    }

    async function fetchInitialData() {
        await wrapLoadingCall(async () =>{
            currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getCurrentWeek());
            
            if(!trySelectDayByDayOfWeek(new Date().getDay())){
                selectedEntry.value = null;
            }
        })
    }
    function tryGetSelectedDayOfWeek(): number{
        if(!currentWeek.value){ return -1; }
        if(!selectedDay.value){ return -1; }

        const dateMilliseconds = Date.parse(selectedDay.value.date);
        const date = new Date(dateMilliseconds);
        return date.getDay();
    }
    
    function trySelectDayByDayOfWeek(dayOfWeek: number): boolean{
        if(!currentWeek.value){ return false; }
        
        switch (dayOfWeek){
            case 0: selectedDay.value = currentWeek.value.sunday; return true;
            case 1: selectedDay.value = currentWeek.value.monday; return true;
            case 2: selectedDay.value = currentWeek.value.tuesday; return true;
            case 3: selectedDay.value = currentWeek.value.wednesday; return true;
            case 4: selectedDay.value = currentWeek.value.thursday; return true;
            case 5: selectedDay.value = currentWeek.value.friday; return true;
            case 6: selectedDay.value = currentWeek.value.saturday; return true;
            default: selectedDay.value = currentWeek.value.monday; return false;
        }
    }
    
    async function fetchCurrentWeekAgain(){
        if(!currentWeek.value){
            await fetchCurrentWeek();
            return;
        }
        
        const prevSelectedDayDate = selectedDay.value?.date;
        await wrapLoadingCall(async () => {
            currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getWeek(
                currentWeek.value!.year, currentWeek.value!.weekNumber));
            
            if(prevSelectedDayDate){
                selectedDay.value = currentWeek.value.tryGetDayByDate(prevSelectedDayDate);
            }else{
                selectedEntry.value = null;
            }
            selectedEntry.value = null;
        });
    }

    /**
     * Go one week backward
     */
    async function fetchWeekBeforeThisWeek(){
        if(!currentWeek.value){
            await fetchCurrentWeek();
            return;
        }
        
        const selectedDayOfWeek = tryGetSelectedDayOfWeek();
        
        const year = currentWeek.value.year;
        const weekNumber = currentWeek.value.weekNumber;
        await wrapLoadingCall(async () =>{
            if(weekNumber > 1){
                currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getWeek(
                    year, weekNumber - 1));
                if(!trySelectDayByDayOfWeek(selectedDayOfWeek)){
                    selectedDay.value = currentWeek.value.monday;   
                }
                selectedEntry.value = null;
            }else{
                const previousYearMetadata = await timeTrackClient.getYearMetadata(year - 1);
                currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getWeek(
                    year - 1,
                    previousYearMetadata.maxWeekNumber));
                if(!trySelectDayByDayOfWeek(selectedDayOfWeek)){
                    selectedDay.value = currentWeek.value.monday;
                }
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

        const selectedDayOfWeek = tryGetSelectedDayOfWeek();
        
        const year = currentWeek.value.year;
        const weekNumber = currentWeek.value.weekNumber;
        await wrapLoadingCall(async () =>{
            if(weekNumber === 53){
                currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getWeek(
                    year + 1,
                    1));
                if(!trySelectDayByDayOfWeek(selectedDayOfWeek)){
                    selectedDay.value = currentWeek.value.monday;
                }
                selectedEntry.value = null;
            } else if(weekNumber === 52){
                const actYearMetadata = await timeTrackClient.getYearMetadata(year);
                if(actYearMetadata.maxWeekNumber === 53){
                    currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getWeek(
                        year,
                        weekNumber + 1));
                    if(!trySelectDayByDayOfWeek(selectedDayOfWeek)){
                        selectedDay.value = currentWeek.value.monday;
                    }
                    selectedEntry.value = null;
                }else{
                    currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getWeek(
                        year + 1,
                        1));
                    if(!trySelectDayByDayOfWeek(selectedDayOfWeek)){
                        selectedDay.value = currentWeek.value.monday;
                    }
                    selectedEntry.value = null;
                }
            } else {
                currentWeek.value = UiTimeTrackingWeek.fromBackendModel(await timeTrackClient.getWeek(
                    year,
                    weekNumber + 1));
                if(!trySelectDayByDayOfWeek(selectedDayOfWeek)){
                    selectedDay.value = currentWeek.value.monday;
                }
                selectedEntry.value = null;
            }
        })
    }

    function addNewEntry(){
        if(isLoading.value){ return; }
        if(!selectedDay.value){ return; }
        if(!selectedDay.value.entries){ return; }

        const newEntry = new UiTimeTrackingEntry(
            undefined,
            "",
            "",
            0,
            0,
            TimeTrackingEntryType.Default,
            "",
        );
        
        selectedDay.value.entries.push(newEntry);
        selectedEntry.value = newEntry;
    }
    
    function applyNewEntryCollection(entries : UiTimeTrackingEntry[]){
        if(isLoading.value){ return; }
        if(!selectedDay.value){ return; }
        
        selectedDay.value.entries = entries;
    }
    
    function copySelectedEntry(){
        if(isLoading.value){ return; }
        if(!selectedEntry.value){ return; }
        if(!selectedDay.value){ return; }
        if(!selectedDay.value.entries){ return; }

        const newEntry = new UiTimeTrackingEntry(
            undefined,
            selectedEntry.value.topicCategory,
            selectedEntry.value.topicName,
            selectedEntry.value.effortInHours,
            selectedEntry.value.effortBilled,
            selectedEntry.value.type,
            selectedEntry.value.description,
        );

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
    
    function copyEffortToEffortBilled(){
        if(isLoading.value){ return; }
        if(!selectedDay.value){ return; }
        if(!selectedEntry.value){ return; }
        if(!canCurrentTopicBeInvoiced.value){ return; }
        
        selectedEntry.value.effortBilled = selectedEntry.value.effortInHours;
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
    
    // noinspection JSIgnoredPromiseFromCall
    fetchInitialData();
    
    return{
        currentWeek, 
        selectedDay, selectedEntry,
        dayTypeValues, entryTypeValues,
        selectMonday, selectTuesday, selectWednesday, selectThursday, selectFriday, selectSaturday, selectSunday,
        fetchCurrentWeekAgain, fetchWeekBeforeThisWeek, fetchWeekAfterThisWeek,
        availableTopicCategories, availableTopicNames, canCurrentTopicBeInvoiced,selectedEntryCategoryChanged,
        addNewEntry, copySelectedEntry, deleteSelectedEntry, copyEffortToEffortBilled,
        applyNewEntryCollection
    }
});