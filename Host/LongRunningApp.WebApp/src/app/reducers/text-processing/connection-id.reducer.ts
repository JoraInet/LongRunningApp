import { createReducer, on } from '@ngrx/store';
import { ConnectionIdReceived  } from '../../actions/text-processing/connection-id.actions';

export const connectionIdFeatureKey = 'connectionId';

export interface ConnectionIdState {
  connectionId : string
}

export const initialState: ConnectionIdState = {
  connectionId : ''
};

export const connectionIdReducer = createReducer(
  initialState,
  on(ConnectionIdReceived, (state, { connectionId }) => {
    return {...state, connectionId: connectionId};
  })
)

