import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-component',
  imports: [FormsModule],
  templateUrl: './employee-component.html',
  styleUrl: './employee-component.css',
})
export class EmployeeComponent {
  eid = 104;
  InputName = '';
  SearchInput = '';
  TextAreaName = '';

  EmployeeData = [
    { id: 101, name: 'kenil pansara', department: 'information Technology', designation: 'Developer', salary: 50000, image: './assets/profile-pic.jpg' },
    { id: 102, name: 'kenil pansara', department: 'information Technology', designation: 'Developer', salary: 50000, image: './assets/profile-pic.jpg' },
    { id: 103, name: 'kenil pansara', department: 'information Technology', designation: 'Developer', salary: 50000, image: './assets/profile-pic.jpg' },
    { id: 104, name: 'kenil pansara', department: 'information Technology', designation: 'Developer', salary: 50000, image: './assets/profile-pic.jpg' }
  ];

  count = this.EmployeeData.length;

  Decrease() {
    this.count--;
    this.eid--;
    this.EmployeeData.pop();
  }
  Increase() {
    this.count++;
    this.eid++;
    this.EmployeeData.push({ id: this.eid, name: 'kenil pansara', department: 'information Technology', designation: 'Developer', salary: 50000, image: './assets/profile-pic.jpg' })
  }

  Promote() {
    alert("Employee is promoted.");
  }

  SendEmail() {
    alert("Email Sended Successfully.");
  }
}
