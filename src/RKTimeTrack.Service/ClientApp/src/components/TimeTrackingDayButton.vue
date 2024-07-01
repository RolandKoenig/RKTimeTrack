<script setup lang="ts">
  import {TimeTrackingDay} from "@/services/time-track-client.generated";
  import {computed} from "vue";

  const model = defineModel<TimeTrackingDay | undefined>();
  const props = defineProps({
    isSelected: Boolean
  });

  const daytypeCssClass = computed(() => {
    if(!model.value){ return "" }
    
    let result = `daytype-${model.value.type.toLowerCase()}`;
    if(props.isSelected){
      result += " selected";
    }
    
    return result;
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
  <div class="buttonBorder"
       :class="daytypeCssClass">
    <Button :class="daytypeCssClass">
      <div class="daytype" >
        <span>{{sumEffort}}</span>
      </div>
    </Button>
  </div>
</template>

<style scoped>
  div.daytype{
    width:3rem;
    height: 3rem;
  }
  
  div.buttonBorder{
    border-width: 4px;
    border-radius: 4px;
    border-color: white;
    border-style: solid;
    background: transparent;
  }
  
  div.selected{
    border-color: #AAAAAA;
    background: #AAAAAA;
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
    color: black;
  }

  button.daytype-compensatorytimeoff{
    background-color: yellow;
    border-color: yellow;
    color: black;
  }
  
  button.daytype-weekend{
    background-color: sandybrown;
    border-color: sandybrown;
  }
</style>