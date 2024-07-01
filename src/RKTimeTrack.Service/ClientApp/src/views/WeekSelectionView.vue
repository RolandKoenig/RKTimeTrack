<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/timeTrackingStore";
  import TimeTrackingDayButton from "@/components/TimeTrackingDayButton.vue";
  
  const timeTrackingStore = useTimeTrackingStore();
</script>

<template>
  <div v-if="timeTrackingStore.currentWeek"
       class="navigation-container">
    <Button label="<--" @click="timeTrackingStore.fetchWeekBeforeThisWeek"/>

    <TimeTrackingDayButton v-model="timeTrackingStore.currentWeek.monday"
                           :is-selected="timeTrackingStore.currentWeek.monday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectMonday" />
    <TimeTrackingDayButton v-model="timeTrackingStore.currentWeek.tuesday"
                           :is-selected="timeTrackingStore.currentWeek.tuesday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectTuesday" />
    <TimeTrackingDayButton v-model="timeTrackingStore.currentWeek.wednesday"
                           :is-selected="timeTrackingStore.currentWeek.wednesday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectWednesday" />
    <TimeTrackingDayButton v-model="timeTrackingStore.currentWeek.thursday"
                           :is-selected="timeTrackingStore.currentWeek.thursday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectThursday" />
    <TimeTrackingDayButton v-model="timeTrackingStore.currentWeek.friday"
                           :is-selected="timeTrackingStore.currentWeek.friday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectFriday" />
    <TimeTrackingDayButton v-model="timeTrackingStore.currentWeek.saturday"
                           :is-selected="timeTrackingStore.currentWeek.saturday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectSaturday" />
    <TimeTrackingDayButton v-model="timeTrackingStore.currentWeek.sunday"
                           :is-selected="timeTrackingStore.currentWeek.sunday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectSunday" />

    <Button label="-->" @click="timeTrackingStore.fetchWeekAfterThisWeek"/>
  </div>
</template>

<style scoped>
  div.navigation-container{
    display: flex;
    justify-content: center;
  }

  button, div.buttonBorder{
    margin: 1rem;
  }
</style>