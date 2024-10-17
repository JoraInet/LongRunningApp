
export interface IProcessTextResponse extends IProcessTextBase {
    processId: string;
}

export interface IProcessTextBase {
    errorMessage: string;
    errorDetails: string;
}