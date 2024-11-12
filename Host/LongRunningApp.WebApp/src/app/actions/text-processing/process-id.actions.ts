import { createAction, props } from '@ngrx/store';

export enum ProcessIdActionsType {
  ProcessIdReceived = '[ProcessId] ProcessId Received'
}

export const ProcessIdReceived = createAction(
  ProcessIdActionsType.ProcessIdReceived,
  props<{ processId : string }>()
)