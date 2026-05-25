<script setup lang="ts">
  import {useStatusStore} from "@/stores/status-store.ts";
  import IconServer from "@/components/icons/IconServer.vue";

  const statusStore = useStatusStore();
</script>

<template>
  <div :class="{ 'text-danger': statusStore.applicationStateError }">
    <span v-if="statusStore.applicationStateError"
          class="m-3">
      Not connected
    </span>
    <span v-if="!statusStore.applicationStateError"
          class="m-3">
      Connected
    </span>
    <span >
      <IconServer size="small" />
    </span>
  </div>
</template>

<style scoped>

</style>