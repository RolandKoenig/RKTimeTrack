<script setup lang="ts">
  import {useTopicStore} from "@/stores/topic-store";

  const topicStore = useTopicStore();
</script>

<template>
  <div class="container">
    <div class="row pt-5 text-center">
      <h2>Topics</h2>
    </div>
    <div class="row">
      <DataTable v-if="topicStore.topics"
                 :value="topicStore.topics"
                 :size="'small'"
                 selectionMode="single"
                 editMode="cell">
        <Column field="category"
                header="Category"
                style="width: 30%"
                sortable/>
        <Column field="name"
                header="Name"
                style="width: 30%"
                sortable/>
        <Column field="canBeInvoiced"
                header="Can be invoiced?"
                style="width: 10%"/>
        <Column field="budget"
                header="Budget"
                style="width: 10%"/>
        <Column field="startDate"
                header="Start Date"
                style="width: 10%"/>
        <Column field="endDate"
                header="End Date"
                style="width: 10%"/>
      </DataTable>
    </div>
  </div>
</template>

<style scoped>

</style>