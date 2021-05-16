import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AddBarServiceService } from 'src/app/services/addBarService.service';
import { TodoRecordInfoPanelComponent } from './components/todo-record-info-panel/todo-record-info-panel.component';

@Component({
  selector: 'app-todo-page',
  templateUrl: './todo-page.component.html',
  styleUrls: ['./todo-page.component.scss']
})
export class TodoPageComponent implements OnInit {

  constructor(
    private abs: AddBarServiceService,
    private titleService: Title) { }

  ngOnInit() {
    this.titleService.setTitle('todo client app');

    this.abs.changeBar(TodoRecordInfoPanelComponent);
  }

}
