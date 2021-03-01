import { StateContext, State, Action } from '@ngxs/store';
import { ApiResource, empty, loading, data, error } from '../models';
import { GetSeasons } from '../actions/season.actions';
import { map, catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { SeasonDto } from '../dtos/season.dto';
import { SeasonService } from '../services/season.service';

type Context = StateContext<SeasonStateModel>;

export interface SeasonStateModel {
    seasons: ApiResource<SeasonDto[]>
}

const initialState: SeasonStateModel = {
    seasons: empty()
};

@State<SeasonStateModel>({
    name: 'season',
    defaults: initialState
})
export class SeasonState {
    constructor(
        private seasonService: SeasonService
    ) { }

    @Action(GetSeasons)
    getSeasons(context: Context) {
        context.patchState({ seasons: loading() });

        return this.seasonService.getAllSeasons().pipe(
            map(seasons => {
                seasons.forEach(s => {
                    s.from = new Date(s.from);
                    s.to = new Date(s.to);
                });
                return seasons;
            }),
            tap(seasons => context.patchState({ seasons: data(seasons) })),
            catchError(e => {
                context.patchState({ seasons: error(e) });
                return of(e);
            })
        );
    }
}
