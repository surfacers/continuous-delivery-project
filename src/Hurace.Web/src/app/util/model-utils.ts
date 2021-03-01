import { SkierDto, RaceStatisticEntryDto, RaceDto } from '../dtos';
import { Gender, RaceType, RaceState } from '../enums';
import { RaceStatisticEntry } from '../models';

export const fullName = (s: SkierDto) => `${s.firstName} ${s.lastName}`;

export const newSkier = (): SkierDto => ({
    id: 0,
    firstName: '',
    lastName: '',
    gender: Gender.Male,
    countryCode: null,
    birthDate: null,
    isActive: true,
    image: null,
    isRemoved: false
});

export const formatGender = (gender: Gender) => {
    switch (gender) {
        case Gender.Male: return 'Male';
        case Gender.Female: return 'Female';
        default: throw Error('Gender not defined');
    }
}

export const getGenderIcon = (gender: Gender) => {
    switch (gender) {
        case Gender.Male: return 'mdi mdi-gender-male';
        case Gender.Female: return 'mdi mdi-gender-female';
        default: throw Error('Gender not defined');
    };
}

export const formatRaceType = (raceType: RaceType) => {
    switch (raceType) {
        case RaceType.Slalom: return 'Slalom';
        case RaceType.GiantSlalom: return 'Giant Slalom';
        case RaceType.SuperG: return 'Super-G';
        case RaceType.Downhill: return 'Downhill';
        default: throw Error('RaceType not defined');
    }
}

export const getRaceStateIcon = (raceState: RaceState) => {
    switch (raceState) {
        case RaceState.NotStarted: return "mdi mdi-square-small";
        case RaceState.Running: return "mdi mdi-access-point";
        case RaceState.Canceled: return "mdi mdi-flag-remove";
        case RaceState.Done: return "mdi mdi-flag-checkered";
        default: throw Error('RaceState not defined');
    }
}

export const mapStatisticDto = (dto: RaceStatisticEntryDto, skiers: SkierDto[]): RaceStatisticEntry => {
    const skier = skiers.find(s => s.id === dto.skierId);
    if (skier == null) {
        throw Error('Skier not found');
    }

    return {
        ...dto,
        skierName: fullName(skier),
        skierCountry: skier.countryCode
    };
}

export const hasSecondRun = (r: RaceDto) =>
    r.raceType === RaceType.Slalom || r.raceType === RaceType.GiantSlalom;
