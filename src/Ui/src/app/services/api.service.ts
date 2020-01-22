import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  rootUrl = 'https://localhost:44348/api/';
  constructor() { }
}
