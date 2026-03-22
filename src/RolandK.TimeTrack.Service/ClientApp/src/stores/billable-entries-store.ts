import {defineStore} from "pinia";
import {inject, ref} from "vue";
import {SearchEntriesByText_Request, TimeTrackClient} from "@/services/time-track-client.generated";
import {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";

export const useBillableEntriesStore = defineStore('billableEntriesStore', () => {
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;

    const billableEntries = ref<UiTimeTrackingEntry[]>([]);
    const isLoading = ref(false);
    
    const fetchBillableEntries = async () => {
        isLoading.value = true;
        try {
            const request = new SearchEntriesByText_Request({
                searchText: "",
                billed: false,
                canBeInvoiced: true,
                maxSearchResults: 500
            });
            const result = await timeTrackClient.searchEntries(request);
            billableEntries.value = result.map(x => UiTimeTrackingEntry.fromBackendModel(x));
        } finally {
            isLoading.value = false;
        }
    };
    
    return { billableEntries, isLoading, fetchBillableEntries };
});
