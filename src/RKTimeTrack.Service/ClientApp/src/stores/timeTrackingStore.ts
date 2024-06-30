﻿import {ref, type Ref, inject} from 'vue'
import {defineStore} from 'pinia'
import {
    TimeTrackClient,
    TimeTrackingDay,
    TimeTrackingDayType,
    TimeTrackingRow,
    TimeTrackingWeek
} from "@/services/time-track-client.generated";

export const useTimeTrackingStore = defineStore('timeTrackingStore', () =>{
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
    
    const currentWeek: Ref<TimeTrackingWeek | undefined> = ref();
    const selectedDay: Ref<TimeTrackingDay | undefined> = ref();
    const selectedRow: Ref<TimeTrackingRow | undefined> = ref();
    
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
        selectedRow.value = undefined;
    }

    function selectTuesday(){
        selectedDay.value = currentWeek.value?.tuesday;
        selectedRow.value = undefined;
    }

    function selectWednesday(){
        selectedDay.value = currentWeek.value?.wednesday;
        selectedRow.value = undefined;
    }

    function selectThursday(){
        selectedDay.value = currentWeek.value?.thursday;
        selectedRow.value = undefined;
    }

    function selectFriday(){
        selectedDay.value = currentWeek.value?.friday;
        selectedRow.value = undefined;
    }

    function selectSaturday(){
        selectedDay.value = currentWeek.value?.saturday;
        selectedRow.value = undefined;
    }

    function selectSunday(){
        selectedDay.value = currentWeek.value?.sunday;
        selectedRow.value = undefined;
    }
    
    async function fetchCurrentWeek() {
        await wrapLoadingCall(async () =>{
            currentWeek.value = await timeTrackClient.getCurrentWeek();
            selectedDay.value = currentWeek.value.monday;
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
                selectedRow.value = undefined;
            }else{
                const previousYearMetadata = await timeTrackClient.getYearMetadata(year - 1);
                currentWeek.value = await timeTrackClient.getWeek(
                    year - 1,
                    previousYearMetadata.maxWeekNumber);
                selectedDay.value = currentWeek.value.monday;
                selectedRow.value = undefined;
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
                selectedRow.value = undefined;
            } else if(weekNumber === 52){
                const actYearMetadata = await timeTrackClient.getYearMetadata(year);
                if(actYearMetadata.maxWeekNumber === 53){
                    currentWeek.value = await timeTrackClient.getWeek(
                        year,
                        weekNumber + 1);
                    selectedDay.value = currentWeek.value.monday;
                    selectedRow.value = undefined;
                }else{
                    currentWeek.value = await timeTrackClient.getWeek(
                        year + 1,
                        1);
                    selectedDay.value = currentWeek.value.monday;
                    selectedRow.value = undefined;
                }
            } else {
                currentWeek.value = await timeTrackClient.getWeek(
                    year,
                    weekNumber + 1);
                selectedDay.value = currentWeek.value.monday;
                selectedRow.value = undefined;
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
        currentWeek, selectedDay, selectedRow,
        dayTypeValues,
        selectMonday, selectTuesday, selectWednesday, selectThursday, selectFriday, selectSaturday, selectSunday,
        fetchCurrentWeek, fetchWeekBeforeThisWeek, fetchWeekAfterThisWeek
    }
});