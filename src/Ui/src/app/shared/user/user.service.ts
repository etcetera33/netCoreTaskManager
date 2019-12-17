import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private rootUrl = 'https://localhost:44348/api/user/';
  constructor(private http: HttpClient) { }

  getUserDictionary() {
    return this.http.get(this.rootUrl + 'dictionary');
  }
}
