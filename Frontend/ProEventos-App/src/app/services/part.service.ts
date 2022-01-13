import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Part } from '@app/models/Part';

import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable()
export class PartService {
  baseUrl = 'https://localhost:44388/api/Part';
  constructor(private http: HttpClient) { }

  public getPartsByEventId(eventId: number): Observable<Part[]> {
    return this.http
    .get<Part[]>(`${this.baseUrl}/${eventId}`)
    .pipe(take(1));
  }

  public SavePart(eventId: number, parts: Part[]): Observable<Part[]> {
    return this.http
    .put<Part[]>(`${this.baseUrl}/${eventId}`, parts);
  }

  public deletePart(eventId: number, id: number): Observable<any> {
    return this.http
    .delete(`${this.baseUrl}/${eventId}/${id}`)
    .pipe(take(1));
  }

}
