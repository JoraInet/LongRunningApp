import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { IProcessTextRequest } from "../../../models/v1/request.api.models";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})

export class TextProcessorApiConnectionService {

  private apiPrefix: string = 'https://localhost:7146/api/v1/text-processor/';

  private cancelSubject: Subject<void> = new Subject<void>();

  constructor(private http: HttpClient) { }

  sendProcessTextRequest(request: IProcessTextRequest) {

    this.cancelSubject = new Subject<void>();

    return this.http.post(this.apiPrefix + 'process-text', request)
      .pipe(takeUntil(this.cancelSubject));
  }

  cancelRequest() {
    this.cancelSubject.next();
  }
}
