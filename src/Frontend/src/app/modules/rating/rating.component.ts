import { Component, Input } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors } from '@angular/forms';
import { LongRating, ShortRating } from 'src/app/modules/rating/rating';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: RatingComponent
    },
    {
      provide: NG_VALIDATORS,
      multi: true,
      useExisting: RatingComponent
    }
  ]
})
export class RatingComponent implements ControlValueAccessor {
  ratingValues: { value: number, text: string, path: string }[] = ShortRating;
  private _scale: 'short' | 'long' = 'short';
  public get scale(): 'short' | 'long' {
    return this._scale;
  }
  @Input()
  public set scale(value: 'short' | 'long') {
    this._scale = value;
    this.ratingValues = value === 'short' ? ShortRating : ShortRating;
  }
  rating?: number;
  onChange = (value: number) => { };

  onTouched = () => { };

  touched = false;

  disabled = false;

  writeValue(value: number) {
    this.rating = value;
  }

  registerOnChange(onChange: any) {
    this.onChange = onChange;
  }

  registerOnTouched(onTouched: any) {
    this.onTouched = onTouched;
  }

  markAsTouched() {
    if (!this.touched) {
      this.onTouched();
      this.touched = true;
    }
  }

  setDisabledState(disabled: boolean) {
    this.disabled = disabled;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    const rating = control.value;
    if (!!rating && (rating <= 0 || rating > 5)) {
      return {
        invalidRating: {
          rating
        }
      };
    }
    return null;
  }

  updateRating(value: number) {
    this.markAsTouched();
    if (!this.disabled) {
      this.rating = value;
      this.onChange(this.rating);
    }
  }
}
