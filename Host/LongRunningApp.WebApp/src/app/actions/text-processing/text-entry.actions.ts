import { createAction, emptyProps, props } from '@ngrx/store';

export enum TextEntryActionsType {
  TextEntered = '[TextEntry] Text Entered',
  TextCancel = '[TextEntry] Text Cancel'
}

export const TextEntered = createAction(
  TextEntryActionsType.TextEntered,
  props<{ text : string}>()
)

export const TextCancel = createAction(
  TextEntryActionsType.TextCancel
)
