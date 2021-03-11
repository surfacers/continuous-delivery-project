import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { Props, ApiResource, empty, RunNumber, RaceStatisticEntry } from 'src/app/models';
import { LiveStatisticDto } from 'src/app/dtos';
import { ActivatedRoute } from '@angular/router';
import { combineLatest } from 'rxjs';
import { GetRaceStatistic } from 'src/app/actions';

@Component({
    selector: 'app-race-statistic',
    templateUrl: './race-statistic.component.html'
})
export class RaceStatisticComponent implements OnInit {
    private raceId: number;
    private runNumber: RunNumber;

    public displayedColumns: Props<RaceStatisticEntry>;
    public statistic: ApiResource<RaceStatisticEntry[]> = empty();

    public liveStatistic: LiveStatisticDto = null;

    constructor(
        private route: ActivatedRoute,
        private store: Store
    ) {
        this.store.select(s => s.race.statistic).subscribe(s => {
            this.statistic = s;

            if (this.statistic.kind === 'Data') {
                this.displayedColumns = this.getDisplayedColumns(this.runNumber);
            }
        });

        this.store.select(s => s.live.data.statistic).subscribe((statistic: LiveStatisticDto) => {
            this.liveStatistic = statistic;
        })
    }

    ngOnInit() {
        combineLatest(this.route.parent.params, this.route.params).subscribe(values => {
            this.raceId = +values[0]['id'];
            this.runNumber = +values[1]['runNumber'] as RunNumber;

            this.store.dispatch(new GetRaceStatistic(this.raceId, this.runNumber));
        });
    }

    private getDisplayedColumns(runNumber: RunNumber): Props<RaceStatisticEntry> {
        return runNumber === 1
            ? ['currentPosition', 'skierId', 'skierCountry', 'time', 'deltaTimeLeadership']
            : ['currentPosition', 'deltaPosition', 'skierId', 'skierCountry', 'time', 'deltaTimeLeadership'];
    }

    public isCurrentRun(skierId: number) {
        return this.liveStatistic != null
            && this.liveStatistic.skierId === skierId
            && this.liveStatistic.raceId === this.raceId
            && this.liveStatistic.runNumber === this.runNumber;
    }

    get data(): RaceStatisticEntry[] {
        if (this.statistic.kind === 'Data') {
            return this.statistic.data;
        }

        return [];
    }
}
