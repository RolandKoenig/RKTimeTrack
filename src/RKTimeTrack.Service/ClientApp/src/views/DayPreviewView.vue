<script setup lang="ts">
  import {TimeTrackingDay} from "@/services/time-track-client.generated";
  import {computed} from "vue";

  const model = defineModel<TimeTrackingDay | undefined>();

  const daytypeCssClass = computed(() => {
    if(!model.value){ return "" }
    return `daytype-${model.value.type.toLowerCase()}`;
  })
  
  const sumEffort = computed(() => {
    if(!model.value){ return 0; }
    if(!model.value.entries){ return 0; }
    
    let sum = 0;
    model.value.entries.forEach(x => sum+= x.effortInHours);
    return sum;
  })
</script>

<template>
  <Button :class="daytypeCssClass">
    <div class="daytype" >
      <span>{{sumEffort}}</span>
    </div>
  </Button>
</template>

<style scoped>
  div.daytype{
    width:3rem;
    height: 3rem;
  }

  button.daytype-workingday{
  }

  button.daytype-owneducation{
    background-color: green;
    border-color: green;
  }

  button.daytype-publicholiday{
    background-color: indianred;
    border-color: indianred;
  }

  button.daytype-ill{
    background-color: gray;
    border-color: gray;
  }

  button.daytype-training{
    background-color: dodgerblue;
    border-color: dodgerblue;
  }
  
  button.daytype-holiday{
    background-color: yellow;
    border-color: yellow;
  }
  
  button.daytype-weekend{
    background-color: sandybrown;
    border-color: sandybrown;
  }
</style>