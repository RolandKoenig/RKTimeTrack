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
    
    tryGetDayByDate(date:string){
        if(this.monday.date === date){ return this.monday; }
        else if(this.tuesday.date === date){ return this.tuesday; }
        else if(this.wednesday.date === date){ return this.wednesday; }
        else if(this.thursday.date === date){ return this.thursday; }
        else if(this.friday.date === date){ return this.friday; }
        else if(this.saturday.date === date){ return this.saturday; }
        else if(this.sunday.date === date){ return this.sunday; }
        else{ return null; }
    }
    
    getAllDays(){
        return [
            this.monday, this.tuesday, this.wednesday,
            this.thursday, this.friday, this.saturday,
            this.sunday];
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
}