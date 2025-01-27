<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import {computed} from "vue";

  const timeTrackingStore = useTimeTrackingStore();
  
  const totalEffort = () => {
    if(!timeTrackingStore.currentWeek){ return 0; }
    return timeTrackingStore.currentWeek.getAllDays()
        .flatMap(day => day.entries)
        .map(entry => entry.effortInHours)
        .reduce((sum, currentValue) => sum + currentValue);
  }

  const totalBilled = () => {
    if(!timeTrackingStore.currentWeek){ return 0; }
    return timeTrackingStore.currentWeek.getAllDays()
        .flatMap(day => day.entries)
        .map(entry => entry.effortBilled)
        .reduce((sum, currentValue) => sum + currentValue);
  };

</script>

<template>
  <p class="text-center">
    total effort: <span class="summaryValue">{{ totalEffort() }}</span>,
    total billed: <span class="summaryValue">{{ totalBilled() }}</span> 
  </p>
</template>

<style scoped>
  span.summaryValue{
    font-weight: bold;
  }
  
  p{
    margin-bottom: 0;
  }
</style>