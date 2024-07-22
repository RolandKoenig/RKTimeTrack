import {TimeTrackingWeek} from "@/services/time-track-client.generated";
import {UiTimeTrackingDay} from "@/stores/models/ui-time-tracking-day";

export class UiTimeTrackingWeek{
    constructor(
        public year: number,
        public weekNumber: number,
        public monday: UiTimeTrackingDay,
        public tuesday: UiTimeTrackingDay,
        public wednesday: UiTimeTrackingDay,
        public thursday: UiTimeTrackingDay,
        public friday: UiTimeTrackingDay,
        public saturday: UiTimeTrackingDay,
        public sunday: UiTimeTrackingDay
    ) {
    }
    
    static fromBackendModel(backendModel: TimeTrackingWeek): UiTimeTrackingWeek{
        return new UiTimeTrackingWeek(
            backendModel.year,
            backendModel.weekNumber,
            UiTimeTrackingDay.fromBackendModel(backendModel.monday),
            UiTimeTrackingDay.fromBackendModel(backendModel.tuesday),
            UiTimeTrackingDay.fromBackendModel(backendModel.wednesday),
            UiTimeTrackingDay.fromBackendModel(backendModel.thursday),
            UiTimeTrackingDay.fromBackendModel(backendModel.friday),
            UiTimeTrackingDay.fromBackendModel(backendModel.saturday),
            UiTimeTrackingDay.fromBackendModel(backendModel.sunday)
        )
    }
    
    toBackendModel(): TimeTrackingWeek {
        return new TimeTrackingWeek({
            year: this.year,
            weekNumber: this.weekNumber,
            monday: this.monday.toBackendModel(),
            tuesday: this.tuesday.toBackendModel(),
            wednesday: this.wednesday.toBackendModel(),
            thursday: this.thursday.toBackendModel(),
            friday: this.friday.toBackendModel(),
            saturday: this.saturday.toBackendModel(),
            sunday: this.sunday.toBackendModel()
        });
    }
}