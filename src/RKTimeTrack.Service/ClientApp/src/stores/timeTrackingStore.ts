import {ref, type Ref, inject} from 'vue'
import {defineStore} from 'pinia'
import {
    TimeTrackClient,
    TimeTrackingDay,
    TimeTrackingDayType,
    TimeTrackingEntry,
    TimeTrackingWeek
} from "@/services/time-track-client.generated";

export const useTimeTrackingStore = defineStore('timeTrackingStore', () =>{
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
    
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

    /**
     * Private helper function to ensure, that we only call one loading function in parallel
     * @param wrappedFunction
     */
    async function wrapLoadingCall(wrappedFunction: () => Promise<void>){
        if(isLoading.value){ return; }
        try{
            await wrappedFunction();
        }finally {
            isLoading.value = false;
        }
    }
    
    return{
        currentWeek, 
        selectedDay, selectedEntry,
        dayTypeValues,
        selectMonday, selectTuesday, selectWednesday, selectThursday, selectFriday, selectSaturday, selectSunday,
        fetchCurrentWeek, fetchWeekBeforeThisWeek, fetchWeekAfterThisWeek
    }
});