<script setup lang="ts">
  import DayPreviewView from "@/views/DayPreviewView.vue";
  import {useTimeTrackingStore} from "@/stores/timeTrackingStore";
  
  const timeTrackingStore = useTimeTrackingStore();
  

  // const dayTypeValues = ref([
  //   TimeTrackingDayType.CompensatoryTimeOff,
  //   TimeTrackingDayType.Holiday,
  //   TimeTrackingDayType.Ill,
  //   TimeTrackingDayType.OwnEducation,
  //   TimeTrackingDayType.PublicHoliday,
  //   TimeTrackingDayType.Training,
  //   TimeTrackingDayType.Weekend,
  //   TimeTrackingDayType.WorkingDay
  // ]);
  
  timeTrackingStore.fetchCurrentWeek();
</script>

<template>
  <div class="container">
    <div class="row py-5 text-center">
      <img class="d-block mx-auto mb-4" src="/RKTimeTrack.svg" alt="" width="48" height="48">
      <h2>Year {{timeTrackingStore.currentWeek?.year}}, Week {{timeTrackingStore.currentWeek?.weekNumber}}</h2>
    </div>
    
    <div class="row">
      <div v-if="timeTrackingStore.currentWeek"
           class="navigation-container">
        <Button label="<--" @click="timeTrackingStore.fetchWeekBeforeThisWeek"/>

        <DayPreviewView v-model="timeTrackingStore.currentWeek.monday"
                        :is-selected="timeTrackingStore.currentWeek.monday == timeTrackingStore.selectedDay"
                        @click="timeTrackingStore.selectMonday" />
        <DayPreviewView v-model="timeTrackingStore.currentWeek.tuesday"
                        :is-selected="timeTrackingStore.currentWeek.tuesday == timeTrackingStore.selectedDay"
                        @click="timeTrackingStore.selectTuesday" />
        <DayPreviewView v-model="timeTrackingStore.currentWeek.wednesday"
                        :is-selected="timeTrackingStore.currentWeek.wednesday == timeTrackingStore.selectedDay"
                        @click="timeTrackingStore.selectWednesday" />
        <DayPreviewView v-model="timeTrackingStore.currentWeek.thursday"
                        :is-selected="timeTrackingStore.currentWeek.thursday == timeTrackingStore.selectedDay"
                        @click="timeTrackingStore.selectThursday" />
        <DayPreviewView v-model="timeTrackingStore.currentWeek.friday"
                        :is-selected="timeTrackingStore.currentWeek.friday == timeTrackingStore.selectedDay"
                        @click="timeTrackingStore.selectFriday" />
        <DayPreviewView v-model="timeTrackingStore.currentWeek.saturday"
                        :is-selected="timeTrackingStore.currentWeek.saturday == timeTrackingStore.selectedDay"
                        @click="timeTrackingStore.selectSaturday" />
        <DayPreviewView v-model="timeTrackingStore.currentWeek.sunday"
                        :is-selected="timeTrackingStore.currentWeek.sunday == timeTrackingStore.selectedDay"
                        @click="timeTrackingStore.selectSunday" />

        <Button label="-->" @click="timeTrackingStore.fetchWeekAfterThisWeek"/>
      </div>
    </div>
    
    <div v-if="timeTrackingStore.selectedDay"
         class="row py-4">
      <h4>Booked times</h4>
      <DataTable v-model:selection="timeTrackingStore.selectedRow"
                 :value="timeTrackingStore.selectedDay.entries"
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
         v-if="timeTrackingStore.selectedRow && timeTrackingStore.selectedRow.topic">
      <h4>Current row</h4>
      <form>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-category" class="form-label">Category</label>
            <InputText id="current-row-category" v-model="timeTrackingStore.selectedRow.topic.category" />
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-name" class="form-label">Name</label>
            <InputText id="current-row-name" v-model="timeTrackingStore.selectedRow.topic.name" />
          </div>
        </div>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-effort" class="form-label">Effort (h)</label>
            <InputNumber id="current-row-effort" 
                         v-model="timeTrackingStore.selectedRow.effortInHours" />
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-billed" class="form-label">Billed (h)</label>
            <InputNumber id="current-row-billed" 
                         v-model="timeTrackingStore.selectedRow.effortBilled" />
          </div>
        </div>
        <div class="row">
          <div class="col-12 mb-3">
            <label for="current-row-description" class="form-label">Description</label>
            <Textarea id="current-row-description" 
                      v-model="timeTrackingStore.selectedRow.description"
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
         v-if="!timeTrackingStore.selectedRow">
      <h4>New row</h4>
    </div>

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
