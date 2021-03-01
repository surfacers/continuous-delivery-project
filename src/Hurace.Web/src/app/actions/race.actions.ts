import { RunNumber } from '../models';

export class SelectRace {
    static readonly type = '[Race] Select';
    constructor(public id: number) { }
}

export class GetAllRaces {
    static readonly type = '[Race] GetAll';
}

export class GetRaceById {
    static readonly type = '[Race] GetById';
    constructor(public id: number) { }
}

export class GetRaceStatistic {
    static readonly type = '[Race] GetStatistic';
    constructor(
        public id: number,
        public runNumber: RunNumber
    ) { }
}
