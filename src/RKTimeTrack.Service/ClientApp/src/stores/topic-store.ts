import {defineStore} from "pinia";
import {inject, shallowRef, type Ref} from "vue";
import {TimeTrackClient, TimeTrackingTopic} from "@/services/time-track-client.generated";

export const useTopicStore = defineStore('topicStore', () => {
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
    const topics: Ref<TimeTrackingTopic[]> = shallowRef([]);

    async function fetchInitialData() {
        topics.value = await timeTrackClient.getAllTopics();
    }
    
    // Trigger initialization
    fetchInitialData();
    
    return{
        topics
    }
});