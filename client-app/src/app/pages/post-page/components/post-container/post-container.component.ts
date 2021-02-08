import { Component, OnDestroy, OnInit } from '@angular/core';
import * as moment from 'moment';
import { AuthService } from 'src/app/services/auth.service';
import { Post } from '../../model/post';
import { PostService } from '../../services/post.service';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.scss']
})
export class PostContainerComponent implements OnInit, OnDestroy {
  //data
  private listPost: Post[] = [];
  public listViewPost: Post[] = [];
  public textFilter = '';
  // timeOut
  private timeOutFilter: any
  // picker
  public typePicker = 'icon';
  public formatOutPut = 'YYYY-MM-DD HH:mm:ss';

  constructor(
    public authService: AuthService,
    private ps: PostService) {
  }

  ngOnInit() {
    this.ps.getPostSubs()
      .subscribe(next => {
        this.listPost = next;
        this.setTextFilter(false);
      })

    this.ps.getCancelAddPostEventSubs()
      .subscribe(() => {
        this.listPost.shift();
        this.setTextFilter(false);
      });
  }

  ngOnDestroy() {
  }


  public emiteAddPost() {
    if (this.listPost.find(x => x.isNew) == null) {
      this.listPost.unshift({ author: '', date: null, displayDate: '', edited: false, id: -1, text: '', isNew: true, fileDescDto: [] });
      this.setTextFilter(false);
    }
  }

  public selectLastDateEvent(data: string[]) {
    this.setTextFilter(false);
    if (data[0] != 'reset')
      this.listViewPost = this.listViewPost.filter(x =>
        moment(x.date, 'YYYY-MM-DDTHH:mm:ss').isBetween(moment(data[0], this.formatOutPut), moment(data[1], this.formatOutPut))
        || x.isNew)
  }

  public setTextFilter(withTimer: boolean) {
    if (withTimer) {
      if (this.timeOutFilter)
        clearTimeout(this.timeOutFilter);
      this.timeOutFilter = setTimeout(() => {
        this.filtered()
      }, 1000 * 2);
    } else {
      this.filtered();
    }
  }

  private filtered() {
    this.listViewPost = this.listPost.filter(x =>
      x.author.trim().toLowerCase().includes(this.textFilter.trim().toLowerCase())
      || x.text.trim().toLowerCase().includes(this.textFilter.trim().toLowerCase())
      || x.isNew)
  }

}
