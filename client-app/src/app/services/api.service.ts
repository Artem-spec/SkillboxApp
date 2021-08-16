import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(
    private httpClient: HttpClient,
    @Inject('BASE_APP_URL') public urlApi: string) { }

  public get<T>(path: string, params?: HttpParams, headers?: HttpHeaders): Observable<T> {
    const httpOptions = this.getHttpOptions();
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();
    if (headers)
      httpOptions.headers = headers;

    return this.httpClient.get<T>(this.urlApi + path, httpOptions);
  }

  public post<T>(path: string, body?: object, params?: HttpParams, headers?: HttpHeaders): Observable<T> {
    const httpOptions = this.getHttpOptions();
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();
    if (headers) {
      httpOptions.headers = headers;
    }
    return this.httpClient.post<T>(this.urlApi + path, JSON.stringify(body), httpOptions);
  }

  public postOptions<T>(path: string, body: object, newHttpOptions: object): Observable<T> {
    return this.httpClient.post<T>(this.urlApi + path, body, newHttpOptions);
  }

  public doRequest(request: HttpRequest<any>) {
    return this.httpClient.request(request);
  }

  public delete<T>(path: string, params?: HttpParams) {
    const httpOptions = this.getHttpOptions();
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();

    return this.httpClient.delete<T>(this.urlApi + path, httpOptions);
  }

  public put<T>(path: string, body: object, params?: HttpParams) {
    const httpOptions = this.getHttpOptions();
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();

    return this.httpClient.put<T>(this.urlApi + path, body, httpOptions);
  }

  private getHttpOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json; charset=utf-8'
      }),
      params: new HttpParams()
    };
  }

}
