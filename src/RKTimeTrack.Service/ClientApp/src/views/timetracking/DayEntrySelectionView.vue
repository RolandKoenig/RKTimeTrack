<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import IconPlusSmall from "@/components/icons/IconPlusSmall.vue";
  import IconDeleteSmall from "@/components/icons/IconDeleteSmall.vue";
  
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
                  class="grid-menu-button"
                  @click="timeTrackingStore.addNewEntry">
            <IconPlusSmall /> New Entry
          </Button>
          <Select id="selected-day-type"
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
            <IconDeleteSmall /> Delete Entry
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
  .grid-menu-button{
    margin-right: 1rem;
  }
</style>