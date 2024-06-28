<script setup lang="ts">
import {inject, type Ref, ref} from "vue";
import {
  TimeTrackClient,
  TimeTrackingDay,
  TimeTrackingDayType, TimeTrackingRow,
  TimeTrackingWeek
} from "@/services/time-track-client.generated";
import DayPreviewView from "@/views/DayPreviewView.vue";

const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
  const currentWeek: Ref<TimeTrackingWeek | undefined> = ref();
  const selectedDay: Ref<TimeTrackingDay | undefined> = ref();
  const selectedRow: Ref<TimeTrackingRow | undefined> = ref();

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
      selectedRow.value = undefined;
    }else{
      const previousYearMetadata = await timeTrackClient.getYearMetadata(currentWeek.value.year - 1);
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year - 1,
          previousYearMetadata.maxWeekNumber);
      selectedDay.value = currentWeek.value.monday;
      selectedRow.value = undefined;
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
      selectedDay.value = currentWeek.value.monday;
      selectedRow.value = undefined;
    } else if(currentWeek.value.weekNumber === 52){
      const actYearMetadata = await timeTrackClient.getYearMetadata(currentWeek.value.year);
      if(actYearMetadata.maxWeekNumber === 53){
        currentWeek.value = await timeTrackClient.getWeek(
            currentWeek.value.year,
            currentWeek.value.weekNumber + 1);
        selectedDay.value = currentWeek.value.monday;
        selectedRow.value = undefined;
      }else{
        currentWeek.value = await timeTrackClient.getWeek(
            currentWeek.value.year + 1,
            1);
        selectedDay.value = currentWeek.value.monday;
        selectedRow.value = undefined;
      }
    } else {
      currentWeek.value = await timeTrackClient.getWeek(
          currentWeek.value.year,
          currentWeek.value.weekNumber + 1);
      selectedDay.value = currentWeek.value.monday;
      selectedRow.value = undefined;
    }
  }
  
  fetchCurrentWeek();
</script>

<template>
  <div class="container">
    <div class="row py-5 text-center">
      <img class="d-block mx-auto mb-4" src="/RKTimeTrack.svg" alt="" width="48" height="48">
      <h2>Year {{currentWeek?.year}}, Week {{currentWeek?.weekNumber}}</h2>
    </div>
    
    <div class="row">
      <div v-if="currentWeek"
           class="navigation-container">
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
    </div>
    
    <div v-if="selectedDay"
         class="row py-4">
      <h4>Booked times</h4>
      <DataTable v-model:selection="selectedRow"
                 :value="selectedDay.entries"
                 :size="'small'"
                 selectionMode="single"
                 editMode="cell">
        <Column selectionMode="single" 
                headerStyle="width: 3rem"></Column>
        <Column field="topic.category" 
                header="Category"
                style="width: 12%"></Column>
        <Column field="topic.name" 
                header="Name"
                style="width: 12%"></Column>
        <Column field="effortInHours" 
                header="Effort (h)"
                style="width: 6%"></Column>
        <Column field="effortBilled" 
                header="Billed (h)"
                style="width: 6%"></Column>
        <Column field="description" 
                header="Description" 
                style="width: 64%"></Column>
      </DataTable>
    </div>
    
    <div class="row py-4"
         v-if="selectedRow && selectedRow.topic">
      <h4>Current row</h4>
      <form>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-category" class="form-label">Category</label>
            <InputText id="current-row-category" v-model="selectedRow.topic.category" />
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-name" class="form-label">Name</label>
            <InputText id="current-row-name" v-model="selectedRow.topic.name" />
          </div>
        </div>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-effort" class="form-label">Effort (h)</label>
            <InputNumber id="current-row-effort" 
                         v-model="selectedRow.effortInHours" />
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-billed" class="form-label">Billed (h)</label>
            <InputNumber id="current-row-billed" 
                         v-model="selectedRow.effortBilled" />
          </div>
        </div>
        <div class="row">
          <div class="col-12 mb-3">
            <label for="current-row-description" class="form-label">Description</label>
            <Textarea id="current-row-description" 
                      v-model="selectedRow.description"
                      rows="6"/>
          </div>
        </div>
        <div class="row">
          <Button label="Cancel" severity="secondary" class="w-25" outlined />
          <Button label="Save" class="w-25" />
        </div>
      </form>
      

    </div>

    <div class="row py-4"
         v-if="!selectedRow">
      <h4>New row</h4>
    </div>

    <!-- 
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
    -->

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

input.p-inputtext, span.p-inputnumber, textarea.p-textarea{
  width: 100%;
}
</style>
