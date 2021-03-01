import { Store } from '@ngxs/store';
import { Component, OnInit } from '@angular/core';

import { ApiResource, empty } from 'src/app/models';
import { RaceDto } from 'src/app/dtos';
import { GetAllRaces } from 'src/app/actions';
import { getGenderIcon, formatRaceType, getRaceStateIcon } from 'src/app/util';

@Component({
    selector: 'app-race-list',
    templateUrl: './race-list.component.html',
    styleUrls: ['./race-list.component.scss']
})
export class RaceListComponent implements OnInit {
    public races: ApiResource<RaceDto[]> = empty();
    public filter = null;

    public get filteredRaces(): RaceDto[] {
        if (this.races.kind == 'Data') {
            if (this.filter == null) {
                return this.races.data;
            }

            const filter = this.filter.toLowerCase();
            return this.races.data.filter(r => `${r.name}`.toLowerCase().includes(filter));
        }

        return [];
    }

    constructor(
        private store: Store
    ) {
        store.select(s => s.race.races)
            .subscribe(races => this.races = races);
    }

    ngOnInit() {
        this.store.dispatch(new GetAllRaces());
    }

    public formatRaceType = formatRaceType;
    public getGenderIcon = getGenderIcon;
    public getRaceStateIcon = getRaceStateIcon;
}
