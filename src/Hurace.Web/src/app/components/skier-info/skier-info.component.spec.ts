import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkierInfoComponent } from './skier-info.component';

describe('SkierInfoComponent', () => {
  let component: SkierInfoComponent;
  let fixture: ComponentFixture<SkierInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkierInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkierInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
