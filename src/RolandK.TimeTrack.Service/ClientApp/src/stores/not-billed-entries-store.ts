import {defineStore} from "pinia";
import {inject, ref} from "vue";
import {SearchEntries_Request, TimeTrackClient} from "@/services/time-track-client.generated";
import {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";

export const useNotBilledEntriesStore = defineStore('notBilledEntriesStore', () => {
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;

    const notBilledEntries = ref<UiTimeTrackingEntry[]>([]);
    const isLoading = ref(false);
    
    const fetchBillableEntries = async () => {
        isLoading.value = true;
        try {
            const request = new SearchEntries_Request({
                searchText: "",
                billed: false,
                canBeInvoiced: true,
                maxSearchResults: 500
            });
            const result = await timeTrackClient.searchEntries(request);
            notBilledEntries.value = result.map(x => UiTimeTrackingEntry.fromBackendModel(x));
        } finally {
            isLoading.value = false;
        }
    };
    
    return { notBilledEntries, isLoading, fetchBillableEntries };
});
