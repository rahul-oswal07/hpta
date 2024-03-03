import { Component, Input } from '@angular/core';
import { ShortRating } from 'src/app/modules/rating/rating';

@Component({
  selector: 'app-rating-view',
  templateUrl: './rating-view.component.html',
  styleUrls: [
    '../rating.component.scss']
})
export class RatingViewComponent {
  rating: { value: number, path: string, text: string };
  private _value: number;
  public get value(): number {
    return this._value;
  }
  @Input()
  public set value(value: number) {
    this._value = value;
    this.rating = ShortRating.find(r => r.value == Math.round(value)) ?? ShortRating[0];
  }
}
