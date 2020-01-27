import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  rootUrl = 'http://localhost:8037/api/';
  constructor() { }
}
