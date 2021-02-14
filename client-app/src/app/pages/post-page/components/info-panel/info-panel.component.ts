import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CompackToastService, TypeToast } from 'ngx-compack';
import { Subscription } from 'rxjs';
import { LoginDialogComponent } from 'src/app/components/login-dialog/login-dialog.component';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';
import { PostService } from '../../services/post.service';

@Component({
  selector: 'app-info-panel',
  templateUrl: './info-panel.component.html',
  styleUrls: ['./info-panel.component.scss']
})
export class InfoPanelComponent implements OnInit, OnDestroy {
  // data
  public countPost = 0;
  public apiUsrl = environment.openApiUrl;
  // http
  private subs: Subscription | null | undefined

  constructor(
    public authService: AuthService,
    private apiService: ApiService,
    private cts: CompackToastService,
    public dialog: MatDialog,
    public postService: PostService
  ) { }

  ngOnInit() {
    this.postService.getPostSubs()
      .subscribe(
        () => this.getCountPost()
      );
  }

  ngOnDestroy() {
    if (this.subs)
      this.subs.unsubscribe();
  }

  public openLoginDialog() {
    this.dialog.open(LoginDialogComponent, {
      height: 'fit-content',
      width: 'auto'
    });

  }

  private getCountPost() {
    this.subs = this.apiService.get<number>("post/count")
      .subscribe(next => this.countPost = next,
        error => this.cts.emitNewNotif(
          { type: TypeToast.Error, title: 'Количество постов', message: 'Произошла ошибка при получении количества постов' }))
  }

}
