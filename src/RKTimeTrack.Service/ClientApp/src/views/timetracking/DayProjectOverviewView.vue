<script setup lang="ts">
  import {useTimeTrackingStore} from "@/stores/time-tracking-store";
  import {computed} from "vue";

  const timeTrackingStore = useTimeTrackingStore();
  
  const projects = computed(() => {
    if(!timeTrackingStore.selectedDay){ return []; }
    
    const summary: any = {};
    for(const actEntry of timeTrackingStore.selectedDay.entries){
      if(!actEntry.topicName){ continue; }
      if(!actEntry.topicCategory){ continue; }
      if(!actEntry.effortBilled){ continue; }
      
      const key = `${actEntry.topicCategory} - ${actEntry.topicName}`;
      if(!summary[key]){
        summary[key] = actEntry.effortBilled
      }else{
        summary[key] = summary[key] * actEntry.effortBilled;
      }
    }
    
    const result: ProjectSummary[] = [];
    for(const actProperty in summary){
      result.push({
        projectName: actProperty,
        sumBilled: summary[actProperty]
      })
    }
    
    return result;
  })
  
  interface ProjectSummary{
    projectName: String,
    sumBilled: number,
  }
</script>

<template>
  <DataTable v-if="projects.length"
             :value="projects"
             :size="'small'"
             selectionMode="single"
             editMode="cell">
    <Column field="projectName"
            header="Project"
            style="width: 60%"></Column>
    <Column field="sumBilled"
            header="Sum Billed (h)"
            style="width: 40%"></Column>
  </DataTable>
</template>

<style scoped>

</style>