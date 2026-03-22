<script setup lang="ts">
  import {useBillableEntriesStore} from "@/stores/billable-entries-store";
  import {onMounted} from "vue";

  const billableStore = useBillableEntriesStore();

  onMounted(() => {
    billableStore.fetchBillableEntries();
  });
</script>

<template>
  <div class="container">
    <div class="row pt-5 text-center">
      <h2>Not Billed</h2>
    </div>
    <div class="row">
      <DataTable :value="billableStore.billableEntries"
                 :size="'small'"
                 :loading="billableStore.isLoading"
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
