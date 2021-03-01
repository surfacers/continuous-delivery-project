import { StateContext, State, Action, Store, Selector } from '@ngxs/store';
import { map, tap, catchError } from 'rxjs/operators';
import { of, combineLatest } from 'rxjs';

import { RaceDto } from '../dtos';
import { ApiResource, empty, loading, data, error, RaceStatisticEntry } from '../models';
import { RaceService } from '../services/race.service';
import { GetAllRaces, GetRaceById, GetRaceStatistic, SelectRace, GetAllSkiers, GetAllLocations } from '../actions';
import { StatisticService } from '../services/statistic.service';
import { SkierState } from './skier.state';
import { mapStatisticDto } from '../util';

type Context = StateContext<RaceStateModel>;

export interface RaceStateModel {
    races: ApiResource<RaceDto[]>,
    selected: ApiResource<RaceDto>,
    statistic: ApiResource<RaceStatisticEntry[]>
}

const initialState: RaceStateModel = {
    races: empty(),
    selected: empty(),
    statistic: empty()
};

@State<RaceStateModel>({
    name: 'race',
    defaults: initialState
})
export class RaceState {
    constructor(
        private store: Store,
        private raceService: RaceService,
        private statisticService: StatisticService
    ) { }

    @Action(SelectRace)
    selectRace(context: Context, action: SelectRace) {
        const a = context.dispatch(new GetRaceById(action.id));
        const b = context.dispatch(new GetAllSkiers());
        const c = context.dispatch(new GetAllLocations());

        return combineLatest(a, b, c);
    }

    @Action(GetAllRaces)
    getAllRaces(context: Context) {
        context.patchState({ races: loading() });

        return this.raceService.getAll().pipe(
            map(races => {
                races.forEach(r => r.raceDate = new Date(r.raceDate));
                return races;
            }),
            tap(races => context.patchState({ races: data(races) })),
            catchError(e => {
                context.patchState({ races: error(e) })
                return of([]);
            })
        );
    }

    @Action(GetRaceById)
    getRaceById(context: Context, action: GetRaceById) {
        // Alread loaded
        const state = context.getState();
        if (state.races.kind === 'Data') {
            const race = state.races.data.find(s => s.id == action.id);
            context.patchState({ selected: data(race) });
            return;
        }

        // Load from api
        context.patchState({ selected: loading() });
        return this.raceService.getById(action.id).pipe(
            map(race => {
                race.raceDate = new Date(race.raceDate);
                return race;
            }),
            tap(race => context.patchState({ selected: data(race) })),
            catchError(e => {
                context.patchState({ selected: error(e) });
                return of([]);
            })
        );
    }

    @Action(GetRaceStatistic)
    getRaceStatistic(context: Context, action: GetRaceStatistic) {
        context.patchState({ statistic: loading() });

        return this.statisticService.getRaceStatistic(action.id, action.runNumber).pipe(
            map(dtos => {
                const skier = this.store.selectSnapshot(SkierState.getSkier);
                if (skier.kind != 'Data') {
                    throw Error('Skiers not loaded');
                }

                return dtos.map(dto => mapStatisticDto(dto, skier.data));
            }),
            tap(statistic => context.patchState({ statistic: data(statistic) })),
            catchError(e => {
                context.patchState({ statistic: error(e) });
                return of([]);
            })
        );
    }
}
