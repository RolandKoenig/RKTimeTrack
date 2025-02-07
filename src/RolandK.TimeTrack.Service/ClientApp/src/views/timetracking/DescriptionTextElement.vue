<script setup lang="ts">
  import {computed} from "vue";

  const props = defineProps({
    description: String
  })
  
  const parts = computed(() => {
    if(!props.description ||
       !props.description.startsWith("#")){ 
      return {
        tag: null,
        text: props.description
      }; 
    }
    
    const indexOfFirstBlank = props.description.indexOf(" ");
    if((indexOfFirstBlank < 0) || (indexOfFirstBlank == props.description.length)){
      return {
        tag: props.description,
        text: ""
      };
    }
    
    return {
      tag: props.description.substring(0, indexOfFirstBlank),
      text: props.description.substring(indexOfFirstBlank + 1)
    }
  })
  
</script>

<template>
  <div>
    <Tag v-if="parts.tag"
         severity="info"
         :value="parts.tag"
         style="margin-right:0.5rem;"/>
    <span>{{ parts.text }}</span>
  </div>
</template>