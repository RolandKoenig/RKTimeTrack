<script setup lang="ts">
import {inject, type Ref, ref} from "vue";
import {
  TimeTrackClient,
  TimeTrackingDay,
  TimeTrackingDayType,
  TimeTrackingWeek
} from "@/services/time-track-client.generated";
import DayPreviewView from "@/views/DayPreviewView.vue";

const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
  const currentWeek: Ref<TimeTrackingWeek | undefined> = ref();
  const selectedDay: Ref<TimeTrackingDay | undefined> = ref();

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
  }

  function selectTuesday(){
    selectedDay.value = currentWeek.value?.tuesday;
  }

  function selectWednesday(){
    selectedDay.value = currentWeek.value?.wednesday;
  }

  function selectThursday(){
    selectedDay.value = currentWeek.value?.thursday;
  }

  function selectFriday(){
    selectedDay.value = currentWeek.value?.friday;
  }

  function selectSaturday(){
    selectedDay.value = currentWeek.value?.saturday;
  }

  function selectSunday(){
    selectedDay.value = currentWeek.value?.sunday;
  }
  
  async function fetchCurrentWeek() {
    currentWeek.value = await timeTrackClient.getCurrentWeek();
    selectedDay.value = currentWeek.value.monday;
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
      selectedDay.value = currentWeek.value.monday;
    }else{
      const previousYearMetadata = await timeTrackClient.getYearMetadata(currentWeek.value.year - 1);
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year - 1,
          previousYearMetadata.maxWeekNumber);
      selectedDay.value = currentWeek.value.monday;
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
        selectedDay.value = currentWeek.value.monday;
      }else{
        currentWeek.value = await timeTrackClient.getWeek(
            currentWeek.value.year + 1,
            1);
        selectedDay.value = currentWeek.value.monday;
      }
    } else {
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year,
          currentWeek.value.weekNumber + 1);
      selectedDay.value = currentWeek.value.monday;
    }
  }
  
  fetchCurrentWeek();
</script>

<template>
  <header>
    <div class="wrapper">
      
      <!-- <HelloWorld msg="Year: {{currentWeek.year}}, Week: {{currentWeek.weekNumber}}" /> -->

      <!-- <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav>-->

      <Card>
        <template #header>
          <div class="navigation-container">
            <h1>Year {{currentWeek?.year}} | Week {{currentWeek?.weekNumber}}</h1>
          </div>
        </template>
        <template #content>
          <div v-if="currentWeek" class="navigation-container">
            <Button label="<--" @click="fetchWeekBeforeThisWeek"/>

            <DayPreviewView v-model="currentWeek.monday"
                            :is-selected="currentWeek.monday == selectedDay"
                            @click="selectMonday" />
            <DayPreviewView v-model="currentWeek.tuesday"
                            :is-selected="currentWeek.tuesday == selectedDay"
                            @click="selectTuesday" />
            <DayPreviewView v-model="currentWeek.wednesday"
                            :is-selected="currentWeek.wednesday == selectedDay"
                            @click="selectWednesday" />
            <DayPreviewView v-model="currentWeek.thursday"
                            :is-selected="currentWeek.thursday == selectedDay"
                            @click="selectThursday" />
            <DayPreviewView v-model="currentWeek.friday"
                            :is-selected="currentWeek.friday == selectedDay"
                            @click="selectFriday" />
            <DayPreviewView v-model="currentWeek.saturday"
                            :is-selected="currentWeek.saturday == selectedDay"
                            @click="selectSaturday" />
            <DayPreviewView v-model="currentWeek.sunday"
                            :is-selected="currentWeek.sunday == selectedDay"
                            @click="selectSunday" />

            <Button label="-->" @click="fetchWeekAfterThisWeek"/>
          </div>
        </template>
      </Card>

      <Card class="selected-day"
            v-if="selectedDay">
        <template #header>
          <h1>Day: {{selectedDay?.date}}</h1>
        </template>
        <template #content>

          <div class="card flex justify-center">
            <Select v-model="selectedDay.type"
                    :options="dayTypeValues"
                    placeholder="Select a day type"
                    class="w-full md:w-56" />

            <DataTable :value="selectedDay.entries" tableStyle="min-width: 50rem">
              <Column field="topic.category" header="Category"></Column>
              <Column field="topic.name" header="Name"></Column>
              <Column field="effortInHours" header="Effort (h)"></Column>
              <Column field="effortBilled" header="Billed (h)"></Column>
              <Column field="description" header="Description"></Column>
            </DataTable>
            
          </div>

        </template>
        <template #footer>
          <div class="flex gap-4 mt-1">
            <Button label="Cancel" severity="secondary" outlined class="w-full" />
            <Button label="Save" class="w-full" />
          </div>
        </template>
      </Card>
      
      
    </div>
  </header>
  
</template>

<style scoped>
div.navigation-container{
  display: flex;
  justify-content: center;
}

button{
  margin: 1rem;
}

.selected-day{
  max-width: 35rem;
}
</style>
