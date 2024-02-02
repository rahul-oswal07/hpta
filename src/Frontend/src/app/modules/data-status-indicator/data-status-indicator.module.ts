import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataStatusIndicator } from './data-status-indicator.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [DataStatusIndicator],
  exports: [
    DataStatusIndicator
  ]
})
export class DataStatusIndicatorModule { }
