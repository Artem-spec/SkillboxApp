import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { CompackBannerService, TypeMessage, TypePositionMessage } from 'ngx-compack';
import { AuthService } from 'src/app/services/auth.service';
import { ResizeService } from 'src/app/services/resize.service';

@Component({
  selector: 'app-todo-page',
  templateUrl: './todo-page.component.html',
  styleUrls: ['./todo-page.component.scss']
})
export class TodoPageComponent implements OnInit {

  // view
  public isHidden = true;
  public viewWidth = window.innerWidth;

  constructor(
    // private bannerService: CompackBannerService,
    public authService: AuthService,
    private titleService: Title,
    rs: ResizeService
  ) {
    rs.getResizeEvent().subscribe((value: number) => {
      this.viewWidth = value;
      if (value < 991)
        this.isHidden = true;
    });
  }

  ngOnInit() {
    this.titleService.setTitle('todo client app');

    // this.bannerService.removeMessage();
    // this.bannerService.addNewMessage({
    //   position: TypePositionMessage.TopRight,
    //   typeMessage: TypeMessage.Info,
    //   intervalView: 15,
    //   message: 'Пример фронта под задание. \n Для авторизации: \n логин: admin \n пароль: complex'
    // });
  }

}
