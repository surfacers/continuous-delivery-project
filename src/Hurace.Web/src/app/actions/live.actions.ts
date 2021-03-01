import { LiveStatisticDto } from '../dtos';
import { RunStoppedReason } from '../models/run-stopped-reason.model';

export class LiveRunUpdate {
    static readonly type = '[Live] RunUpdate';
    constructor(public statistic: LiveStatisticDto) { }
}

export class LiveCurrentRunChange {
    static readonly type = '[Live] CurrentRunChange';
    constructor(public statistic: LiveStatisticDto) { }
}

export class LiveRunStopped {
    static readonly type = '[Live] RunStopped';
    constructor(
        public reason: RunStoppedReason,
        public statistic: LiveStatisticDto) { }
}
