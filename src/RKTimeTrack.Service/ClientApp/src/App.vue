<script setup lang="ts">
  import {ref} from "vue";
  
  const menuItems = ref([{
      label: 'TimeTracking',
      icon: 'pi pi-home',
      route: '/'
    },{
      label: 'Topics',
      icon: 'pi pi-home',
      route: '/topics'
    }]);
</script>

<template>
  <Menubar :model="menuItems">
    <template #start>
      <img width="40" height="40" src="/RKTimeTrack.svg" alt="">
    </template>
    <template #item="{ item, props }">
      <router-link v-slot="{ href, navigate }" :to="item.route" custom>
        <a :href="href" v-bind="props.action" @click="navigate">
          <span :class="item.icon"></span>
          <span class="ml-2">{{ item.label }}</span>
        </a>
      </router-link>
    </template>
  </Menubar>
  
  <RouterView />
  
  <Toast />
</template>

<style scoped>

</style>
