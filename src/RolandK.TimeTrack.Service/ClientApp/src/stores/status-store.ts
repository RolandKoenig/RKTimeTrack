import {ref, type Ref, inject} from 'vue'
import {defineStore} from "pinia";
import {TimeTrackClient, TimeTrackApplicationState} from "@/services/time-track-client.generated.ts";

export const useStatusStore = defineStore('statusStore', () => {
    const applicationStatePollIntervalMs = 1000;
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;

    const applicationState: Ref<TimeTrackApplicationState | null> = ref(null);
    const applicationStateLastUpdatedAt: Ref<Date | null> = ref(null);
    const applicationStateError: Ref<boolean> = ref(false);
    
    let statePollingStopRequested = false;
    let statePollingPromise: Promise<void> | null = null;
    let isFetchingApplicationState = false;

    async function startApplicationStatePolling(): Promise<void> {
        if (statePollingPromise) { return; } // already running

        statePollingStopRequested = false;

        // Start with an immediate fetch so app gets data ASAP
        await fetchApplicationStateOnce();

        statePollingPromise = (async () => {
            while (!statePollingStopRequested) {
                await delay(applicationStatePollIntervalMs);
                if (statePollingStopRequested) { break; }
                await fetchApplicationStateOnce();
            }
        })();
    }
    
    async function fetchApplicationStateOnce(): Promise<void> {
        if (isFetchingApplicationState) { return; }
        
        isFetchingApplicationState = true;
        try {
            applicationState.value = await timeTrackClient.getState();
            applicationStateLastUpdatedAt.value = new Date();
            applicationStateError.value = false;
        } catch (e) {
            console.error("Failed to fetch application state", e);
            applicationStateError.value = true;
        } finally {
            isFetchingApplicationState = false;
        }
    }

    function delay(ms: number): Promise<void> {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    // Start polling loop
    startApplicationStatePolling();
    
    return {
        applicationState,
        applicationStateLastUpdatedAt,
        applicationStateError,
    }
});