import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { BlockNavigationIfChange } from 'src/app/core/guards/unsaved-changes.guard';
import { availabilityValidator } from 'src/app/core/utils/availability-check';
import { SubCategoriesService } from '../sub-categories.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, finalize, map, of, switchMap } from 'rxjs';
import { SubCategory } from '../sub-category';

@Component({
  selector: 'app-edit-sub-category',
  templateUrl: './edit-sub-category.component.html',
  styleUrls: ['./edit-sub-category.component.css'],
})
export class EditSubCategoryComponent
  implements OnInit, OnDestroy, BlockNavigationIfChange
{
  isLoading = false;
  title = '';
  form = new UntypedFormGroup({
    id: new UntypedFormControl(0),
    categoryId: new UntypedFormControl(null),
    name: new UntypedFormControl('', {
      validators: [Validators.required],
      asyncValidators: [
        availabilityValidator((name, id) =>
          this.categoryService.checkNameAvailability(name, id)
        ),
      ],
    }),
  });
  constructor(
    private categoryService: SubCategoriesService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private snackbarService: MatSnackBar
  ) {}

  ngOnDestroy(): void {
    console.log('On destroy');
  }
  canDeactivate() {
    return !this.form.dirty;
  }

  ngOnInit() {
    this.isLoading = true;
    this.activatedRoute.params
      .pipe(
        map((params) => {
          return { id: params['id'], categoryId: params['categoryId'] };
        }),
        switchMap((p) => {
          if (p.id) return this.categoryService.getSubCategoryById(p.id);
          const result = new SubCategory();
          result.categoryId = p.categoryId;
          return of(result);
        })
      )
      .subscribe({
        next: (category) => {
          this.form.reset();
          this.form.patchValue(category);
          this.isLoading = false;
          this.title = category.id
            ? `Edit Sub-Category - ${category.name}`
            : 'Create New Sub-Category';
        },
        error: (e: Error) => {
          this.snackbarService.open(e.message, 'Okay');
          this.isLoading = false;
        },
      });
  }

  onSubmit() {
    this.isLoading = true;
    let observer: Observable<void>;
    if (this.form.get('id')?.value) {
      observer = this.categoryService.updateSubCategory(this.form.value);
    } else {
      observer = this.categoryService.addSubCategory(this.form.value);
    }
    observer.pipe(finalize(() => (this.isLoading = false))).subscribe({
      next: () => {
        this.categoryService.requestReload();
        this.router.navigate(['/subcategories']);
      },
      error: (e: Error) => {
        this.form.markAsDirty();
        this.snackbarService.open(e.message, 'Okay');
      },
    });
  }
}
