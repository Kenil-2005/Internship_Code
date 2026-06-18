import { Component } from '@angular/core';

@Component({
  selector: 'app-header-component',
  imports: [],
  templateUrl: './header-component.html',
  styleUrl: './header-component.css',
})
export class HeaderComponent {
  HeaderData = {
    Name: 'ABC Technologies', Logo: 'Dev.', Title: 'Employee Managment Portal'
  };
}
