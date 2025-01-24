<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import WeekDaySelectionView from "./timetracking/WeekDaySelectionView.vue";
  import DayEntrySelectionView from "./timetracking/DayEntrySelectionView.vue";
  import DayEntryEditView from "./timetracking/DayEntryEditView.vue";
  import DayProjectOverviewView from "@/views/timetracking/DayProjectOverviewView.vue";
  import {computed} from "vue";
  import WeekSummaryRowView from "@/views/timetracking/WeekSummaryRowView.vue";
  
  function getWeekdayName(dateString: string | null | undefined): string {
    if (!dateString){ return ""; }
    
    const parts = dateString.split('-');
    const year = parseInt(parts[0]);
    const month = parseInt(parts[1]) - 1; // JavaScript months are zero-based
    const day = parseInt(parts[2]);
    const date = new Date(year, month, day);
    
    const weekdays = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    
    return weekdays[date.getDay()];
  }
  
  const currentWeekDay = computed(
      () => getWeekdayName(timeTrackingStore.selectedDay?.date)
  )
  
  const timeTrackingStore = useTimeTrackingStore();
</script>

<template>
  <div class="container">
    
    <!-- header -->
    <div class="row pt-5 text-center">
      <h2 v-if="timeTrackingStore.currentWeek">
        Year {{timeTrackingStore.currentWeek.year}}, Week {{timeTrackingStore.currentWeek.weekNumber}}
      </h2>
    </div>
    <div class="row py-1 text-center"
         v-if="timeTrackingStore.selectedDay">
      <h4>
        {{  timeTrackingStore.selectedDay.date }}
        <span v-if="currentWeekDay"> ({{ currentWeekDay }})</span>
      </h4>
    </div>
    
    <!-- week/day selection -->
    <div class="row">
      <WeekDaySelectionView />
    </div>
    
    <!-- week summary -->
    <div class="row">
      <WeekSummaryRowView />
    </div>
    
    <!-- day entry selection -->
    <div v-if="timeTrackingStore.selectedDay"
         class="row py-4">
      <DayEntrySelectionView />
    </div>

    <!-- day entry edit -->
    <div class="row py-4"
         v-if="timeTrackingStore.selectedEntry">
      <DayEntryEditView />
    </div>
    
    <div class="row py-4"
         v-if="timeTrackingStore.selectedDay">
      <DayProjectOverviewView />
    </div>

  </div>
</template>