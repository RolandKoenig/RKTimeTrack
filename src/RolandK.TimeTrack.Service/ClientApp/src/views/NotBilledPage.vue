<script setup lang="ts">
  import {useNotBilledEntriesStore} from "@/stores/not-billed-entries-store.ts";
  import {onMounted} from "vue";

  const notBilledEntriesStore = useNotBilledEntriesStore();
  
  onMounted(() => {
    notBilledEntriesStore.fetchBillableEntries();
  });
</script>

<template>
  <div class="container">
    <div class="row pt-5 mb-3">
      <div class="col">
        <h2>Not Billed</h2>
      </div>
    </div>

    <div class="row">
      <DataTable :value="notBilledEntriesStore.notBilledEntries"
                 :size="'small'"
                 :loading="notBilledEntriesStore.isLoading"
                 selectionMode="single">
        <Column field="topicCategory"
                header="Category"
                style="width: 20%"
                sortable/>
        <Column field="topicName"
                header="Topic"
                style="width: 20%"
                sortable/>
        <Column field="effortBilled"
                header="Effort Billed (h)"
                style="width: 10%"
                sortable/>
        <Column field="description"
                header="Description"
                style="width: 50%"/>
      </DataTable>
    </div>
  </div>
</template>

<style scoped>

</style>