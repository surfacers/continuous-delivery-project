import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RaceStatisticComponent } from './race-statistic.component';

describe('RaceStatisticComponent', () => {
  let component: RaceStatisticComponent;
  let fixture: ComponentFixture<RaceStatisticComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RaceStatisticComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RaceStatisticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
