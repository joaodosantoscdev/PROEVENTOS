import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '@environments/environment';
import { PaginatedResult } from '@app/models/pagination';
import { Speaker } from '@app/models/Speaker';


import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SpeakerService {
  baseUrl = `${environment.apiURL}api/Speaker`;
  constructor(private http: HttpClient) { }

  public getSpeakers(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Speaker[]>> {
    const paginatedResult: PaginatedResult<Speaker[]> = new PaginatedResult<Speaker[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (term != null && term !== '') {
      params = params.append('term', term);
    }

    return this.http
      .get<Speaker[]>(this.baseUrl + '/all', {observe: 'response', params })
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

  public getSpeakerByToken(): Observable<Speaker> {
    return this.http
    .get<Speaker>(`${this.baseUrl}`)
    .pipe(take(1));
  }

  public post(): Observable<Speaker> {
    return this.http
    .post<Speaker>(this.baseUrl, {} as Speaker)
    .pipe(take(1));
  }

  public put(speaker: Speaker): Observable<Speaker> {
    return this.http
    .put<Speaker>(`${this.baseUrl}`, speaker);
  }
}
