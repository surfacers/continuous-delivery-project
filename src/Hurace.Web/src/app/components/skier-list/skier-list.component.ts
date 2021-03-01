import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';

import { SkierDto } from 'src/app/dtos';
import { GetAllSkiers, NewSkier } from 'src/app/actions';
import { fullName } from 'src/app/util';
import { ApiResource, data, empty } from 'src/app/models';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-skier-list',
    templateUrl: './skier-list.component.html',
    styleUrls: ['./skier-list.component.scss']
})
export class SkierListComponent implements OnInit {
    public skiers: ApiResource<SkierDto[]> = empty();
    public filter = null;

    public get filteredSkiers(): SkierDto[] {
        if (this.skiers.kind == 'Data') {
            const skiers = this.skiers.data.filter(s => !s.isRemoved);

            if (this.filter == null) {
                return skiers;
            }

            const filter = this.filter.toLowerCase();
            return skiers.filter(s => `${s.countryCode} ${fullName(s)}`.toLowerCase().includes(filter));
        }

        return [];
    }

    constructor(
        private store: Store,
        public auth: AuthService
    ) {
        store.select(s => s.skier.all)
            .subscribe((skiers: ApiResource<SkierDto[]>) => this.skiers = skiers);
    }

    ngOnInit() {
        this.store.dispatch(new GetAllSkiers());
    }

    public new() {
        this.store.dispatch(new NewSkier());
    }
}
