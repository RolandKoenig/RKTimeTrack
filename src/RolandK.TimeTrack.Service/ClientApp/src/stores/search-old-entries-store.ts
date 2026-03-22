import {defineStore} from "pinia";
import {inject, ref} from "vue";
import {SearchEntries_Request, TimeTrackClient} from "@/services/time-track-client.generated";
import {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";
import {watchThrottled} from '@vueuse/core'

export const useSearchOldEntriesStore = defineStore('searchOldEntriesStore', () => {
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;

    const searchString = ref("");
    const searchResults = ref<UiTimeTrackingEntry[]>([]);
    
    let lastTestResultTimestamp = Date.now();
    watchThrottled(
        searchString,
        () => {
            const request = new SearchEntries_Request({
                searchText: searchString.value,
                billed: undefined,
                canBeInvoiced: undefined,
                maxSearchResults: 10});
            timeTrackClient.searchEntries(request)
                .then(result =>{
                    const now = Date.now();
                    if(now < lastTestResultTimestamp){ return; }

                    lastTestResultTimestamp = now;
                    searchResults.value = result.map(x => UiTimeTrackingEntry.fromBackendModel(x));
                })
        }, {
            throttle: 500, 
            leading: false,
            trailing: true
        }
    )
    
    return { searchString, searchResults };
});