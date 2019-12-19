import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  headers =  {
    headers: new HttpHeaders( {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + localStorage.getItem('jwt')
    })
  };
  rootUrl = 'https://localhost:44348/api/';
  constructor() { }
}
