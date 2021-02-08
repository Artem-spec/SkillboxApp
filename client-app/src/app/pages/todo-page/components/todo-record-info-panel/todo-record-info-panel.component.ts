import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CompackToastService, TypeToast } from 'ngx-compack';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LoginDialogComponent } from 'src/app/components/login-dialog/login-dialog.component';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';
import { TodoRecordService } from '../../services/todo-record.service';

@Component({
  selector: 'app-todo-record-info-panel',
  templateUrl: './todo-record-info-panel.component.html',
  styleUrls: ['./todo-record-info-panel.component.scss']
})
export class TodoRecordInfoPanelComponent implements OnInit {

  // data
  public countPost$: Observable<number> | null = null;
  public apiUsrl = environment.openApiUrl;

  constructor(
    private recordService: TodoRecordService,
    public authService: AuthService,
    private apiService: ApiService,
    private cts: CompackToastService,
    public dialog: MatDialog,
  ) { }

  ngOnInit() {
    this.recordService.getRecordsSubs()
      .subscribe(
        () => this.getCountPost()
      )
  }

  public openLoginDialog() {
    this.dialog.open(LoginDialogComponent, {
      height: 'auto',
      width: 'auto'
    });

  }

  private getCountPost() {
    const httpParams = new HttpParams()
      .append('IdUser', this.authService.getUserId().toString())
    this.countPost$ = this.apiService.get<number>("todo/count", httpParams)
      .pipe(catchError((err) => {
        this.cts.emitNewNotif({ type: TypeToast.Error, title: 'Количество записей', message: 'Произошла ошибка при получении количества постов' })
        throw err;
      }))
  }

}
