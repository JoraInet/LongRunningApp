import { textProcessingReducer, initialState } from './text-processing.reducer';

describe('TextProcessing Reducer', () => {
  describe('an unknown action', () => {
    it('should return the previous state', () => {
      const action = {} as any;

      const result = textProcessingReducer(initialState, action);

      expect(result).toBe(initialState);
    });
  });
});
