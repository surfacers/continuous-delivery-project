import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { RaceDto } from 'src/app/dtos';
import { RaceState } from '../enums';

@Injectable({
    providedIn: 'root'
})
export class RaceService {
    private baseUrl = `${environment.apiBaseUrl}/api/race`;

    constructor(
        private http: HttpClient
    ) { }

    public getAll() {
        return this.http.get<RaceDto[]>(this.baseUrl);
    }

    public getByState(raceState: RaceState) {
        return this.http.get<RaceDto[]>(`${this.baseUrl}/state/${raceState}`);
    }

    public getById(id: number) {
        return this.http.get<RaceDto>(`${this.baseUrl}/${id}`);
    }
}
