import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { TimeTrackClient } from "@/services/time-track-client.generated";
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import Button from "primevue/button"
import Card from 'primevue/card';

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
});

app.use(createPinia())
app.provide("TimeTrackClient", new TimeTrackClient());
app.use(router)

// Components from PrimeVue
app.component('Button', Button);
app.component('Card', Card);

app.mount('#app')
