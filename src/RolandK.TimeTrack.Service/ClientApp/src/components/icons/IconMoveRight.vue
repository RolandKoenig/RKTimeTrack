<script setup lang="ts">
  import {useIconSizeMapping} from "@/components/icons/icon-defaults";
  
  const props = defineProps({
    size: {
      type: String,
      default: "normal"
    }});
  
  const {width, height} = useIconSizeMapping(props.size);
</script>

<template>
  <svg xmlns="http://www.w3.org/2000/svg"
       :width="width" :height="height"
       viewBox="0 0 512 512"
       fill="currentColor"
       data-testid="icon-move-right">
    <!--!Font Awesome Free 6.5.2 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license/free Copyright 2024 Fonticons, Inc.-->
    <path d="M334.5 414c8.8 3.8 19 2 26-4.6l144-136c4.8-4.5 7.5-10.8 7.5-17.4s-2.7-12.9-7.5-17.4l-144-136c-7-6.6-17.2-8.4-26-4.6s-14.5 12.5-14.5 22l0 72L32 192c-17.7 0-32 14.3-32 32l0 64c0 17.7 14.3 32 32 32l288 0 0 72c0 9.6 5.7 18.2 14.5 22z" />
  </svg>
</template>