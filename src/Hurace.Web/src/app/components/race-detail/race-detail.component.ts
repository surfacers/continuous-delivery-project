import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';

import { SelectRace, GetRaceStatistic } from 'src/app/actions';
import { RaceDto, LiveStatisticDto, LocationDto } from 'src/app/dtos';
import { ApiResource, empty } from 'src/app/models';
import { hasSecondRun } from 'src/app/util';
import { NavLink } from 'src/app/models/nav-link.model';

import { RunStoppedReason } from 'src/app/models/run-stopped-reason.model';

@Component({
    selector: 'app-race-detail',
    templateUrl: './race-detail.component.html',
    styleUrls: ['./race-detail.component.scss']
})
export class RaceDetailComponent implements OnInit {
    public race: ApiResource<RaceDto> = empty();
    public navLinks: NavLink[] = [];

    public liveStatistic: LiveStatisticDto = null;
    public locations: LocationDto[] = [];

    constructor(
        private route: ActivatedRoute,
        private store: Store
    ) {
        store.select(s => s.race.selected).subscribe(race => {
            this.race = race;

            if (race.kind === 'Data') {
                this.navLinks = hasSecondRun(race.data)
                    ? [{ path: '1', label: 'Run 1' }, { path: '2', label: 'Run 2' }]
                    : [{ path: '1', label: 'Run' }]
            }
        });

        store.select(s => s.live.data).subscribe((data: { statistic: LiveStatisticDto, reason: RunStoppedReason }) => {
            this.liveStatistic = data.statistic;

            if (this.showLiveStatistic() && data.reason === 'Finished') {
                this.store.dispatch(new GetRaceStatistic((<any>this.race).data.id, this.liveStatistic.runNumber));
            }
        });

        store.select(state => state.countries.locations).subscribe((locations: LocationDto[]) => this.locations = locations)
    }

    ngOnInit() {
        this.route.params.subscribe(async params => {
            const id = +params['id'];
            this.store.dispatch(new SelectRace(id));
        });
    }

    showLiveStatistic() {
        return this.liveStatistic != null
            && this.race.kind === 'Data'
            && this.liveStatistic.raceId === this.race.data.id;
    }

    get data(): RaceDto {
        if (this.race.kind === 'Data') {
            return this.race.data;
        }

        return null;
    }
}
