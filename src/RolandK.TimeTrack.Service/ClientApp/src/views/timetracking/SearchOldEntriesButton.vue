<script setup lang="ts">
  import {ref} from "vue";
  import {useSearchOldEntriesStore} from "@/stores/search-old-entries-store";
  import type {DataTableRowClickEvent} from "primevue/datatable";
  import type {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";
  import IconSearch from "@/components/icons/IconSearch.vue";
  
  const emit = defineEmits<{
    descriptionSelected: [description: string]
  }>()
  
  const store = useSearchOldEntriesStore();
  const dialogVisible = ref(false);
  
  function showDialog(){
    dialogVisible.value = true
  }
  
  function rowClicked(event: DataTableRowClickEvent){
    const selectedData = event.data as UiTimeTrackingEntry;
    if(!selectedData.description){ return; }
    
    emit('descriptionSelected', selectedData.description);
    dialogVisible.value = false;
  }
</script>

<template>
  <Button text
          @click="showDialog">
    <IconSearch size="small" />
  </Button>

  <Dialog v-model:visible="dialogVisible" modal header="Search old entries" :style="{ width: '50rem' }">
    <div class="row inputRow">
      <label for="search-string"
             class="form-label">Search string</label>
      <InputText id="search-string"
                 autofocus
                 v-model="store.searchString"/>
    </div>
    <div v-if="store.searchResults.length" 
         class="row tableRow">
      <DataTable :value="store.searchResults"
                 scrollable scrollHeight="19rem"
                 size="small"
                 selectionMode="single"
                 @row-click="rowClicked">
        <Column field="description"
                header="Description"></Column>
      </DataTable>
    </div>
    <div v-else
         class="row tableRow">
      <p style="margin-left: 0.5rem">No results found</p>
    </div>
    <div class="row justify-content-md-end" >
      <div class="col-3">
        <Button type="button"
                label="Close"
                severity="secondary"
                style="width: 100%"
                @click="dialogVisible = false" />
      </div>
    </div>
  </Dialog>
</template>

<style scoped>
  .row{
    margin-top: 0.5rem;
  }
  
  .inputRow{
    margin-left: 0.2rem;
    margin-right: 0.1rem;
  }

  .tableRow{
    height: 20rem;
  }
</style>