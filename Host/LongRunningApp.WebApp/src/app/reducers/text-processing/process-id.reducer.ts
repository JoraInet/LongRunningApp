import { createReducer, on } from '@ngrx/store';
import { ProcessIdReceived } from '../../actions/text-processing/process-id.actions';

export const processIdFeatureKey = 'processId';

export interface ProcessIdState {
  processId : string
}

export const initialState: ProcessIdState = {
  processId : ''
};

export const processIdReducer = createReducer(
  initialState,
  on(ProcessIdReceived, (state, { processId }) => {
    return {...state, processId: processId };
  })
);

