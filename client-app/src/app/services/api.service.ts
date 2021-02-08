import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json; charset=utf-8'
  }),
  params: new HttpParams()
};

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private readonly url = environment.urlApi;

  constructor(private httpClient: HttpClient) {
  }

  public get<T>(path: string, params?: HttpParams, headers?: HttpHeaders): Observable<T> {
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();
    if (headers)
      httpOptions.headers = headers;

    return this.httpClient.get<T>(this.url + path, httpOptions);
  }

  public post<T>(path: string, body?: object, params?: HttpParams, headers?: HttpHeaders): Observable<T> {
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();
    if (headers) {
      httpOptions.headers = headers;
    }
    return this.httpClient.post<T>(this.url + path, JSON.stringify(body), httpOptions);
  }

  public postOptions<T>(path: string, body: object, newHttpOptions: object): Observable<T> {
    return this.httpClient.post<T>(this.url + path, body, newHttpOptions);
  }

  public doRequest(request: HttpRequest<any>) {
    return this.httpClient.request(request);
  }

  public delete(path: string, params?: HttpParams) {
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();

    return this.httpClient.delete(this.url + path, httpOptions);
  }

  public put(path: string, body: object, params?: HttpParams) {
    if (params)
      httpOptions.params = params;
    else
      httpOptions.params = new HttpParams();

    return this.httpClient.put(this.url + path, body, httpOptions);
  }

}
