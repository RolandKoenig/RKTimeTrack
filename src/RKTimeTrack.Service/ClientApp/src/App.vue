<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import WeekSelectionView from "@/views/WeekSelectionView.vue";
  import DayEntrySelectionView from "@/views/DayEntrySelectionView.vue";
  
  const timeTrackingStore = useTimeTrackingStore();
  
  timeTrackingStore.fetchInitialData();
  
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
    
    <div v-if="timeTrackingStore.selectedDay"
         class="row py-4">
      <h4>{{  timeTrackingStore.selectedDay.date }}</h4>
      <form>
        <div class="row">
          <div class="col-4 mb-3">
            <label for="selected-day-type" class="form-label">Type</label>
            <Select id="selected-day-type"
                    variant="filled"
                    v-model="timeTrackingStore.selectedDay.type"
                    :options="timeTrackingStore.dayTypeValues"
                    @change="timeTrackingStore.pushCurrentDayToServer" />
          </div>
        </div>
      </form>
      
      <DayEntrySelectionView />
    </div>
    
    <div class="row py-4"
         v-if="timeTrackingStore.selectedEntry">
      <form>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-category" class="form-label">Category</label>
            <Select id="selected-entry-category"
                    variant="filled"
                    v-model="timeTrackingStore.selectedEntry.topic.category"
                    :options="timeTrackingStore.availableTopicCategories"
                    @change="timeTrackingStore.selectedEntryCategoryChanged"/>
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-name" class="form-label">Name</label>
            <Select id="selected-entry-category"
                    variant="filled"
                    v-model="timeTrackingStore.selectedEntry.topic.name"
                    :options="timeTrackingStore.availableTopicNames"
                    @change="timeTrackingStore.pushCurrentDayToServer" />
          </div>
        </div>
        <div class="row">
          <div class="col-6 mb-3">
            <label for="current-row-effort" class="form-label">Effort (h)</label>
            <InputNumber id="current-row-effort" 
                         v-model="timeTrackingStore.selectedEntry.effortInHours"
                         @change="timeTrackingStore.pushCurrentDayToServer" />
          </div>
          <div class="col-6 mb-3">
            <label for="current-row-billed" class="form-label">Billed (h)</label>
            <InputNumber id="current-row-billed" 
                         v-model="timeTrackingStore.selectedEntry.effortBilled"
                         @change="timeTrackingStore.pushCurrentDayToServer" />
          </div>
        </div>
        <div class="row">
          <div class="col-12 mb-3">
            <label for="current-row-description" class="form-label">Description</label>
            <Textarea id="current-row-description" 
                      v-model="timeTrackingStore.selectedEntry.description"
                      rows="6"
                      @change="timeTrackingStore.pushCurrentDayToServer" />
          </div>
        </div>
      </form>
    </div>

  </div>
  
</template>

<style scoped>
input.p-inputtext, span.p-inputnumber, textarea.p-textarea, div.p-select{
  width: 100%;
}
</style>
