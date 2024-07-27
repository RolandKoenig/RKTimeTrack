<script setup lang="ts">
import {computed} from "vue";
import {UiTimeTrackingDay} from "@/stores/models/ui-time-tracking-day";
import {TimeTrackingDayType} from "@/services/time-track-client.generated";

const props = defineProps({
    isSelected: Boolean,
    timeTrackingDay: UiTimeTrackingDay
  });

  const selectionCssClass = computed(() => {
    if(props.isSelected){ return "selected"; }
    return "";
  })
  
  const daytypeSeverity = computed(() =>{
    if(!props.timeTrackingDay){ return "secondary"; }
    
    switch(props.timeTrackingDay.type){
      case TimeTrackingDayType.CompensatoryTimeOff:
        return "help";
      case TimeTrackingDayType.Holiday:
        return "help";
      case TimeTrackingDayType.Ill:
        return "danger";
      case TimeTrackingDayType.OwnEducation:
        return "info";
      case TimeTrackingDayType.PublicHoliday:
        return "help";
      case TimeTrackingDayType.Training:
        return "info";
      case TimeTrackingDayType.Weekend:
        return "warn";
      case TimeTrackingDayType.WorkingDay:
        return "secondary";
      default:
        return "secondary";
    }
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
       :class="selectionCssClass">
    <Button :severity="daytypeSeverity" 
            v-bind="$attrs"
            raised>
      <div class="daytype" >
        <span>{{sumEffort}}</span><br />
        <span v-if="sumBilled > 0" class="effortBilledSum">{{sumBilled}}</span>
      </div>
    </Button>
  </div>
</template>

<style scoped>
  div.daytype{
    width: 3rem;
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
</style>