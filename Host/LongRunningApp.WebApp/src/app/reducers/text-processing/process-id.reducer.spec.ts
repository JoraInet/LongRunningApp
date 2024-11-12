import { processIdReducer, initialState } from './process-id.reducer';

describe('ProcessId Reducer', () => {
  describe('an unknown action', () => {
    it('should return the previous state', () => {
      const action = {} as any;

      const result = processIdReducer(initialState, action);

      expect(result).toBe(initialState);
    });
  });
});
