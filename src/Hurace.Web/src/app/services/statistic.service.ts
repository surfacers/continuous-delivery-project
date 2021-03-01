import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { RaceStatisticEntryDto } from 'src/app/dtos';
import { RunNumber } from '../models';

@Injectable({
    providedIn: 'root'
})
export class StatisticService {
    private baseUrl = `${environment.apiBaseUrl}/api/statistic`;

    constructor(
        private http: HttpClient
    ) { }

    public getRaceStatistic(id: number, runNumber: RunNumber) {
        return this.http.get<RaceStatisticEntryDto[]>(`${this.baseUrl}/${id}/run/${runNumber}`);
    }
}
