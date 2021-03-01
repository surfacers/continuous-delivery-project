import { Component, Input } from '@angular/core';
import { NavLink } from 'src/app/models/nav-link.model';

@Component({
    selector: 'app-tab-nav',
    templateUrl: './tab-nav.component.html'
})
export class TabNavComponent {
    @Input() public links: NavLink[] = [];
}
