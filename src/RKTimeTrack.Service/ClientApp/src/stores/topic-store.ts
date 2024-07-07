import {defineStore} from "pinia";
import {inject, ref, type Ref} from "vue";
import {TimeTrackClient, TimeTrackingTopic} from "@/services/time-track-client.generated";

export const useTopicStore = defineStore('topicStore', () => {
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
    const topics: Ref<TimeTrackingTopic[]> = ref([]);

    async function fetchInitialData() {
        topics.value = await timeTrackClient.getAllTopics();
    }
    
    // Trigger initialization
    fetchInitialData();
    
    return{
        topics
    }
});