import { createReducer, on } from '@ngrx/store';
import { TextProcessingCancel, TextProcessingFinished, TextProcessingLoaded, TextProcessingLoadFailed } from '../../actions/text-processing/text-processing.actions';
import { ProcessIdState } from './process-id.reducer';
import { ProcessIdReceived } from '../../actions/text-processing/process-id.actions';

export const textProcessingFeatureKey = 'textProcessing';

export interface TextProcessingState {
  processId: ProcessIdState,
  processError: string,
  isInProgress: boolean
}

export const initialState: TextProcessingState = {
  processId: { processId: ''},
  processError: '',
  isInProgress: false
};

export const textProcessingReducer = createReducer<TextProcessingState>(
  initialState,
  on(TextProcessingLoaded, (state, action) => {
    if (action.response.processId === '00000000-0000-0000-0000-000000000000') {
      return {...state};
    } else {
      return {...state, 
        processId: ProcessIdReceived({ processId: action.response.processId}), 
        isInProgress: true };
    }
  }),
  on(TextProcessingLoadFailed, (state, action) => {
    return {...state, 
      processId: ProcessIdReceived({ processId: '' }),
      processError: action.response.errorMessage, 
      isInProgress: false };
  }),
  on(TextProcessingCancel, (state, action) => {
    return {...state,
      processId: ProcessIdReceived({ processId: '' }),
      processError: '',
      isInProgress: false
    }
  }),
  on(TextProcessingFinished, (state, action) => {
    return {...state,
      processId: ProcessIdReceived({ processId: '' }),
      processError: '',
      isInProgress: false
    }
  })
);

