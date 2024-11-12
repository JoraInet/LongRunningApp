import { connectionIdReducer, initialState } from './connection-id.reducer';

describe('ConnectionId Reducer', () => {
  describe('an unknown action', () => {
    it('should return the previous state', () => {
      const action = {} as any;

      const result = connectionIdReducer(initialState, action);

      expect(result).toBe(initialState);
    });
  });
});
