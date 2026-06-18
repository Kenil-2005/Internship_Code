import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeWelcomePortal } from './employee-welcome-portal';

describe('EmployeeWelcomePortal', () => {
  let component: EmployeeWelcomePortal;
  let fixture: ComponentFixture<EmployeeWelcomePortal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmployeeWelcomePortal],
    }).compileComponents();

    fixture = TestBed.createComponent(EmployeeWelcomePortal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
