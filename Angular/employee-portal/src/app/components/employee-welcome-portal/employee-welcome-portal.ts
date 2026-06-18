import { Component } from '@angular/core';

@Component({
  selector: 'app-employee-welcome-portal',
  imports: [],
  templateUrl: './employee-welcome-portal.html',
  styleUrl: './employee-welcome-portal.css',
})
export class EmployeeWelcomePortal {
  Title = "Employee Welcome Portal";
  portalData = {
    company: {
      name: 'Stridely Solution',
      location: '306, Stridly House, GTPL Lane, Shindhu Bhavan Road, Ahmedabad - 380054',
      email: 'info@stridelysolution.com',
      totalEmployees: '600+'
    },
    employee: {
      id: 101,
      name: 'Kenil Pansara',
      department: 'Web Developer',
      designation: 'Trainee',
      salary: 25000
    },
    training: {
      trainerName: 'Palash Shah',
      technology: 'Angular & TypeScript',
      duration: '4 days'
    },
    developer: {
      name: 'Kenil Pansara',
      college: 'Sigma Institute Of Engineering',
      graduationYear: 2026
    }
  };
}
