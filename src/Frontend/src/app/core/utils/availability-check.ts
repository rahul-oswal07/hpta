import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { Observable, catchError, debounceTime, map, of, switchMap, take } from "rxjs";
import { AvailabilityCheckResult } from "src/app/core/models/availability-check-result";
export type CheckAvailabilityFunction = (name: string, id: any) => Observable<AvailabilityCheckResult>;

export function availabilityValidator(clientFunction: CheckAvailabilityFunction, idKey: string = 'id'): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    if (!control.value) {
      return of(null); // return null if the field is empty
    }
    return control.valueChanges.pipe(
      debounceTime(500),
      take(1),
      switchMap(name => {
        const id = control.parent?.get(idKey)?.value;
        return clientFunction(name, id).pipe(
          map((result) => (result.isAvailable ? null : { taken: true })),
          catchError(() => of({ unavailable: true })) // Handle server or network errors
        )
      }
      )
    );
  };
}
