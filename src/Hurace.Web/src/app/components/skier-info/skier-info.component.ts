import { Component, Input } from '@angular/core';
import { formatGender } from 'src/app/util';
import { SkierDto } from 'src/app/dtos';

@Component({
    selector: 'app-skier-info',
    templateUrl: './skier-info.component.html',
    styleUrls: ['./skier-info.component.scss']
})
export class SkierInfoComponent {
    @Input() public skier: SkierDto;

    formatGender = formatGender;
}
