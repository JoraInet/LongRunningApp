import { createAction, props } from '@ngrx/store';
import { IProcessTextResponse } from '../../../models/v1/response.api.models';

export enum TextProcessingActionType {
  TextProcessingLoaded = '[TextProcessing] ProcessingText Loaded',
  TextProcessingLoadFailed = '[TextProcessing] ProcessingText Load Failed',
  TextProcessingCancel = '[TextProcessing] ProcessingText Cancel',
  TextProcessingFinished = '[TextProcessing] ProcessingText Finished'
}

export const TextProcessingLoaded = createAction(
  TextProcessingActionType.TextProcessingLoaded,
  props<{ response: IProcessTextResponse }>()
)

export const TextProcessingLoadFailed = createAction(
  TextProcessingActionType.TextProcessingLoadFailed,
  props<{ response: IProcessTextResponse }>()
)

export const TextProcessingCancel = createAction(
  TextProcessingActionType.TextProcessingCancel,
  props<{ response: IProcessTextResponse }>()
)

export const TextProcessingFinished = createAction(
  TextProcessingActionType.TextProcessingFinished
)
