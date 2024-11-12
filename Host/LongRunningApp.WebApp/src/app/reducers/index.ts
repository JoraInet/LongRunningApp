import { isDevMode } from '@angular/core';
import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { textEntryReducer, TextEntryState } from './text-processing/text-entry.reducer';
import { connectionIdReducer, ConnectionIdState } from './text-processing/connection-id.reducer';
import { textProcessingReducer, TextProcessingState } from './text-processing/text-processing.reducer';
import { textResultReducer, TextResultState } from './text-processing/text-result.reducer';

export interface AppState {
  hubConnectionId: ConnectionIdState,
  text: TextEntryState,
  textProcessing: TextProcessingState,
  textResult: TextResultState
}

export const reducers: ActionReducerMap<AppState> = {
  hubConnectionId: connectionIdReducer,
  text: textEntryReducer,
  textProcessing: textProcessingReducer,
  textResult: textResultReducer
};


export const metaReducers: MetaReducer<AppState>[] = isDevMode() ? [] : [];
