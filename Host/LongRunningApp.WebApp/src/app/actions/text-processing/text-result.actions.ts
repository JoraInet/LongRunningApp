import { createAction, emptyProps, props } from '@ngrx/store';
import { IProcessingText } from '../../../models/v1/response.hub.models';

export enum TextResultActionsType {
  TextResultReceived = '[TextResult] TextResult Received',
  TextResultClosed = '[TextResult] TextResult Closed',
}

export const TextResultReceived = createAction(
  TextResultActionsType.TextResultReceived,
  props<{ result:IProcessingText }>()
)

export const TextResultClosed = createAction(
  TextResultActionsType.TextResultClosed
)


