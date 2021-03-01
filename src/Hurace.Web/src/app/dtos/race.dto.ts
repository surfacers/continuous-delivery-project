import { Gender, RaceType, RaceState } from '../enums';

export interface RaceDto {
    id: number,
    name: string,
    description: string,
    raceDate: Date,
    raceType: RaceType,
    locationId: number,
    sensorAmount: number,
    gender: Gender,
    raceState: RaceState
}
