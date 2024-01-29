import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'timeUnitPipe' })
export class TimeUnitPipe implements PipeTransform {
  transform(id?: number): string {
    const timeUnits = [
      { id: 0, value: 'Minutes' },
      { id: 1, value: 'Hours' }
    ];
    return timeUnits.find(unit => unit.id === id)?.value ?? 'Unknown';
  }
}
