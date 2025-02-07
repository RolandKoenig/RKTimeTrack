import type {UiTimeTrackingDateOnly} from "@/stores/models/ui-time-tracking-date-only";
import type {UiTimeTrackingDayType} from "@/stores/models/ui-time-tracking-day-type";
import {UiTimeTrackingEntry} from "@/stores/models/ui-time-tracking-entry";
import {TimeTrackingDay} from "@/services/time-track-client.generated";

export class UiTimeTrackingDay{
    constructor(
        public date: UiTimeTrackingDateOnly,
        public type: UiTimeTrackingDayType,
        public entries: UiTimeTrackingEntry[]
    ) {
    }
    
    static fromBackendModel(backendModel: TimeTrackingDay): UiTimeTrackingDay{
        const mappedEntries = backendModel.entries 
            ? backendModel.entries.map(val => UiTimeTrackingEntry.fromBackendModel(val))
            : [];
        
        return new UiTimeTrackingDay(
            backendModel.date,
            backendModel.type,
            mappedEntries
        )
    }
    
    toBackendModel(): TimeTrackingDay{
        return new TimeTrackingDay({
            date: this.date,
            type: this.type,
            entries: this.entries.map(entry => entry.toBackendModel())
        })
    }
}