import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetSeasons } from 'src/app/actions/season.actions';
import { RaceDto } from 'src/app/dtos';
import { ApiResource } from 'src/app/models';
import { RaceType } from 'src/app/enums';
import { formatRaceType, formatGender } from 'src/app/util';
import { Props } from 'src/app/models/props.model';
import { GetAllRaces } from 'src/app/actions';
import { SeasonDto } from 'src/app/dtos/season.dto';

interface RaceGroup {
    raceType: RaceType,
    data: RaceDto[]
}

@Component({
    selector: 'app-season',
    templateUrl: './season.component.html',
    styleUrls: ['./season.component.scss']
})
export class SeasonComponent implements OnInit {
    public races: ApiResource<RaceDto[]>;
    public seasons: ApiResource<SeasonDto[]>;

    public selectedSeason: SeasonDto = null;

    public displayedColumns: Props<RaceDto & { actions: any }> = ['name', 'raceDate', 'gender', 'actions'];

    public total: number;
    public groups: RaceGroup[];

    constructor(
        private store: Store
    ) {
        this.store.select(s => s.season.seasons).subscribe((seasons: ApiResource<SeasonDto[]>) => {
            this.seasons = seasons;
            this.selectedSeason = null;

            if (seasons.kind === 'Data') {
                this.selectedSeason = seasons.data[0];
            }
        });

        this.store.select(s => s.race.races).subscribe((races: ApiResource<RaceDto[]>) => {
            this.races = races;
            this.groupRaces();
        })
    }

    ngOnInit() {
        this.store.dispatch(new GetSeasons()).subscribe(() => {
            this.store.dispatch(new GetAllRaces());
        });
    }

    private groupRaces() {
        if (this.races.kind === 'Data' && this.selectedSeason != null) {
            const races = this.races.data
                .filter(r => r.raceDate >= this.selectedSeason.from
                    && r.raceDate <= this.selectedSeason.to);

            this.total = races.length;
            this.groups = [RaceType.Slalom, RaceType.GiantSlalom, RaceType.SuperG, RaceType.Downhill]
                .map(raceType => ({
                    raceType: raceType,
                    data: races.filter(r => r.raceType === raceType)
                }));
        }
    }

    public selectionChanged(season: SeasonDto) {
        this.selectedSeason = season;
        this.groupRaces();
    }

    formatRaceType = formatRaceType;
    formatGender = formatGender;
}
