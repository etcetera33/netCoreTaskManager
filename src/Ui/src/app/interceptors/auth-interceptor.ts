import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent
  } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    intercept(
      req: HttpRequest<any>,
      next: HttpHandler
    ): Observable<HttpEvent<any>> {
      if (req.headers.has('Accept')) {
        const cloneReq = req.clone({
          headers: req.headers
          .set(
            'Authorization', 'Bearer ' + localStorage.getItem('jwt')
          )
        });
        return next.handle(cloneReq);
      } else {
        const cloneReq = req.clone({
          headers: req.headers
          .set(
            'Authorization', 'Bearer ' + localStorage.getItem('jwt')
          )
          .set(
            'Content-type', 'application/json'
          )
        });
        return next.handle(cloneReq);
      }
    }
}
