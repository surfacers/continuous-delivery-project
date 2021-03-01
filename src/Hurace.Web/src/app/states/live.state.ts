import { State, Action, StateContext, Select, Selector } from '@ngxs/store';

import { ApiResource, empty, loading, data, error } from '../models';
import { LiveStatisticDto } from '../dtos';
import { LiveRunUpdate, LiveCurrentRunChange, LiveRunStopped  } from '../actions/live.actions';
import { RunStoppedReason } from '../models/run-stopped-reason.model';

type Context = StateContext<LiveStateModel>;

export interface LiveStateModel {
    data: {
        statistic: LiveStatisticDto | null
        reason: RunStoppedReason | null
    }
}
const initialState: LiveStateModel = {
    data: {
        statistic: null,
        reason: null
    }
};

@State<LiveStateModel>({
    name: 'live',
    defaults: initialState
})
export class LiveState {
    @Action(LiveRunUpdate)
    onRunUpdate(context: Context, action: LiveRunUpdate) {
        context.patchState({ data: { statistic: action.statistic, reason: null }});
    }

    @Action(LiveCurrentRunChange)
    onCurrentRunChange(context: Context, action: LiveCurrentRunChange) {
        context.patchState({ data: { statistic: action.statistic, reason: null }});
    }

    @Action(LiveRunStopped)
    onRunStopped(context: Context, action: LiveRunStopped) {
        context.patchState({ data: { statistic: action.statistic, reason: action.reason }});
    }
}
