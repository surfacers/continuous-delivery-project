import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { SkierDto } from 'src/app/dtos';

@Injectable({
    providedIn: 'root'
})
export class SkierService {
    private baseUrl = `${environment.apiBaseUrl}/api/skier`;

    constructor(
        private http: HttpClient
    ) { }

    public getAll() {
        return this.http.get<SkierDto[]>(this.baseUrl);
    }

    public getById(id: number) {
        return this.http.get<SkierDto>(`${this.baseUrl}/${id}`);
    }

    public save(skier: SkierDto) {
        return this.http.post<number>(this.baseUrl, skier);
    }

    public remove(id: number) {
        return this.http.delete(`${this.baseUrl}/${id}`);
    }
}
