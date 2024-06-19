<script setup lang="ts">
  import {inject, type Ref, ref} from "vue";
  import {TimeTrackClient, TimeTrackingWeek} from "@/services/time-track-client.generated";
  
  const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
  const currentWeek: Ref<TimeTrackingWeek | undefined> = ref();

  async function fetchCurrentWeek() {
    currentWeek.value = await timeTrackClient.getCurrentWeek();
  }
  
  // Move one week backward
  async function fetchWeekBeforeThisWeek(){
    if(!currentWeek.value){
      await fetchCurrentWeek();
      return;
    }
    
    if(currentWeek.value.weekNumber > 1){
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year,
          currentWeek.value.weekNumber - 1);
    }else{
      const previousYearMetadata = await timeTrackClient.getYearMetadata(currentWeek.value.year - 1);
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year - 1,
          previousYearMetadata.maxWeekNumber);
    }
  }
  
  // Move one week forward
  async function fetchWeekAfterThisWeek(){
    if(!currentWeek.value){
      await fetchCurrentWeek();
      return;
    }

    if(currentWeek.value.weekNumber === 53){
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year + 1,
          1);
    } else if(currentWeek.value.weekNumber === 52){
      const actYearMetadata = await timeTrackClient.getYearMetadata(currentWeek.value.year);
      if(actYearMetadata.maxWeekNumber === 53){
        currentWeek.value = await timeTrackClient.getWeek(
            currentWeek.value.year,
            currentWeek.value.weekNumber + 1);
      }else{
        currentWeek.value = await timeTrackClient.getWeek(
            currentWeek.value.year + 1,
            1);
      }
    } else {
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year,
          currentWeek.value.weekNumber + 1);
    }
  }
  
  fetchCurrentWeek();
</script>

<template>
  <header>
    <div class="wrapper">
      <h1>Year: {{currentWeek?.year}}, Week: {{currentWeek?.weekNumber}}</h1>
      <!-- <HelloWorld msg="Year: {{currentWeek.year}}, Week: {{currentWeek.weekNumber}}" /> -->

      <!-- <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav>-->

      <Button label="<--" @click="fetchWeekBeforeThisWeek"/>
      <Button label="-->" @click="fetchWeekAfterThisWeek"/>
      
    </div>
  </header>

  <!-- <RouterView /> -->
</template>

<style scoped>

</style>
