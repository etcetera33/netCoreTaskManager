import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  rootUrl = 'http://localhost:9001/api/';
  constructor() { }
}
