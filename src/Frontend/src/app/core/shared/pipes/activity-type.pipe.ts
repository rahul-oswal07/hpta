import { Pipe, PipeTransform } from '@angular/core';
import { activityTypes } from 'src/app/core/shared/models/indent';

@Pipe({ name: 'activityTypePipe' })
export class ActivityTypePipe implements PipeTransform {
  transform(id?: number): string {
    return activityTypes.find(activity => activity.id === id)?.value ?? 'Unknown';
  }
}
