import { createReducer, on } from '@ngrx/store';
import { TextResultClosed, TextResultReceived } from '../../actions/text-processing/text-result.actions';

export const textResultFeatureKey = 'textResult';

export interface TextResultState {
  textResult: string,
  progress: number
}

export const initialState: TextResultState = {
  textResult: '',
  progress: 0
};

export const textResultReducer = createReducer<TextResultState>(
  initialState,
  on(TextResultReceived, (state, action) => {
    return {...state,
      textResult: state.textResult + action.result.text,
      progress: action.result.progressPercentage
    }
  }),
  on(TextResultClosed, (state, action) => {
    return {...state,
      textResult: '',
      progress: 0
    }
  })
);

