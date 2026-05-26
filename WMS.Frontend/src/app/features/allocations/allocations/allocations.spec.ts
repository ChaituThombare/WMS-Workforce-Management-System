import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Allocations } from './allocations';

describe('Allocations', () => {
  let component: Allocations;
  let fixture: ComponentFixture<Allocations>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Allocations],
    }).compileComponents();

    fixture = TestBed.createComponent(Allocations);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
