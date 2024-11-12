import { createAction,  props } from '@ngrx/store';

export enum ConnectionIdActionsType {
  ConnectionIdReceived = '[ConnectionId] ConnectionId received'
}

export const ConnectionIdReceived = createAction(
  ConnectionIdActionsType.ConnectionIdReceived,
  props<{ connectionId : string }>()
)