import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { CompackBannerService, TypeMessage, TypePositionMessage } from 'ngx-compack';
import { ResizeService } from 'src/app/services/resize.service';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.scss']
})
export class PostPageComponent implements OnInit {

  // view
  public isHidden = true;
  public viewWidth = window.innerWidth;

  constructor(
    private titleService: Title,
    private bannerService: CompackBannerService,
    rs: ResizeService
  ) {
    rs.getResizeEvent().subscribe((value: number) => {
      this.viewWidth = value;
      if (value < 965)
        this.isHidden = true;
    });
  }

  ngOnInit() {
    this.titleService.setTitle('post client app');

    // this.bannerService.removeMessage();
    // this.bannerService.addNewMessage({
    //   position: TypePositionMessage.TopRight,
    //   typeMessage: TypeMessage.Info,
    //   intervalView: 15,
    //   message: 'Пример фронта под тз. \n Для авторизации: \n логин: admin \n пароль: complex'
    // });
  }

}
