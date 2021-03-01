import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-data-field',
  templateUrl: './data-field.component.html'
})
export class DataFieldComponent {
    @Input() public label: string;
    @Input() public value: string;
}
