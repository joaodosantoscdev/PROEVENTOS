import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, pipe } from 'rxjs';
import { Event } from '../models/Event';
import { map, take } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { PaginatedResult } from '@app/models/pagination';

@Injectable()

export class  EventService {
  baseUrl = `${environment.apiURL}api/Event`;
  constructor(private http: HttpClient) { }

  public getEvents(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Event[]>> {
    const paginatedResult: PaginatedResult<Event[]> = new PaginatedResult<Event[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (term != null && term !== '') {
      params = params.append('term', term);
    }

    return this.http
      .get<Event[]>(this.baseUrl, {observe: 'response', params })
      .pipe(
        take(1),
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.has('Pagination')) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        }));
  }

  public getEventsByTheme(theme: string): void {

  }

  public getEventById(id: number): Observable<Event> {
    return this.http
    .get<Event>(`${this.baseUrl}/${id}`)
    .pipe(take(1));
  }

  public post(event: Event): Observable<Event> {
    return this.http
    .post<Event>(this.baseUrl, event)
    .pipe(take(1));
  }

  public put(event: Event): Observable<Event> {
    return this.http
    .put<Event>(`${this.baseUrl}/${event.id}`, event);
  }

  public deleteEvent(id: number): Observable<any> {
    return this.http
    .delete(`${this.baseUrl}/${id}`)
    .pipe(take(1));
  }

  postUpload(eventId: number, file: File): Observable<Event> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    return this.http
    .post<Event>(`${this.baseUrl}/upload-image/${eventId}`, formData)
    .pipe(take(1));
  }
}
