import { Component, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AddBarServiceService } from 'src/app/services/addBarService.service';
import { InfoPanelComponent } from './components/info-panel/info-panel.component';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.scss']
})
export class PostPageComponent implements OnInit {

  constructor(
    private abs: AddBarServiceService,
    private titleService: Title
  ) { }

  ngOnInit() {
    this.titleService.setTitle('post client app');

    this.abs.changeBar(InfoPanelComponent);
  }

}
