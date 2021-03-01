import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'app-icon-info',
    template: `
        <div class="align-center align-items-center d-flex flex-1 flex-col p-4 text-larger text-muted">
            <div class="mdi mdi-36px mdi-{{ icon }}"></div>
            <div class="p-3">
                <ng-content></ng-content>
            </div>
        </div>
    `
})
export class IconInfoComponent {
    @Input() public icon;
}
