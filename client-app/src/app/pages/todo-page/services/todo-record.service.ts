import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import * as moment from 'moment';
import { CompackToastService, TypeToast } from 'ngx-compack';
import { Observable, ReplaySubject, Subscription } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';
import { TodoRecordInfo } from '../model/todo-record-info';
import { DateFIlterDto, GetToDoQuery, TodoRecordInfoDto } from '../model/todo-record-info-dto';
import { TypeViewPeriodRecord } from '../model/type-view-period-record';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class TodoRecordService {
  // data
  private posts$: ReplaySubject<TodoRecordInfo[]> = new ReplaySubject<TodoRecordInfo[]>(1)
  // emit
  public cancelAddEvent$: EventEmitter<boolean> = new EventEmitter<boolean>();
  // http
  private subs: Subscription | null | undefined
  // configs
  private typeView: TypeViewPeriodRecord = TypeViewPeriodRecord.Today;
  private dates: string[] = [];

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService,
    private cts: CompackToastService,
    private apiService: ApiService) {
    moment.locale('ru');
  }

  ngOnDestroy() {
    if (this.subs)
      this.subs.unsubscribe();
  }

  public emiteLoadRecords(typeViewPeriodRecord: TypeViewPeriodRecord, dates: string[]) {
    this.getToDoRecords(typeViewPeriodRecord, dates)
  }

  public emiteCancelAddPosts() {
    this.cancelAddEvent$.emit(true);
  }

  public exportRecords() {
    const httpBody: GetToDoQuery = {
      typeView: this.typeView,
      userId: this.authService.getUserId(),
      dateFilter: new DateFIlterDto(this.dates)
    }

    this.subs = this.apiService.postOptions<Blob>("todo/export-file", httpBody, {
      reportProgress: true,
      responseType: 'blob'
    })
      .subscribe(
        next => {
          saveAs(next, 'exports');
        }, error => {
          this.cts.emitNewNotif({ type: TypeToast.Error, title: 'Экспорт данных', message: 'Произошла ошибка' });
        }
      )
  }

  public importRecords(file: File) {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const req = new HttpRequest('POST', environment.urlApi + 'todo/import-file', formData, {
      reportProgress: true,
      params: new HttpParams()
        .append('IdUser', this.authService.getUserId().toString())
    });

    this.apiService.doRequest(req)
      .subscribe(next => { },
        error => {
          this.cts.emitNewNotif({ type: TypeToast.Error, title: 'Импорт записей', message: 'Произошла ошибка' });
        },
        () => {
          this.cts.emitNewNotif({ type: TypeToast.Success, title: 'Импорт записей', message: 'Успешно' });
          this.getToDoRecords(this.typeView, this.dates);
        });
  }

  public updateRecord(record: TodoRecordInfo) {
    const httpParams = new HttpParams()
      .append('IdUser', this.authService.getUserId().toString())
    this.subs = this.apiService.put("todo", record, httpParams)
      .subscribe(
        next => {
          this.cts.emitNewNotif(
            { type: TypeToast.Success, title: 'Изменение заметки', message: 'Успешно' })
          this.getToDoRecords(this.typeView, this.dates);
        },
        error => this.cts.emitNewNotif(
          { type: TypeToast.Error, title: 'Изменение заметки', message: 'Произошла ошибка при получении списка постов' }))
  }

  public addNewRecord(record: TodoRecordInfo) {
    const httpParams = new HttpParams()
      .append('IdUser', this.authService.getUserId().toString())
    this.subs = this.apiService.post("todo", record, httpParams)
      .subscribe(
        next => {
          this.cts.emitNewNotif(
            { type: TypeToast.Success, title: 'Добавление заметки', message: 'Успешно' })
          this.getToDoRecords(this.typeView, this.dates);
        },
        error => this.cts.emitNewNotif(
          { type: TypeToast.Error, title: 'Добавление заметки', message: 'Произошла ошибка при получении списка постов' }))
  }

  public removeRecord(idRecord: number) {
    const httpParams = new HttpParams()
      .append('IdRecord', idRecord.toString())
      .append('IdUser', this.authService.getUserId().toString())
    this.subs = this.apiService.delete("todo", httpParams)
      .subscribe(
        next => {
          this.cts.emitNewNotif(
            { type: TypeToast.Success, title: 'Удаление заметки', message: 'Успешно' })
          this.getToDoRecords(this.typeView, this.dates);
        },
        error => this.cts.emitNewNotif(
          { type: TypeToast.Error, title: 'Удаление заметки', message: 'Произошла ошибка при получении списка постов' }))
  }

  public getRecordsSubs(): Observable<TodoRecordInfo[]> {
    return this.posts$;
  }

  private getToDoRecords(typeView: TypeViewPeriodRecord, dates: string[]) {
    const httpBody: GetToDoQuery = {
      typeView,
      userId: this.authService.getUserId(),
      dateFilter: new DateFIlterDto(dates)
    }
    this.typeView = typeView;
    this.dates = dates;
    this.subs = this.apiService.post<TodoRecordInfoDto[]>("todo/list", httpBody)
      .subscribe(records => {
        if (records.length !== 0) {
          const recordsInfo: TodoRecordInfo[] = []
          for (const item of records) {
            recordsInfo.push({
              isNew: false,
              text: item.text,
              id: item.id,
              date: moment(item.dateCreate, 'YYYY-MM-DDTHH:mm:ss').format('D MMMM HH:mm')
            })
          }
          this.posts$.next(recordsInfo);
        } else {
          this.posts$.next([]);
        }
      },
        error => this.cts.emitNewNotif(
          { type: TypeToast.Error, title: 'Список заметок', message: 'Произошла ошибка при получении списка постов' }))
  }

}
