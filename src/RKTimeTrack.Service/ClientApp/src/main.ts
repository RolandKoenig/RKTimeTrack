// Vue
import { createApp } from 'vue'
import { createPinia } from 'pinia'

// Bootstrap
import "bootstrap/dist/css/bootstrap.min.css"

// PrimeView
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import Button from "primevue/button"
import Card from 'primevue/card';
import DataTable from 'primevue/datatable';
import Divider from "primevue/divider";
import Column from 'primevue/column';
import InputNumber from 'primevue/inputnumber'
import InputText from 'primevue/inputtext'
import Menubar from "primevue/menubar";
import Select from 'primevue/select';
import Tag from 'primevue/tag';
import Textarea from 'primevue/textarea';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import Toolbar from "primevue/toolbar";

// App
import App from './App.vue'
import router from './router'
import { TimeTrackClient } from "@/services/time-track-client.generated";

//############################

const app = createApp(App)

// Configure App
app.use(createPinia())
app.use(ToastService);
app.provide("TimeTrackClient", new TimeTrackClient());
app.use(router)

// Configure PrimeVue
app.use(PrimeVue, {
    theme: {
        preset: Aura,
        options: {
            prefix: 'p',
            darkModeSelector: 'dark',
            cssLayer: false
        }
    }
});
app.component('Button', Button);
app.component('Card', Card);
app.component('Column', Column);
app.component('DataTable', DataTable);
app.component('Divider', Divider);
app.component('InputNumber', InputNumber);
app.component('InputText', InputText);
app.component('Menubar', Menubar);
app.component('Select', Select);
app.component('Tag', Tag);
app.component('Textarea', Textarea);
app.component('Toast', Toast);
app.component('Toolbar', Toolbar);

// Place app inside html
app.mount('#app')
