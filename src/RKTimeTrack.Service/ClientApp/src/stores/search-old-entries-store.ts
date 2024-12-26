import {defineStore} from "pinia";
import {inject, ref, watch} from "vue";
import {SearchEntriesByText_Request, TimeTrackClient} from "@/services/time-track-client.generated";
import {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";
import _ from "lodash";

export const useSearchOldEntriesStore = defineStore('searchOldEntriesStore', () => {
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;

    const searchString = ref("");
    const searchResults = ref<UiTimeTrackingEntry[]>([]);
    
    let lastTestResultTimestamp = Date.now();
    const executeSearchThrottled = _.throttle(
        () => {
            const request = new SearchEntriesByText_Request({
                searchText: searchString.value,
                maxSearchResults: 10});
            timeTrackClient.searchEntries(request)
                .then(result =>{
                    const now = Date.now();
                    if(now < lastTestResultTimestamp){ return; }
                    
                    lastTestResultTimestamp = now;
                    searchResults.value = result.map(x => UiTimeTrackingEntry.fromBackendModel(x));
                });
        },
        500,
        {
            leading: false,
            trailing: true
        });
    
    watch(
        searchString,
        () => executeSearchThrottled());
    
    return { searchString, searchResults };
});