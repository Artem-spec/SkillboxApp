import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-main-tool-bar',
  templateUrl: './main-tool-bar.component.html',
  styleUrls: ['./main-tool-bar.component.scss']
})
export class MainToolBarComponent implements OnInit, AfterViewInit {

  // state nav block
  @ViewChild('closedIcon') closedIcon: any
  @ViewChild('openIcon') openIcon: any
  public iconTemplate: any
  public navMenuisVisible = false;
  public lincMenuVisible = false;
  // url
  public apiUsrl = document.baseURI + environment.openApiUrl;

  constructor(
    private dcs: ChangeDetectorRef,
    public authService: AuthService) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.iconTemplate = !this.navMenuisVisible ? this.closedIcon : this.openIcon;
    this.dcs.detectChanges();
  }

  public loadNavICon() {
    return !this.navMenuisVisible ? this.closedIcon : this.openIcon;
  }

  public loadLincICon() {
    return !this.lincMenuVisible ? this.closedIcon : this.openIcon;
  }

}
