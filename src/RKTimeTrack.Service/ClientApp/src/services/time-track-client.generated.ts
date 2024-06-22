//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.8.0 (NJsonSchema v11.0.1.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export class TimeTrackClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl ?? "";
    }

    /**
     * @return OK
     */
    getCurrentWeek(): Promise<TimeTrackingWeek> {
        let url_ = this.baseUrl + "/api/ui/week";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetCurrentWeek(_response);
        });
    }

    protected processGetCurrentWeek(response: Response): Promise<TimeTrackingWeek> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = TimeTrackingWeek.fromJS(resultData200);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ProblemDetails.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            let result500: any = null;
            let resultData500 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result500 = ProblemDetails.fromJS(resultData500);
            return throwException("Internal Server Error", status, _responseText, _headers, result500);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<TimeTrackingWeek>(null as any);
    }

    /**
     * @return OK
     */
    getWeek(year: number, weekNumber: number): Promise<TimeTrackingWeek> {
        let url_ = this.baseUrl + "/api/ui/week/{year}/{weekNumber}";
        if (year === undefined || year === null)
            throw new Error("The parameter 'year' must be defined.");
        url_ = url_.replace("{year}", encodeURIComponent("" + year));
        if (weekNumber === undefined || weekNumber === null)
            throw new Error("The parameter 'weekNumber' must be defined.");
        url_ = url_.replace("{weekNumber}", encodeURIComponent("" + weekNumber));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetWeek(_response);
        });
    }

    protected processGetWeek(response: Response): Promise<TimeTrackingWeek> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = TimeTrackingWeek.fromJS(resultData200);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ProblemDetails.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            let result500: any = null;
            let resultData500 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result500 = ProblemDetails.fromJS(resultData500);
            return throwException("Internal Server Error", status, _responseText, _headers, result500);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<TimeTrackingWeek>(null as any);
    }

    /**
     * @param year (optional) 
     * @return OK
     */
    getYearMetadata(year: number | undefined): Promise<TimeTrackingYearMetadata> {
        let url_ = this.baseUrl + "/api/ui/year/{year}/metadata";
        if (year !== null && year !== undefined)
        url_ = url_.replace("{year}", encodeURIComponent("" + year));
        else
            url_ = url_.replace("/{year}", "");
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetYearMetadata(_response);
        });
    }

    protected processGetYearMetadata(response: Response): Promise<TimeTrackingYearMetadata> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = TimeTrackingYearMetadata.fromJS(resultData200);
            return result200;
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result400 = ProblemDetails.fromJS(resultData400);
            return throwException("Bad Request", status, _responseText, _headers, result400);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            let result500: any = null;
            let resultData500 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result500 = ProblemDetails.fromJS(resultData500);
            return throwException("Internal Server Error", status, _responseText, _headers, result500);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<TimeTrackingYearMetadata>(null as any);
    }
}

export class ProblemDetails implements IProblemDetails {
    type!: string | undefined;
    title!: string | undefined;
    status!: number | undefined;
    detail!: string | undefined;
    instance!: string | undefined;

    [key: string]: any;

    constructor(data?: IProblemDetails) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            for (var property in _data) {
                if (_data.hasOwnProperty(property))
                    this[property] = _data[property];
            }
            this.type = _data["type"];
            this.title = _data["title"];
            this.status = _data["status"];
            this.detail = _data["detail"];
            this.instance = _data["instance"];
        }
    }

    static fromJS(data: any): ProblemDetails {
        data = typeof data === 'object' ? data : {};
        let result = new ProblemDetails();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        for (var property in this) {
            if (this.hasOwnProperty(property))
                data[property] = this[property];
        }
        data["type"] = this.type;
        data["title"] = this.title;
        data["status"] = this.status;
        data["detail"] = this.detail;
        data["instance"] = this.instance;
        return data;
    }
}

export interface IProblemDetails {
    type: string | undefined;
    title: string | undefined;
    status: number | undefined;
    detail: string | undefined;
    instance: string | undefined;

    [key: string]: any;
}

export class TimeTrackingDay implements ITimeTrackingDay {
    /** Date in format 'yyyy-mm-dd' */
    date!: string;
    type!: TimeTrackingDayType;
    entries!: TimeTrackingRow[] | undefined;

    constructor(data?: ITimeTrackingDay) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.date = _data["date"];
            this.type = _data["type"];
            if (Array.isArray(_data["entries"])) {
                this.entries = [] as any;
                for (let item of _data["entries"])
                    this.entries!.push(TimeTrackingRow.fromJS(item));
            }
        }
    }

    static fromJS(data: any): TimeTrackingDay {
        data = typeof data === 'object' ? data : {};
        let result = new TimeTrackingDay();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["date"] = this.date;
        data["type"] = this.type;
        if (Array.isArray(this.entries)) {
            data["entries"] = [];
            for (let item of this.entries)
                data["entries"].push(item.toJSON());
        }
        return data;
    }
}

export interface ITimeTrackingDay {
    /** Date in format 'yyyy-mm-dd' */
    date: string;
    type: TimeTrackingDayType;
    entries: TimeTrackingRow[] | undefined;
}

