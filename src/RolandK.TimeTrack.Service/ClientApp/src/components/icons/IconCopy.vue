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
       viewBox="0 0 448 512"
       fill="currentColor"
       data-testid="icon-copy">
    <!--!Font Awesome Free 6.5.2 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license/free Copyright 2024 Fonticons, Inc.-->
    <path d="M384 336H192c-8.8 0-16-7.2-16-16V64c0-8.8 7.2-16 16-16l140.1 0L400 115.9V320c0 8.8-7.2 16-16 16zM192 384H384c35.3 0 64-28.7 64-64V115.9c0-12.7-5.1-24.9-14.1-33.9L366.1 14.1c-9-9-21.2-14.1-33.9-14.1H192c-35.3 0-64 28.7-64 64V320c0 35.3 28.7 64 64 64zM64 128c-35.3 0-64 28.7-64 64V448c0 35.3 28.7 64 64 64H256c35.3 0 64-28.7 64-64V416H272v32c0 8.8-7.2 16-16 16H64c-8.8 0-16-7.2-16-16V192c0-8.8 7.2-16 16-16H96V128H64z"/>
  </svg>
</template>