import {TimeTrackingEntry} from "@/services/time-track-client.generated";
import {TimeTrackingTopicReference} from "@/services/time-track-client.generated";
import {v4 as createUuidV4} from 'uuid';
import type {UiTimeTrackingEntryType} from "@/stores/models/ui-time-tracking-entry-type";

export class UiTimeTrackingEntry{
    constructor(
        public id: string | undefined,
        public topicCategory: string,
        public topicName: string,
        public effortInHours: number,
        public effortBilled: number,
        public type: UiTimeTrackingEntryType,
        public description: string
    ){
        if(!id){ this.id = createUuidV4(); }
    }

    static fromBackendModel(backendModel: TimeTrackingEntry): UiTimeTrackingEntry{
        return new UiTimeTrackingEntry(
            createUuidV4(),
            backendModel.topic?.category ?? "",
            backendModel.topic?.name ?? "",
            backendModel.effortInHours,
            backendModel.effortBilled,
            backendModel.type,
            backendModel.description ?? ""
        )
    }
    
    toBackendModel(): TimeTrackingEntry{
        return new TimeTrackingEntry({
            topic: new TimeTrackingTopicReference({
                category: this.topicCategory,
                name: this.topicName
            }),
            effortInHours: this.effortInHours,
            effortBilled: this.effortBilled,
            type: this.type,
            description: this.description
        })
    }
}