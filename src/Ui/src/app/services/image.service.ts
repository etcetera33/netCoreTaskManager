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

  getImages(page?: number, itemsPerPage = 10) {
    const objectUrl: any = {};
    if (page != null) {
      objectUrl.page = page;
    }
    objectUrl.itemsPerPage = itemsPerPage;
    const params = new HttpParams({
      fromObject: objectUrl
    });
    return this.http.get<any>(this.rootUrl + '?', {params});
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
    return this.http.delete(this.rootUrl + imageId + '/work-items/' + workItemId);
  }
}
