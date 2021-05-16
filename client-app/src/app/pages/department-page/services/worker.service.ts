import { HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CompackToastService, TypeToast } from 'ngx-compack';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ApiService } from 'src/app/services/api.service';
import { environment } from 'src/environments/environment';
import { TypeWorkerFile } from '../model/dto/type-worker-file';
import { WorkerDto } from '../model/dto/worker-dto';
import { WorkerDep } from '../model/entity/worker';
import { DepartmentEventService } from './department-event.service';

@Injectable()
export class WorkerService {

  // data
  private workers$: BehaviorSubject<WorkerDep[] | null> = new BehaviorSubject<WorkerDep[] | null>(null)
  private viewDepId: number | null = null;

  constructor(
    private eventService: DepartmentEventService,
    private cts: CompackToastService,
    private apiService: ApiService) { }

  public getWorkersDataSubs() {
    return this.workers$;
  }

  public RemoveWorker(workerId: number): Observable<boolean> {
    const url = 'worker';
    const param = new HttpParams()
      .append('workerId', workerId.toString());
    return this.apiService.delete<boolean>(url, param)
      .pipe(catchError((err) => {
        this.cts.emitNewNotif({ type: TypeToast.Error, title: 'Ошибка при добавлении' })
        throw err;
      }));
  }

  public UpdateWorker(worker: WorkerDto): Observable<boolean> {
    const url = 'worker';
    return this.apiService.put<boolean>(url, worker)
      .pipe(catchError((err) => {
        this.cts.emitNewNotif({ type: TypeToast.Error, title: 'Ошибка при добавлении' })
        throw err;
      }));
  }

  public AddWorker(departmentId: number, worker: WorkerDto): Observable<boolean> {
    const url = 'worker';
    const param = new HttpParams()
      .append('departmentId', departmentId.toString());
    return this.apiService.post<boolean>(url, worker, param)
      .pipe(catchError((err) => {
        this.cts.emitNewNotif({ type: TypeToast.Error, title: 'Ошибка при добавлении' })
        throw err;
      }));
  }

  public getViewDepartmentId(): number | null {
    return this.viewDepId;
  }

  public EmitLoadWorkerByDepartment(idDepartment: number) {
    this.viewDepId = idDepartment;
    const url = 'worker';
    const param = new HttpParams()
      .append('departmentId', idDepartment.toString());
    this.apiService.get<WorkerDto[]>(url, param)
      .subscribe(
        next => {
          this.workers$.next(next);
        },
        error => {
          this.cts.emitNewNotif({ type: TypeToast.Error, title: 'ошибка при загрузки сотрудников' })
        })
  }

  public importWorker(file: File, typeFile: TypeWorkerFile) {
    if (this.viewDepId) {
      const formData = new FormData();
      formData.append('file', file, file.name);

      const req = new HttpRequest('POST', environment.urlApi + 'worker/import-file', formData, {
        reportProgress: true,
        params: new HttpParams()
          .append('departmentId', this.viewDepId.toString())
          .append('typeFile', typeFile.toString())
      });

      this.apiService.doRequest(req)
        .subscribe(next => { },
          error => {
            this.cts.emitNewNotif({ type: TypeToast.Error, title: 'Импорт записей', message: 'Произошла ошибка' });
          },
          () => {
            this.cts.emitNewNotif({ type: TypeToast.Success, title: 'Импорт записей', message: 'Успешно' });
            this.ReloadWorkers();
          });
    }
  }

  public exportWorker(typeFile: TypeWorkerFile) {
    const httpBody = {
      typeFile: typeFile,
      departmentId: this.viewDepId
    }
    return this.apiService.postOptions<Blob>("worker/export-file", httpBody, {
      reportProgress: true,
      responseType: 'blob'
    })
  }

  public ReloadWorkers() {
    console.log('reload worker for', this.viewDepId);
    this.EmitClearWorkers(false)
    this.eventService.emiteLoadWorkerEvent();
    if (this.viewDepId)
      this.EmitLoadWorkerByDepartment(this.viewDepId);
  }

  public EmitClearWorkers(cleareSelected: boolean) {
    if (cleareSelected)
      this.viewDepId = null;
    this.workers$.next([]);
  }

}
