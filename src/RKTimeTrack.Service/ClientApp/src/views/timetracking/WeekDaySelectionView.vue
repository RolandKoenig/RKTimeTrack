﻿<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import TimeTrackingDayButton from "./TimeTrackingDayButton.vue";
  import IconMoveLeft from "@/components/icons/IconMoveLeft.vue";
  import IconMoveRight from "@/components/icons/IconMoveRight.vue";
  
  const timeTrackingStore = useTimeTrackingStore();
</script>

<template>
  <div v-if="timeTrackingStore.currentWeek"
       class="navigation-container">
    <Button text
            @click="timeTrackingStore.fetchWeekBeforeThisWeek">
      <IconMoveLeft />
    </Button>

    <TimeTrackingDayButton :time-tracking-day="timeTrackingStore.currentWeek.monday"
                           :is-selected="timeTrackingStore.currentWeek.monday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectMonday" />
    <TimeTrackingDayButton :time-tracking-day="timeTrackingStore.currentWeek.tuesday"
                           :is-selected="timeTrackingStore.currentWeek.tuesday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectTuesday" />
    <TimeTrackingDayButton :time-tracking-day="timeTrackingStore.currentWeek.wednesday"
                           :is-selected="timeTrackingStore.currentWeek.wednesday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectWednesday" />
    <TimeTrackingDayButton :time-tracking-day="timeTrackingStore.currentWeek.thursday"
                           :is-selected="timeTrackingStore.currentWeek.thursday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectThursday" />
    <TimeTrackingDayButton :time-tracking-day="timeTrackingStore.currentWeek.friday"
                           :is-selected="timeTrackingStore.currentWeek.friday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectFriday" />
    <TimeTrackingDayButton :time-tracking-day="timeTrackingStore.currentWeek.saturday"
                           :is-selected="timeTrackingStore.currentWeek.saturday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectSaturday" />
    <TimeTrackingDayButton :time-tracking-day="timeTrackingStore.currentWeek.sunday"
                           :is-selected="timeTrackingStore.currentWeek.sunday == timeTrackingStore.selectedDay"
                           @click="timeTrackingStore.selectSunday" />

    <Button text
            @click="timeTrackingStore.fetchWeekAfterThisWeek">
      <IconMoveRight />
    </Button>
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