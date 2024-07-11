<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";

  const timeTrackingStore = useTimeTrackingStore();
</script>

<template>
  <form v-if="timeTrackingStore.selectedEntry">
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
                :options="timeTrackingStore.availableTopicNames" />
      </div>
    </div>
    <div class="row">
      <div class="col-6 mb-3">
        <label for="current-row-effort" class="form-label">Effort (h)</label>
        <InputNumber id="current-row-effort"
                     v-model.lazy="timeTrackingStore.selectedEntry.effortInHours" />
      </div>
      <div class="col-6 mb-3">
        <label for="current-row-billed" class="form-label">Billed (h)</label>
        <InputNumber id="current-row-billed"
                     v-model.lazy="timeTrackingStore.selectedEntry.effortBilled" />
      </div>
    </div>
    <div class="row">
      <div class="col-12 mb-3">
        <label for="current-row-description" class="form-label">Description</label>
        <Textarea id="current-row-description"
                  v-model.lazy="timeTrackingStore.selectedEntry.description"
                  rows="6" />
      </div>
    </div>
  </form>
</template>

<style scoped>
  input.p-inputtext, span.p-inputnumber, textarea.p-textarea, div.p-select{
    width: 100%;
  }
</style>