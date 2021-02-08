import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ResizeService {

  private resizeHeightEvent$: EventEmitter<number> = new EventEmitter<number>();

  constructor() {
    window.onresize = () => {
      this.resizeHeightEvent$.emit(window.innerWidth)
    }
  }

  public getResizeEvent() {
    return this.resizeHeightEvent$;
  }

}
