<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/timeTrackingStore";
  import WeekSelectionView from "@/views/WeekSelectionView.vue";
  import DayEntrySelectionView from "@/views/DayEntrySelectionView.vue";
  
  const timeTrackingStore = useTimeTrackingStore();
  
  timeTrackingStore.fetchCurrentWeek();
  
</script>

<template>
  <div class="container">
    <div class="row py-5 text-center">
      <img class="d-block mx-auto mb-4" src="/RKTimeTrack.svg" alt="" width="48" height="48">
      <h2 v-if="timeTrackingStore.currentWeek">
        Year {{timeTrackingStore.currentWeek.year}}, Week {{timeTrackingStore.currentWeek.weekNumber}}
      </h2>
    </div>
    
    <div class="row">
      <WeekSelectionView />
    </div>
    
    <div v-if="timeTrackingStore.isDaySelected"
         class="row py-4">
      <h4>Booked times</h4>
      <DayEntrySelectionView />
    </div>
    
    <div class="row py-4"
         v-if="timeTrackingStore.selectedEntry">
      <h4>Current row</h4>
      <form>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-category" class="form-label">Category</label>
            <InputText id="current-row-category" v-model="timeTrackingStore.selectedEntry.topic.category" />
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-name" class="form-label">Name</label>
            <InputText id="current-row-name" v-model="timeTrackingStore.selectedEntry.topic.name" />
          </div>
        </div>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-effort" class="form-label">Effort (h)</label>
            <InputNumber id="current-row-effort" 
                         v-model="timeTrackingStore.selectedEntry.effortInHours" />
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-billed" class="form-label">Billed (h)</label>
            <InputNumber id="current-row-billed" 
                         v-model="timeTrackingStore.selectedEntry.effortBilled" />
          </div>
        </div>
        <div class="row">
          <div class="col-12 mb-3">
            <label for="current-row-description" class="form-label">Description</label>
            <Textarea id="current-row-description" 
                      v-model="timeTrackingStore.selectedEntry.description"
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
         v-if="!timeTrackingStore.isEntrySelected">
      <h4>New row</h4>
    </div>

  </div>
  
</template>

<style scoped>
input.p-inputtext, span.p-inputnumber, textarea.p-textarea{
  width: 100%;
}
</style>
