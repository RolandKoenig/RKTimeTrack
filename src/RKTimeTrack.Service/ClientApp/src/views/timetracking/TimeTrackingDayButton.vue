<script setup lang="ts">
  import {computed} from "vue";
  import {UiTimeTrackingDay} from "@/stores/models/ui-time-tracking-day";
  
  const props = defineProps({
    isSelected: Boolean,
    timeTrackingDay: UiTimeTrackingDay
  });

  const daytypeCssClass = computed(() => {
    if(!props.timeTrackingDay){ return "" }
    
    let result = `daytype-${props.timeTrackingDay.type.toLowerCase()}`;
    if(props.isSelected){
      result += " selected";
    }
    
    return result;
  })
  
  const sumEffort = computed(() => {
    if(!props.timeTrackingDay){ return 0; }
    if(!props.timeTrackingDay.entries){ return 0; }
    
    let sum = 0;
    props.timeTrackingDay.entries.forEach(x => sum+= x.effortInHours);
    return sum;
  })
  
  const sumBilled = computed(() =>{
    if(!props.timeTrackingDay){ return 0; }
    if(!props.timeTrackingDay.entries){ return 0; }

    let sum = 0;
    props.timeTrackingDay.entries.forEach(x => sum+= x.effortBilled);
    return sum;
  });
</script>

<template>
  <div class="buttonBorder"
       :class="daytypeCssClass">
    <Button :class="daytypeCssClass" v-bind="$attrs">
      <div class="daytype" >
        <span>{{sumEffort}}</span><br />
        <span v-if="sumBilled > 0" class="effortBilledSum">{{sumBilled}}</span>
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
  
  span.effortBilledSum{
    opacity: 0.6;
  }
  
  div.selected{
    border-color: #AAAAAA;
    background: #AAAAAA;
  }
  
  button.daytype-workingday{
    /* default button styling */
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