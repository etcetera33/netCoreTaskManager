import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpHeaders
  } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    intercept(
      req: HttpRequest<any>,
      next: HttpHandler
    ): Observable<HttpEvent<any>> {
      const headers = new HttpHeaders();
      headers.append('Authorization', 'Bearer ' + localStorage.getItem('jwt'));
      headers.append('content-type', 'application/json');
      const cloneReq = req.clone({
        headers: req.headers
        .set(
          'Authorization', 'Bearer ' + localStorage.getItem('jwt')
        )
        .set(
          'content-type', 'application/json'
        )
      });
      return next.handle(cloneReq);
    }
}
