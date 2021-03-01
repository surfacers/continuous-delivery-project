import { LiveRaceDataDto } from './live-race-data.dto';
import { RunNumber } from '../models';

export interface LiveStatisticDto {
    skierId: number,
    firstName: string,
    lastName: string,
    countryCode: string,
    raceId: number,
    runNumber: RunNumber,
    raceData: LiveRaceDataDto[],
    totalTime: string,
}
