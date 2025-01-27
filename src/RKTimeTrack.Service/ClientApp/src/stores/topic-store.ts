import {defineStore} from "pinia";
import {inject, shallowRef, type Ref} from "vue";
import {TimeTrackClient, TimeTrackingTopic} from "@/services/time-track-client.generated";

export const useTopicStore = defineStore('topicStore', () => {
    const timeTrackClient = inject<TimeTrackClient>("TimeTrackClient")!;
    const topics: Ref<TimeTrackingTopic[]> = shallowRef([]);

    async function fetchInitialData() {
        topics.value = await timeTrackClient.getAllTopics();
    }

    function getTopicsVisibleAt(date: Date) {
        return topics.value.filter(topic => {
            const startDate = topic.startDate ? new Date(topic.startDate) : null;
            const endDate = topic.endDate ? new Date(topic.endDate) : null;
            
            if(endDate){
                endDate.setDate(endDate.getDate() + 1);
            }
            
            return (!startDate || startDate <= date) && (!endDate || endDate >= date);
        });
    }
    
    // Trigger initialization
    fetchInitialData();
    
    return{
        topics,
        getTopicsVisibleAt
    }
});