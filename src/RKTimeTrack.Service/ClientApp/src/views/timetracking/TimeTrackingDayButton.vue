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

  const tooltipText = computed(() =>{
    if(!props.timeTrackingDay){ return ""; }
    
    let result = `${props.timeTrackingDay.type}`;
    if(props.timeTrackingDay.entries.length > 0){
      const sumEffort = props.timeTrackingDay.entries
          .map(actEntry => actEntry.effortInHours)
          .reduce((sum, currentValue) => sum + currentValue);
      if(sumEffort > 0){ result += `\nEffort: ${sumEffort} h` }
      
      const sumBilled = props.timeTrackingDay.entries
          .map(actEntry => actEntry.effortBilled)
          .reduce((sum, currentValue) => sum + currentValue);
      if(sumBilled > 0){ result += `\nBilled: ${sumBilled} h` }
    }
    
    return result;
  })
  
  const daytypeSeverity = computed(() =>{
    if(!props.timeTrackingDay){ return "secondary"; }
    
    switch(props.timeTrackingDay.type){
      case TimeTrackingDayType.CompensatoryTimeOff: return "help";
      case TimeTrackingDayType.Holiday: return "help";
      case TimeTrackingDayType.Ill: return "danger";
      case TimeTrackingDayType.OwnEducation: return "info";
      case TimeTrackingDayType.PublicHoliday: return "help";
      case TimeTrackingDayType.Training: return "info";
      case TimeTrackingDayType.Weekend: return "warn";
      case TimeTrackingDayType.WorkingDay: return "secondary";
      default: return "secondary";
    }
  })

  const daytypeAbbreviation = computed(() => {
    if(!props.timeTrackingDay){ return ""; }

    switch(props.timeTrackingDay.type){
      case TimeTrackingDayType.CompensatoryTimeOff: return "ZA";
      case TimeTrackingDayType.Holiday: return "UT";
      case TimeTrackingDayType.Ill: return "KT";
      case TimeTrackingDayType.OwnEducation: return "BT";
      case TimeTrackingDayType.PublicHoliday: return "FT";
      case TimeTrackingDayType.Training: return "ST";
      case TimeTrackingDayType.Weekend: return "WE";
      case TimeTrackingDayType.WorkingDay: return "";
      default: return "";
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
            v-tooltip="{ value: tooltipText, showDelay: 500 }"
            raised>
      <div class="parent">
        <div class="daytype" >
          <span>{{sumEffort}}</span><br />
          <span v-if="sumBilled > 0" class="effortBilledSum">{{sumBilled}}</span>
        </div>
        <div class="daytype">
          <div class="text-end daytype-abbreviation">{{ daytypeAbbreviation }}</div>
        </div>
      </div>
    </Button>
  </div>
</template>

<style scoped>
  div.parent{
    width: 3rem;
    height: 3rem;
    position: relative;
  }

  div.daytype{
    width: 3rem;
    height: 3rem;
    position: absolute;
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
  
  div.daytype-abbreviation{
    margin: -8px;
    font-size: 8pt;
    opacity: 0.7;
  }
</style>