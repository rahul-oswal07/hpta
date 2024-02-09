import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, catchError, finalize, map, of, switchMap, take } from 'rxjs';
import { BlockNavigationIfChange } from 'src/app/core/guards/unsaved-changes.guard';
import { CategoriesService } from 'src/app/modules/categories/categories.service';
import { Category } from 'src/app/modules/categories/category';
import { categoryNameAvailabilityValidator } from 'src/app/modules/categories/category-availability-check';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy, BlockNavigationIfChange {
  isLoading = false;
  title = '';
  form = new UntypedFormGroup({
    id: new UntypedFormControl(0),
    name: new UntypedFormControl('',
      {
        validators: [Validators.required],
        asyncValidators: [categoryNameAvailabilityValidator((
          (name, id) => this.categoryService.checkNameAvailability(name, id)
        ))]
      })
  });
  constructor(
    private categoryService: CategoriesService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private snackbarService: MatSnackBar) { }

  ngOnDestroy(): void {
    console.log('On destroy')
  }
  canDeactivate() {
    return !this.form.dirty;
  };

  ngOnInit() {
    this.isLoading = true;
    this.activatedRoute.params.pipe(
      map(params => params['id']),
      switchMap(id => id ? this.categoryService.getCategoryById(id) : of(new Category()))
    ).subscribe({
      next: category => {
        this.form.reset();
        this.form.patchValue(category);
        this.isLoading = false;
        this.title = category.id ? `Edit Category - ${category.name}` : 'Create New Category'
      },
      error: (e: Error) => {
        this.snackbarService.open(e.message, 'Okay');
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    this.isLoading = true;
    let observer: Observable<void>;
    if (this.form.get('id')?.value) {
      observer = this.categoryService.updateCategory(this.form.value);
    } else {
      observer = this.categoryService.addCategory(this.form.value);
    }
    observer.pipe(finalize(() => this.isLoading = false)).subscribe({
      next: () => {
        this.categoryService.requestReload();
        this.router.navigate(['/categories']);
      }, error: (e: Error) => {
        this.form.markAsDirty();
        this.snackbarService.open(e.message, 'Okay');
      }
    })
  }

}
