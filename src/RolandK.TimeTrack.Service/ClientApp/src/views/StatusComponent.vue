<script setup lang="ts">
  import {useStatusStore} from "@/stores/status-store.ts";
  import IconServer from "@/components/icons/IconServer.vue";
  import {computed, ref} from "vue";

  const statusStore = useStatusStore();

  const tooltipText = computed(() => {
    if(statusStore.applicationStateError){
      return "Connection error";
    }
    
    if(statusStore.applicationState){
      let result = "Startup timestamp: " + formatDateTime(statusStore.applicationState.serviceStartupTimestamp);
      if(statusStore.applicationState.lastSuccessfulExport){
        result += "\n\nExport timestamp: " + formatDateTime(statusStore.applicationState.lastSuccessfulExport);
      }
      return result;
    }
    return "No state gathered";
  });

  function formatDateTime(value: Date | null | undefined): string {
    if (!value) { return "—"; }
    return value.toLocaleString();
  }
</script>

<template>
  <div v-tooltip.bottom="{ value: tooltipText, showDelay: 500 }">
    <span v-if="statusStore.applicationStateError"
          class="text-danger m-3">
      Nicht verbunden
    </span>
    <span v-if="!statusStore.applicationStateError"
          class="m-3">
      Verbunden
    </span>
    <span >
      <IconServer size="small" />
    </span>
  </div>

</template>

<style scoped>

</style>