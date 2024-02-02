export const defaultDateFormat = 'yyyy/MM/dd';
export const defaultTimeFormat = 'h:mmaa';
export const defaultDateTimeFormat = 'yyyy/MM/dd h:mmaa';
import { AbstractControl, FormControl, ValidationErrors } from '@angular/forms';
import { DialogPosition } from '@angular/material/dialog';

export const secondsPerDay = 86400;


const timeRegex = /^\s*([0-9]?[0-9]):([0-9]{2})\s*(am|pm)?\s*$/i;

export const isValidTimeString = (text: string): boolean => {
  if (typeof text !== 'string') {
    return false;
  }
  const matchedText = text.match(timeRegex);
  if (!matchedText) {
    return false;
  }
  const minutes = +matchedText[2];
  if (minutes < 0 || minutes > 59) {
    return false;
  }
  const meridiem = matchedText[3];
  const hours = +matchedText[1];
  const minHour = meridiem ? 1 : 0;
  const maxHour = meridiem ? 12 : 23;
  if (hours < minHour || hours > maxHour) {
    return false;
  }
  return true;
};

export const getNormalTimeString = (text: string): string => {
  if (!isValidTimeString(text)) {
    return '';
  }
  const matchedText = text.match(timeRegex);
  if (!matchedText) {
    return '';
  }
  const hours = + matchedText[1];
  const minutes = +matchedText[2];
  const meridiem = matchedText[3];
  const formattedHours = (meridiem?.toLowerCase() === 'pm' ? 12 + hours : hours)
    .toString()
    .padStart(2, '0');
  const formattedMinutes = minutes.toString().padStart(2, '0');
  return formattedHours + ':' + formattedMinutes;
};

export function setControlDisabled(control: AbstractControl, disabled: boolean): void {
  if (control.disabled === disabled) {
    return;
  }
  if (disabled) {
    control.disable();
  } else {
    control.enable();
  }
}

export const timeStringValidator: ValidationErrors = (control: FormControl) => {
  if (control.value && !isValidTimeString(control.value)) {
    return { timeString: true };
  }
  return null;
};

export const positionTopLeftRelativeToTopRight = (relativeTo: HTMLElement): DialogPosition => {
  if (relativeTo == null) {
    return { top: '0px', left: '0px' };
  }
  const clientRect = relativeTo.getBoundingClientRect();
  return { top: clientRect.top + 4 + 'px', left: clientRect.right + 'px' };
};

export const positionTopLeftRelativeToTopLeft = (relativeTo: HTMLElement): DialogPosition => {
  if (relativeTo == null) {
    return { top: '0px', left: '0px' };
  }
  const clientRect = relativeTo.getBoundingClientRect();
  return { top: clientRect.top + 4 + 'px', left: clientRect.left + 'px' };
};
export const valueValidator: ValidationErrors = (control: FormControl) => {
  const value = control.value as number;
  if (!value?.toString().trim().length) {
    return { required: true };
  }
  return null;
}
