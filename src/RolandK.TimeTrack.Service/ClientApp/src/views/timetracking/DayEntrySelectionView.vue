﻿<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import IconPlus from "@/components/icons/IconPlus.vue";
  import IconDelete from "@/components/icons/IconDelete.vue";
  import IconCopy from "@/components/icons/IconCopy.vue";
  import IconCalendarDay from "@/components/icons/IconCalendarDay.vue";
  import DescriptionTextElement from "@/views/timetracking/DescriptionTextElement.vue";
  import type {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";
  import type {DataTableRowReorderEvent} from "primevue/datatable";
  import IconTraining from "@/components/icons/IconTraining.vue";
  import IconOnCall from "@/components/icons/IconOnCall.vue";
    
  const timeTrackingStore = useTimeTrackingStore();
  
  function copyToClipboard(dataToCopy: string){
    navigator.clipboard.writeText(dataToCopy);
  }

  const onRowReorder = (event : DataTableRowReorderEvent) => {
    timeTrackingStore.applyNewEntryCollection(event.value as UiTimeTrackingEntry[]);
  };
</script>

<template>
  <DataTable v-if="timeTrackingStore.selectedDay"
             v-model:selection="timeTrackingStore.selectedEntry"
             :value="timeTrackingStore.selectedDay.entries"
             :size="'small'"
             selectionMode="single"
             editMode="cell"
             @rowReorder="onRowReorder">
    <template #header>
      <Toolbar>
        <template #start>
          <Button outlined
                  class="me-2"
                  @click="timeTrackingStore.addNewEntry">
            <IconPlus size="small" /> New Entry
          </Button>
          <Button outlined
                  class="me-2"
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

    <Column rowReorder headerStyle="width: 3rem" :reorderableColumn="false" />
    <Column field="topicCategory"
            header="Category"
            style="width: 12%"></Column>
    <Column field="topicName"
            header="Name"
            style="width: 12%"></Column>
    <Column field="effortInHours"
            header="Effort (h)"
            style="width: 6%" />
    <Column field="effortBilled"
            header="Billed (h)"
            style="width: 6%">
      <template #body="slotProps">
        <span v-if="slotProps.data.effortBilled != 0">{{ slotProps.data.effortBilled}}</span>
      </template>
    </Column>
    <Column field="description"
            header="Description"
            style="width: 64%">
      <template #body="slotProps">
        <div class="cell-container">
          <DescriptionTextElement class="row-content"
                                  :description="slotProps.data.description" />

          <!-- Mark training entries with an icon -->
          <Button v-if="slotProps.data.type == 'Training'"
                  text
                  disabled>
            <IconTraining size="small" />
          </Button>
          
          <!-- Mark on-call entries with an icon -->
          <Button v-if="slotProps.data.type == 'OnCall'"
                  text
                  disabled>
            <IconOnCall size="small" />
          </Button>
          
          <!-- Copy button -->
          <Button text
                  @click="copyToClipboard(slotProps.data.description)">
            <IconCopy size="small"/>
          </Button>
        </div>
      </template>
    </Column>
  </DataTable>
</template>

<style scoped>
  div.cell-container{
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  
  .row-content{
    width:100%;
  }
</style>