import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Observable, catchError, throwError } from "rxjs";
import { IProcessTextRequest, ICancelProcessTextRequest } from "../../../models/v1/request.api.models";
import { IProcessTextResponse } from "../../../models/v1/response.api.models";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})

export class TextProcessorApiConnectionService {

  private apiPrefix: string = 'http://localhost:7145/api/v1/text-processor/';

  constructor(private http: HttpClient) { }

  sendProcessTextRequest(request: IProcessTextRequest) : Observable<IProcessTextResponse> {
    return this.http.post<IProcessTextResponse>(this.apiPrefix + 'start-process', request)
      .pipe(catchError((err : HttpErrorResponse) => { return throwError(err.error as IProcessTextResponse)}));
  }

  sendCancelProcessTextRequest(request: ICancelProcessTextRequest) : Observable<IProcessTextResponse> {
    return this.http.delete<IProcessTextResponse>(this.apiPrefix + 'cancel-process?processId=' + request.processId)
      .pipe(catchError((err : HttpErrorResponse) => { return throwError(err.error as IProcessTextResponse)}));
  }


}
