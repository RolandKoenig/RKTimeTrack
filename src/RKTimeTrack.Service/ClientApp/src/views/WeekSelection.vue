<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/timeTrackingStore";
  import DayPreview from "@/views/DayPreview.vue";
  
  const timeTrackingStore = useTimeTrackingStore();
</script>

<template>
  <div v-if="timeTrackingStore.currentWeek"
       class="navigation-container">
    <Button label="<--" @click="timeTrackingStore.fetchWeekBeforeThisWeek"/>

    <DayPreview v-model="timeTrackingStore.currentWeek.monday"
                :is-selected="timeTrackingStore.currentWeek.monday == timeTrackingStore.selectedDay"
                @click="timeTrackingStore.selectMonday" />
    <DayPreview v-model="timeTrackingStore.currentWeek.tuesday"
                :is-selected="timeTrackingStore.currentWeek.tuesday == timeTrackingStore.selectedDay"
                @click="timeTrackingStore.selectTuesday" />
    <DayPreview v-model="timeTrackingStore.currentWeek.wednesday"
                :is-selected="timeTrackingStore.currentWeek.wednesday == timeTrackingStore.selectedDay"
                @click="timeTrackingStore.selectWednesday" />
    <DayPreview v-model="timeTrackingStore.currentWeek.thursday"
                :is-selected="timeTrackingStore.currentWeek.thursday == timeTrackingStore.selectedDay"
                @click="timeTrackingStore.selectThursday" />
    <DayPreview v-model="timeTrackingStore.currentWeek.friday"
                :is-selected="timeTrackingStore.currentWeek.friday == timeTrackingStore.selectedDay"
                @click="timeTrackingStore.selectFriday" />
    <DayPreview v-model="timeTrackingStore.currentWeek.saturday"
                :is-selected="timeTrackingStore.currentWeek.saturday == timeTrackingStore.selectedDay"
                @click="timeTrackingStore.selectSaturday" />
    <DayPreview v-model="timeTrackingStore.currentWeek.sunday"
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
  
  button{
    margin: 1rem;
  }
</style>