import { Component, ElementRef, Input, OnInit, Renderer2 } from '@angular/core';
import { Observable } from 'rxjs';
export type DataStatus = 'Default' | 'Loading' | 'NoData' | 'Error';
@Component({
  selector: 'app-data-status-indicator',
  templateUrl: './data-status-indicator.component.html',
  styleUrls: ['./data-status-indicator.component.css']
})
export class DataStatusIndicator implements OnInit {
  @Input()
  element?: HTMLElement;
  private _status: DataStatus = 'Default';
  public get status(): DataStatus {
    return this._status;
  }
  @Input()
  public set status(value: DataStatus) {
    this._status = value;
    this.setElementVisibility();
  }
  @Input()
  initialState: DataStatus = 'Default';

  @Input()
  errorMessage?: string;
  constructor(private renderer: Renderer2) { }

  ngOnInit() {
    this.status = this.initialState;
  }
  setElementVisibility() {
    if (this.status === 'Default') {
      this.renderer.removeClass(this.element, 'invisible');
    } else {
      this.renderer.addClass(this.element, 'invisible');
    }
  }
  setNoData() {
    this.status = 'NoData';
  }
  setDefault() {
    this.status = 'Default';
  }
  setLoading() {
    this.status = 'Loading';
  }
  setError(message: string) {
    this.status = 'Error';
    this.errorMessage = message;
  }
}
