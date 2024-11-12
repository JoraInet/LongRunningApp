import { textResultReducer, initialState } from './text-result.reducer';

describe('TextResult Reducer', () => {
  describe('an unknown action', () => {
    it('should return the previous state', () => {
      const action = {} as any;

      const result = textResultReducer(initialState, action);

      expect(result).toBe(initialState);
    });
  });
});
