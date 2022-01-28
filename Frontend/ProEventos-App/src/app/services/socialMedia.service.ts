import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SocialMedia } from '@app/models/SocialMedia';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SocialMediaService {

  baseURL = environment.apiURL + 'api/SocialMedia';

constructor(private http: HttpClient) { }

  public getSocialMedia(origin: string, id: number): Observable<SocialMedia[]> {
    let url =
      id === 0
      ? `${this.baseURL}/${origin}`
      : `${this.baseURL}/${origin}/${id}`;

    return this.http.get<SocialMedia[]>(url)
        .pipe(take(1));
  }

  public saveSocialMedia(origin: string, id: number, socialMedias: SocialMedia[]): Observable<SocialMedia[]> {
    let url =
      id === 0
      ? `${this.baseURL}/${origin}`
      : `${this.baseURL}/${origin}/${id}`;

    return this.http.put<SocialMedia[]>(url, socialMedias)
        .pipe(take(1));
  }


  public deleteSocialMedia(origin: string, id: number, socialMediaId: number): Observable<any> {
    let url =
      id === 0
      ? `${this.baseURL}/${origin}/${socialMediaId}`
      : `${this.baseURL}/${origin}/${id}/${socialMediaId}`;

    return this.http.delete<SocialMedia[]>(url)
        .pipe(take(1));
  }
}
