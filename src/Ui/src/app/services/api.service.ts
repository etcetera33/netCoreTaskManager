import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  rootUrl = 'http://da-taskmanager.poliit.rocks/api/';
  constructor() { }
}
