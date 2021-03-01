import { State, Action, StateContext } from '@ngxs/store';
import { empty, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

import { CountryService } from '../services/country.service';
import { GetAllCountries, GetAllLocations } from '../actions';
import { LocationDto } from '../dtos';

type Context = StateContext<CountryStateModel>;

export type CountryStateModel = {
    countryCodes: string[];
    locations: LocationDto[]
}
const initialState: CountryStateModel = {
    countryCodes: [],
    locations: []
};

@State<CountryStateModel>({
    name: 'countries',
    defaults: initialState
})
export class CountryState {
    constructor(
        private countryService: CountryService
    ) { }

    @Action(GetAllCountries)
    getAllCountries(context: Context) {
        return this.countryService.getCountryCodes().pipe(
            tap(countries => context.patchState({ countryCodes: countries })),
            catchError(_ => {
                context.patchState({ countryCodes: [] });
                return of([]);
            })
        );
    }

    @Action(GetAllLocations)
    getAllLocations(context: Context) {
        return this.countryService.getLocations().pipe(
            tap(countries => context.patchState({ locations: countries })),
            catchError(_ => {
                context.patchState({ locations: [] });
                return of([]);
            })
        );
    }
}
