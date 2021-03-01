import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LiveStatisticComponent } from './live-statistic.component';

describe('LiveStatisticComponent', () => {
  let component: LiveStatisticComponent;
  let fixture: ComponentFixture<LiveStatisticComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LiveStatisticComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LiveStatisticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
