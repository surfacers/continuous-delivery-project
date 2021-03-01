import { State, Action, StateContext, Select, Selector } from '@ngxs/store';
import { of } from 'rxjs';
import { map, tap, catchError, mergeMap } from 'rxjs/operators';
import { Router } from '@angular/router';

import { ApiResource, empty, loading, data, error } from '../models';
import { SkierDto } from '../dtos';
import { SkierService } from '../services/skier.service';
import { GetAllSkiers, GetSkierById, SaveSkier, RemoveSkier, NewSkier } from '../actions';

type Context = StateContext<SkierStateModel>;

export interface SkierStateModel {
    all: ApiResource<SkierDto[]>,
    selected: ApiResource<SkierDto>
}
const initialState: SkierStateModel = {
    all: empty(),
    selected: empty()
};

@State<SkierStateModel>({
    name: 'skier',
    defaults: initialState
})
export class SkierState {
    constructor(
        private router: Router,
        private skierService: SkierService
    ) { }

    @Selector()
    static getSkier(state: SkierStateModel) {
        return state.all;
    }

    @Action(GetAllSkiers)
    getAllSkier(context: Context) {
        context.patchState({ all: loading() });

        return this.skierService.getAll().pipe(
            map(skiers => {
                skiers.forEach(s => s.birthDate = new Date(s.birthDate));
                return skiers;
            }),
            tap(skiers => context.patchState({ all: data(skiers) })),
            catchError(e => {
                context.patchState({ all: error(e) })
                return of([]);
            })
        );
    }

    @Action(GetSkierById)
    getSkierById(context: Context, action: GetSkierById) {
        // Alread loaded
        const state = context.getState();
        if (state.all.kind === 'Data') {
            const skier = state.all.data.find(s => s.id == action.id);
            context.patchState({ selected: data(skier) });
            return;
        }

        // Load from api
        context.patchState({ selected: loading() });
        return this.skierService.getById(action.id).pipe(
            map(skier => {
                if (skier != null) {
                    skier.birthDate = new Date(skier.birthDate);
                }

                return skier;
            }),
            tap(skier => context.patchState({ selected: data(skier) })),
            catchError(e => {
                context.patchState({ selected: error(e) });
                return of([]);
            })
        );
    }

    @Action(NewSkier)
    newSkier(context: Context) {
        this.router.navigateByUrl('/skiers/0');
    }

    @Action(SaveSkier)
    saveSkier(context: Context, action: SaveSkier) {
        context.patchState({ selected: loading() });

        return this.skierService.save(action.skier).pipe(
            tap(() => context.patchState({ selected: empty() })),
            mergeMap(id => [
                context.dispatch(new GetAllSkiers()),
                context.dispatch(new GetSkierById(id))
            ]),
            catchError(e => {
                context.patchState({ selected: error(e) });
                return of([]);
            })
        );
    }

    @Action(RemoveSkier)
    removeSkier(context: Context, action: RemoveSkier) {
        context.patchState({ selected: loading() });

        return this.skierService.remove(action.id).pipe(
            tap(() => context.patchState({ selected: empty() })),
            mergeMap(id => [
                context.dispatch(new GetAllSkiers())
            ]),
            catchError(e => {
                context.patchState({ selected: error(e) });
                return of([]);
            })
        );
    }
}
