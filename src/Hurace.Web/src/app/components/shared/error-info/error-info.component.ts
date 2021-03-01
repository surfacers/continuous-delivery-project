import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-error-info',
    template: '<app-icon-info icon="alert-outline">{{info}}</app-icon-info>'
})
export class ErrorInfoComponent {
    @Input() public info = "error while loading";
}
