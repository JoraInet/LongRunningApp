import { textEntryReducer, initialState } from './text-entry.reducer';

describe('TextEntry Reducer', () => {
  describe('an unknown action', () => {
    it('should return the previous state', () => {
      const action = {} as any;

      const result = textEntryReducer(initialState, action);

      expect(result).toBe(initialState);
    });
  });
});
