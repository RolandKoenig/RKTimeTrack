<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import WeekDaySelectionView from "./timetracking/WeekDaySelectionView.vue";
  import DayEntrySelectionView from "./timetracking/DayEntrySelectionView.vue";
  import DayEntryEditView from "./timetracking/DayEntryEditView.vue";
  import DayProjectOverviewView from "@/views/timetracking/DayProjectOverviewView.vue";
  
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
      <h4>{{  timeTrackingStore.selectedDay.date }}</h4>
    </div>
    
    <!-- week/day selection -->
    <div class="row">
      <WeekDaySelectionView />
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