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
                style="width: 40%"
                sortable/>
        <Column field="name"
                header="Name"
                style="width: 40%"
                sortable/>
        <Column field="budget"
                header="Budget"
                style="width: 20%"/>
      </DataTable>
    </div>
  </div>
</template>

<style scoped>

</style>