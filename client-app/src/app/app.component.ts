import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CompackBannerService, TypeMessage, TypePositionMessage } from 'ngx-compack';
import { LoginDialogComponent } from './components/login-dialog/login-dialog.component';
import { AddBarServiceService } from './services/addBarService.service';
import { AuthService } from './services/auth.service';
import { ResizeService } from './services/resize.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  // dynamic bar from page
  public dynamicBar: any = null;
  // size info
  public width = window.innerWidth;
  // resize bar block
  public isResizeBarBlock = this.width < 991;
  public barBlockIsVisible = false;

  constructor(
    private dcs: ChangeDetectorRef,
    private abs: AddBarServiceService,
    private bannerService: CompackBannerService,
    public authService: AuthService,
    public dialog: MatDialog,
    resizeService: ResizeService) {
    resizeService.getResizeEvent().subscribe((newWidth: number) => {
      this.width = newWidth;
      this.isResizeBarBlock = this.width < 991;
    })
  }

  ngOnInit() {

    this.bannerService.viewBanner(
      TypeMessage.Info, TypePositionMessage.TopRight,
      'Для авторизации: \n логин: admin \n пароль: complex', undefined, 15);

    this.abs.getEmiterChangeBar()
      .subscribe((next: any) => {
        this.dynamicBar = next;
        this.dcs.detectChanges();
      });

  }

  public openLoginDialog() {
    this.dialog.open(LoginDialogComponent, {
      height: 'auto',
      width: 'auto'
    });
  }

}
