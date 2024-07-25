<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import {required, minValue} from "@vuelidate/validators";
  import {useVuelidate} from "@vuelidate/core";
  import {computed} from "vue";
  
  const timeTrackingStore = useTimeTrackingStore();
  
  const wrappedEffortInHours = computed({
    get() {
      return timeTrackingStore.selectedEntry?.effortInHours ?? 0;
    },
    set(newValue) {
      if(timeTrackingStore.selectedEntry){
        timeTrackingStore.selectedEntry.effortInHours = Math.round(newValue * 4) / 4;
      }
    }
  })
  
  const wrappedEffortBilled = computed({
    get() {
      return timeTrackingStore.selectedEntry?.effortBilled ?? 0;
    },
    set(newValue) {
      if(timeTrackingStore.selectedEntry){
        timeTrackingStore.selectedEntry.effortBilled = Math.round(newValue * 4) / 4;
      }
    }
  })
  
  // Configure validation
  const validationRules = {
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
  const v$ = useVuelidate(validationRules, timeTrackingStore);
  
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
                     v-model.lazy="wrappedEffortInHours"
                     minFractionDigits="0"
                     maxFractionDigits="2"
                     :invalid="v$.selectedEntry.effortInHours.$invalid"
                     showButtons step="0.25"/>
        <div v-for="error of v$.selectedEntry.effortInHours.$silentErrors" :key="error.$uid">
          <div class="error-msg">{{ error.$message }}</div>
        </div>
      </div>
      <div class="col-6 mb-3">
        <label for="current-row-billed" class="form-label">Billed (h)</label>
        <InputNumber id="current-row-billed"
                     v-model.lazy="wrappedEffortBilled"
                     minFractionDigits="0"
                     maxFractionDigits="2"
                     :invalid="v$.selectedEntry.effortBilled.$invalid"
                     showButtons step="0.25"/>
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