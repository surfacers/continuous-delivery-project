import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { FormBuilder, Validators, FormGroup, AbstractControl } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { SkierDto } from 'src/app/dtos';
import { Gender } from 'src/app/enums';
import { getErrorMessage, hasError, newSkier, lengthValidator } from 'src/app/util';
import { GetAllCountries, SaveSkier, RemoveSkier } from 'src/app/actions';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-skier-edit',
    templateUrl: './skier-edit.component.html',
    styleUrls: ['./skier-edit.component.scss']
})
export class SkierEditComponent implements OnInit, OnChanges {
    @Input() public skier: SkierDto;

    public countryCodes$: Observable<string[]>;
    public skierForm: FormGroup = null;

    constructor(
        private fb: FormBuilder,
        public auth: AuthService,
        private store: Store
    ) {
        this.countryCodes$ = this.store.select(state => state.countries.countryCodes);
    }

    ngOnInit() {
        this.store.dispatch(new GetAllCountries());
    }

    ngOnChanges() {
        const formValues = this.skier != null
            ? this.skier
            : newSkier();

        this.skierForm = this.initForm();
        this.skierForm.patchValue(formValues);
    }

    private initForm() {
        return this.fb.group({
            firstName: ['', lengthValidator(2, 100)],
            lastName: ['', lengthValidator(2, 100)],
            gender: [Gender.Male],
            countryCode: ['', lengthValidator(3, 3)],
            birthDate: [null],
            isActive: [true],
            image: [null, Validators.maxLength(500)]
        })
    }

    public async save() {
        this.skierForm.markAllAsTouched();
        if (this.skierForm.valid) {
            const skier: SkierDto = {
                ...this.skier,
                ...this.skierForm.getRawValue()
            };

            this.store.dispatch(new SaveSkier(skier));
        }
    }

    public remove() {
        this.store.dispatch(new RemoveSkier(this.skier.id));
    }

    public hasError(formControlName: string) {
        return hasError(this.skierForm, formControlName);
    }

    public getErrorMessage(formControlName: string) {
        return getErrorMessage(this.skierForm, formControlName);
    }
}
