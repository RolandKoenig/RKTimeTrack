import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { TimeTrackClient } from "@/services/client.generated";

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia())
app.provide("TimeTrackClient", new TimeTrackClient());
app.use(router)

app.mount('#app')
