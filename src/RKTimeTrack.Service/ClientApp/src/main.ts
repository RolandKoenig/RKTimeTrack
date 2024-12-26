// Vue
import { createApp } from 'vue'
import { createPinia } from 'pinia'

// Bootstrap
import "bootstrap/dist/css/bootstrap.min.css"

// PrimeView
import { definePreset } from "@primevue/themes";
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import Button from "primevue/button"
import Card from 'primevue/card';
import DataTable from 'primevue/datatable';
import Dialog from 'primevue/dialog';
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
import Tooltip from 'primevue/tooltip';

// App
import App from './App.vue'
import router from './router'
import { TimeTrackClient } from "@/services/time-track-client.generated";
import { handleColorMode } from "@/util/color-mode-handling";

//############################

const app = createApp(App)

// Configure App
app.use(createPinia())
app.use(ToastService);
app.provide("TimeTrackClient", new TimeTrackClient());
app.use(router)

app.directive('tooltip', Tooltip);

// Configure theme
const MyThemePreset = definePreset(Aura, {
    semantic: {
        primary: {
            50: '{sky.50}',
            100: '{sky.100}',
            200: '{sky.200}',
            300: '{sky.300}',
            400: '{sky.400}',
            500: '{sky.500}',
            600: '{sky.600}',
            700: '{sky.700}',
            800: '{sky.800}',
            900: '{sky.900}',
            950: '{sky.950}'
        }
    }
});

// Configure PrimeVue
app.use(PrimeVue, {
    theme: {
        preset: MyThemePreset,
        options: {
            prefix: 'p',
            darkModeSelector: '.p-color-mode-dark',
            cssLayer: false
        }
    }
});
app.component('Button', Button);
app.component('Card', Card);
app.component('Column', Column);
app.component('DataTable', DataTable);
app.component('Dialog', Dialog);
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

// Handle color mode for this application
handleColorMode();