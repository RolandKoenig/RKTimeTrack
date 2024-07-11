<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import IconPlus from "@/components/icons/IconPlus.vue";
  import IconDelete from "@/components/icons/IconDelete.vue";
  import IconCopy from "@/components/icons/IconCopy.vue";
  import IconCalendarDay from "@/components/icons/IconCalendarDay.vue";
  
  const timeTrackingStore = useTimeTrackingStore();
</script>

<template>
  <DataTable v-if="timeTrackingStore.selectedDay"
             v-model:selection="timeTrackingStore.selectedEntry"
             :value="timeTrackingStore.selectedDay.entries"
             :size="'small'"
             selectionMode="single"
             editMode="cell">
    <template #header>
      <Toolbar>
        <template #start>
          <Button outlined
                  class="me-2"
                  @click="timeTrackingStore.addNewEntry">
            <IconPlus size="small" /> New Entry
          </Button>
          <Button outlined
                  :disabled="!timeTrackingStore.selectedEntry"
                  @click="timeTrackingStore.copySelectedEntry">
            <IconCopy size="small" /> Copy Entry
          </Button>

          <Divider layout="vertical" />
          <IconCalendarDay size="small" />
          <Select id="selected-day-type"
                  class="ms-2"
                  variant="filled"
                  v-model="timeTrackingStore.selectedDay.type"
                  :options="timeTrackingStore.dayTypeValues" />
        </template>
        <template #end>
          <Button outlined
                  severity="danger"
                  class="grid-menu-button"
                  :disabled="!timeTrackingStore.selectedEntry"
                  @click="timeTrackingStore.deleteSelectedEntry">
            <IconDelete size="small" /> Delete Entry
          </Button>
        </template>
      </Toolbar>
    </template>
    
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