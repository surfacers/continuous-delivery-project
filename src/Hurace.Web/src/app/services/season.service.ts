import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { SeasonDto } from '../dtos/season.dto';

@Injectable({
    providedIn: 'root'
})
export class SeasonService {
    private baseUrl = `${environment.apiBaseUrl}/api/season`;

    constructor(
        private http: HttpClient
    ) { }

    public getAllSeasons() {
        return this.http.get<SeasonDto[]>(`${this.baseUrl}`);
    }
}
