<script setup lang="ts">
  import {useStatusStore} from "@/stores/status-store.ts";

  const statusStore = useStatusStore();

  function formatTimestamp(value: string | Date | null | undefined): string {
    if (!value) { return "-"; }

    const date = value instanceof Date ? value : new Date(value);
    if (isNaN(date.getTime())) { return "-"; }

    const pad = (n: number) => n.toString().padStart(2, '0');

    const year = date.getFullYear();
    const month = pad(date.getMonth() + 1);
    const day = pad(date.getDate());
    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());

    return `${year}-${month}-${day} ${hours}:${minutes}`;
  }
</script>

<template>
  <div class="container footer-info">
    <span v-if="statusStore.applicationStateError" class="text-danger">Not connected</span>
    <template v-else>
      <span>Started: {{ formatTimestamp(statusStore.applicationState?.serviceStartupTimestamp) }}</span>
      <span>Last export: {{ formatTimestamp(statusStore.applicationState?.lastSuccessfulExport) }}</span>
    </template>
  </div>
</template>

<style scoped>
  div.footer-info{
    display: flex;
    flex-wrap: wrap;
    flex-direction: row;
    justify-content: center;
    gap: 1rem;
    margin-top: 1rem;
    margin-bottom: 1rem;
  }
</style>