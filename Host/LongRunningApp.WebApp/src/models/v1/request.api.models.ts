
export interface IProcessTextRequest {
  connectionId: string;
  text: string;
}

export interface ICancelProcessTextRequest {
  processId: string;
}
