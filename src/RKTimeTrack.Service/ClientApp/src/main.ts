import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { TimeTrackClient } from "@/services/time-track-client.generated";
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';

// Components from PrimeVue
import Button from "primevue/button"
import Card from 'primevue/card';
import Select from 'primevue/select';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';

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
app.component('Select', Select);
app.component('DataTable', DataTable);
app.component('Column', Column);

app.mount('#app')
