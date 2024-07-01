<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/timeTrackingStore";
  
  const timeTrackingStore = useTimeTrackingStore();
</script>

<template>
  <DataTable v-if="timeTrackingStore.isDaySelected"
             v-model:selection="timeTrackingStore.selectedEntry"
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
</template>

<style scoped>

</style>