export enum TimeTrackingDayType {
    WorkingDay = "WorkingDay",
    OwnEducation = "OwnEducation",
    PublicHoliday = "PublicHoliday",
    Ill = "Ill",
    Training = "Training",
    Holiday = "Holiday",
    Weekend = "Weekend",
}

export class TimeTrackingRow implements ITimeTrackingRow {
    topic!: TimeTrackingTopicReference;
    effortInHours!: number;
    effortBilled!: number;
    description!: string | undefined;

    constructor(data?: ITimeTrackingRow) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.topic = _data["topic"] ? TimeTrackingTopicReference.fromJS(_data["topic"]) : <any>undefined;
            this.effortInHours = _data["effortInHours"];
            this.effortBilled = _data["effortBilled"];
            this.description = _data["description"];
        }
    }

    static fromJS(data: any): TimeTrackingRow {
        data = typeof data === 'object' ? data : {};
        let result = new TimeTrackingRow();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["topic"] = this.topic ? this.topic.toJSON() : <any>undefined;
        data["effortInHours"] = this.effortInHours;
        data["effortBilled"] = this.effortBilled;
        data["description"] = this.description;
        return data;
    }
}

export interface ITimeTrackingRow {
    topic: TimeTrackingTopicReference;
    effortInHours: number;
    effortBilled: number;
    description: string | undefined;
}

export class TimeTrackingTopicReference implements ITimeTrackingTopicReference {
    category!: string | undefined;
    name!: string | undefined;

    constructor(data?: ITimeTrackingTopicReference) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.category = _data["category"];
            this.name = _data["name"];
        }
    }

    static fromJS(data: any): TimeTrackingTopicReference {
        data = typeof data === 'object' ? data : {};
        let result = new TimeTrackingTopicReference();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["category"] = this.category;
        data["name"] = this.name;
        return data;
    }
}

export interface ITimeTrackingTopicReference {
    category: string | undefined;
    name: string | undefined;
}

export class TimeTrackingWeek implements ITimeTrackingWeek {
    year!: number;
    weekNumber!: number;
    monday!: TimeTrackingDay;
    tuesday!: TimeTrackingDay;
    wednesday!: TimeTrackingDay;
    thursday!: TimeTrackingDay;
    friday!: TimeTrackingDay;
    saturday!: TimeTrackingDay;
    sunday!: TimeTrackingDay;

    constructor(data?: ITimeTrackingWeek) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.year = _data["year"];
            this.weekNumber = _data["weekNumber"];
            this.monday = _data["monday"] ? TimeTrackingDay.fromJS(_data["monday"]) : <any>undefined;
            this.tuesday = _data["tuesday"] ? TimeTrackingDay.fromJS(_data["tuesday"]) : <any>undefined;
            this.wednesday = _data["wednesday"] ? TimeTrackingDay.fromJS(_data["wednesday"]) : <any>undefined;
            this.thursday = _data["thursday"] ? TimeTrackingDay.fromJS(_data["thursday"]) : <any>undefined;
            this.friday = _data["friday"] ? TimeTrackingDay.fromJS(_data["friday"]) : <any>undefined;
            this.saturday = _data["saturday"] ? TimeTrackingDay.fromJS(_data["saturday"]) : <any>undefined;
            this.sunday = _data["sunday"] ? TimeTrackingDay.fromJS(_data["sunday"]) : <any>undefined;
        }
    }

    static fromJS(data: any): TimeTrackingWeek {
        data = typeof data === 'object' ? data : {};
        let result = new TimeTrackingWeek();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["year"] = this.year;
        data["weekNumber"] = this.weekNumber;
        data["monday"] = this.monday ? this.monday.toJSON() : <any>undefined;
        data["tuesday"] = this.tuesday ? this.tuesday.toJSON() : <any>undefined;
        data["wednesday"] = this.wednesday ? this.wednesday.toJSON() : <any>undefined;
        data["thursday"] = this.thursday ? this.thursday.toJSON() : <any>undefined;
        data["friday"] = this.friday ? this.friday.toJSON() : <any>undefined;
        data["saturday"] = this.saturday ? this.saturday.toJSON() : <any>undefined;
        data["sunday"] = this.sunday ? this.sunday.toJSON() : <any>undefined;
        return data;
    }
}

export interface ITimeTrackingWeek {
    year: number;
    weekNumber: number;
    monday: TimeTrackingDay;
    tuesday: TimeTrackingDay;
    wednesday: TimeTrackingDay;
    thursday: TimeTrackingDay;
    friday: TimeTrackingDay;
    saturday: TimeTrackingDay;
    sunday: TimeTrackingDay;
}

export class TimeTrackingYearMetadata implements ITimeTrackingYearMetadata {
    maxWeekNumber!: number;

    constructor(data?: ITimeTrackingYearMetadata) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.maxWeekNumber = _data["maxWeekNumber"];
        }
    }

    static fromJS(data: any): TimeTrackingYearMetadata {
        data = typeof data === 'object' ? data : {};
        let result = new TimeTrackingYearMetadata();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["maxWeekNumber"] = this.maxWeekNumber;
        return data;
    }
}

export interface ITimeTrackingYearMetadata {
    maxWeekNumber: number;
}

export class ApiException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}