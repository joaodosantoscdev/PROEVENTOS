import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, pipe } from 'rxjs';
import { Event } from '../models/Event';
import { take } from 'rxjs/operators';

@Injectable()

export class EventService {
  baseUrl = 'https://localhost:44388/api/Event';
  constructor(private http: HttpClient) { }

  public getEvents(): Observable<Event[]> {
    return this.http
    .get<Event[]>(this.baseUrl)
    .pipe(take(1));
  }

  public getEventsByTheme(theme: string): Observable<Event[]> {
    return this.http
    .get<Event[]>(`${this.baseUrl}/theme/${theme}`)
    .pipe(take(1));
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
