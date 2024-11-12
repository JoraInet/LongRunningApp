import { createReducer, on } from '@ngrx/store';
import { TextEntered } from '../../actions/text-processing/text-entry.actions';

export const textEntryFeatureKey = 'textEntry';

export interface TextEntryState {
  text: string
}

export const initialState: TextEntryState = {
  text: ''
};

export const textEntryReducer = createReducer<TextEntryState>(
  initialState,
  on(TextEntered, (state, { text }) => {
    return {...state, text: text};
  })
);

