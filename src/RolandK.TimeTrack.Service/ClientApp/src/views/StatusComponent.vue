<script setup lang="ts">
  import {useStatusStore} from "@/stores/status-store.ts";

  const statusStore = useStatusStore();

  function formatDateTime(value: Date | null | undefined): string {
    if (!value) { return "—"; }
    return value.toLocaleString();
  }
</script>

<template>
  <div>
    <p>
      Exported: {{ formatDateTime(statusStore.applicationState?.lastSuccessfulExport) }} <br />
      Started: {{ formatDateTime(statusStore.applicationState?.serviceStartupTimestamp) }} 
    </p>
  </div>

</template>

<style scoped>

</style>