import { Component, signal } from '@angular/core';
import { HeaderComponent } from "./components/header-component/header-component";
import { EmployeeComponent } from "./components/employee-component/employee-component";
import { FooterComponent } from "./components/footer-component/footer-component";

@Component({
  selector: 'app-root',
  imports: [HeaderComponent, EmployeeComponent, FooterComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('employee-portal');
}
