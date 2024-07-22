<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import {required, minValue} from "@vuelidate/validators";
  import {useVuelidate} from "@vuelidate/core";
  
  const timeTrackingStore = useTimeTrackingStore();

  const rules = {
    selectedEntry: {
      topic: {
        category: { },
        name: { },
      },
      effortInHours: { required, minValue: minValue(0) },
      effortBilled: { required, minValue: minValue(0) },
      description: { }
    }
  }
  
  const v$ = useVuelidate(rules, timeTrackingStore);
  
</script>

<template>
  <form v-if="timeTrackingStore.selectedEntry">
    <div class="row">
      <div class="col-6 mb-3">
        <label for="current-row-category" class="form-label">Category</label>
        <Select id="selected-entry-category"
                variant="filled"
                v-model="timeTrackingStore.selectedEntry.topicCategory"
                :options="timeTrackingStore.availableTopicCategories"
                @change="timeTrackingStore.selectedEntryCategoryChanged"
                :invalid="v$.selectedEntry.topic.category.$invalid"/>
        <div v-for="error of v$.selectedEntry.topic.category.$silentErrors" :key="error.$uid">
          <div class="error-msg">{{ error.$message }}</div>
        </div>
      </div>
      <div class="col-6 mb-3">
        <label for="current-row-name" class="form-label">Name</label>
        <Select id="selected-entry-category"
                variant="filled"
                v-model="timeTrackingStore.selectedEntry.topicName"
                :options="timeTrackingStore.availableTopicNames"
                :invalid="v$.selectedEntry.topic.name.$invalid"/>
        <div v-for="error of v$.selectedEntry.topic.name.$silentErrors" :key="error.$uid">
          <div class="error-msg">{{ error.$message }}</div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-6 mb-3">
        <label for="current-row-effort" class="form-label">Effort (h)</label>
        <InputNumber id="current-row-effort"
                     v-model.lazy="timeTrackingStore.selectedEntry.effortInHours"
                     :invalid="v$.selectedEntry.effortInHours.$invalid"/>
        <div v-for="error of v$.selectedEntry.effortInHours.$silentErrors" :key="error.$uid">
          <div class="error-msg">{{ error.$message }}</div>
        </div>
      </div>
      <div class="col-6 mb-3">
        <label for="current-row-billed" class="form-label">Billed (h)</label>
        <InputNumber id="current-row-billed"
                     v-model.lazy="timeTrackingStore.selectedEntry.effortBilled"
                     :invalid="v$.selectedEntry.effortBilled.$invalid"/>
        <div v-for="error of v$.selectedEntry.effortBilled.$silentErrors" :key="error.$uid">
          <div class="error-msg">{{ error.$message }}</div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-12 mb-3">
        <label for="current-row-description" class="form-label">Description</label>
        <Textarea id="current-row-description"
                  v-model.lazy="timeTrackingStore.selectedEntry.description"
                  rows="6"
                  :invalid="v$.selectedEntry.description.$invalid"/>
        <div v-for="error of v$.selectedEntry.description.$silentErrors" :key="error.$uid">
          <div class="error-msg">{{ error.$message }}</div>
        </div>
      </div>
    </div>
  </form>
</template>

<style scoped>
  input.p-inputtext, span.p-inputnumber, textarea.p-textarea, div.p-select{
    width: 100%;
  }
  
  div.error-msg{
    color: red;
  }
</style>