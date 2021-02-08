import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginDto } from '../components/model/login-dto';
import { ApiService } from './api.service';
import { map } from 'rxjs/operators';
import { LoginHttpResponse } from '../model/login-http-response';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public user: BehaviorSubject<LoginHttpResponse>;

  constructor(
    private apiService: ApiService
  ) {
    const token = sessionStorage.getItem('token');
    const id = sessionStorage.getItem('id');

    if (token && id) {
      this.user = new BehaviorSubject<LoginHttpResponse>({ token: token, id: +id });
    } else {
      this.user = new BehaviorSubject<LoginHttpResponse>({ token: null, id: null });
    }

  }

  public logIn(login: string, pass: string): Observable<LoginHttpResponse> {
    const httpBody = new LoginDto(login, pass);
    return this.apiService.post<LoginHttpResponse>('auth', httpBody)
      .pipe(
        map((data: LoginHttpResponse) => {
          if (data) {
            if (data.token && data.id) {
              this.user.next({ token: data.token, id: data.id });
              sessionStorage.setItem('token', data.token);
              sessionStorage.setItem('id', data.id.toString());
            }
          }
          return data;
        })
      );
  }

  public getAuthorizationToken(): string | null {
    return this.user.getValue().token;
  }

  public checkLogIn(): boolean {
    return !!this.user.getValue().token;
  }

  public getUserId(): number {
    const id = sessionStorage.getItem('id');
    if (id)
      return +id
    return -1;
  }

}
