import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  baseUrl = 'https://localhost:44388/api/Event'
  constructor(private http: HttpClient) { }

  getEvent() {
    return this.http.get(this.baseUrl);
  }
}
