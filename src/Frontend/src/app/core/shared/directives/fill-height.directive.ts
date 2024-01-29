import { Directive, ElementRef, HostListener, Input, NgZone, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { Subject, asyncScheduler, combineLatest, distinctUntilChanged, interval, map, observeOn, takeUntil } from 'rxjs';
import { getWindowHeight } from '../utils/functions';
import { enterZone, leaveZone } from 'src/app/core/shared/utils/scheduler';

/**
 * Calculates viewport height + element position and applies
 * 'max-height' css style to fill the remaining screen space.
 */
@Directive({
  selector: '[kbsFillHeight]'
})
export class FillHeightDirective implements OnInit, OnDestroy {
  /**
   * Height in pixels to reduce from bottom.
   */
  @Input() marginBottom = 0;

  /**
   * Add extra padding to prevent overflow
   */
  @Input() paddingBottom = 0;

  /**
   * Frequency in milliseconds, 1000 by default.
   * This should not be too low, because calling getBoundingClientRect might be expensive...
   */
  @Input() pollingFrequency = 1000;

  private readonly viewportHeight$ = new Subject<number>();
  private readonly onDestroy$ = new Subject<void>();

  constructor(
    private readonly elementRef: ElementRef,
    private readonly renderer2: Renderer2,
    private readonly zone: NgZone
  ) { }

  /** OnInit */
  ngOnInit() {
    const yPosition$ = interval(this.pollingFrequency, leaveZone(this.zone, asyncScheduler)).pipe(
      map(() => ((this.elementRef.nativeElement as HTMLElement).getBoundingClientRect()).y),
      observeOn(enterZone(this.zone, asyncScheduler))
    );

    combineLatest(
      [this.viewportHeight$.pipe(distinctUntilChanged()),
      yPosition$.pipe(distinctUntilChanged())]
    ).pipe(
      map(values => this.calcHeight(values[0], values[1], this.marginBottom, this.paddingBottom)),
      distinctUntilChanged(),
      takeUntil(this.onDestroy$)
    ).subscribe(height => {
      this.setHeight(height);
    });

    this.viewportHeight$.next(getWindowHeight());
  }

  /** OnDestroy */
  ngOnDestroy() {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  /** Handle window resize event. */
  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.viewportHeight$.next((event.target as Window).innerHeight);
  }

  private calcHeight(viewportHeight: number, yPosition: number, footerHeight: number, paddingBottom: number) {
    return Math.max(0, viewportHeight - yPosition - footerHeight - paddingBottom);
  }

  private setHeight(height: number) {
    this.renderer2.setStyle(this.elementRef.nativeElement, 'max-height', height + 'px');
  }
}
