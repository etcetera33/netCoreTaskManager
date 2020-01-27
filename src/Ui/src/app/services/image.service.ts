import { ApiService } from './api.service';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { File } from './../models/file';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private rootUrl = this.apiService.rootUrl + 'files/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  createImage(data) {
    const HttpUploadOptions = {
      headers: new HttpHeaders({ Accept: 'application/json'})
    };
    return this.http.post(this.rootUrl, data, HttpUploadOptions);
  }

  attachToWorkItem(images: File[], workItemId: number) {
    const data: any = {};
    data.files = images;

    return this.http.post(this.rootUrl + 'work-items/' + workItemId, JSON.stringify(images));
  }

  getWorkItemImages(workItemId: number) {
    return this.http.get(this.rootUrl + 'work-items/' + workItemId);
  }

  removeFromAttachedToWorkItem(imageId: number, workItemId: number) {
    return this.http.delete(this.rootUrl + imageId + '/' + workItemId);
  }

  remove(data) {
    return this.http.request('delete', this.rootUrl, {body: data});
  }
}
