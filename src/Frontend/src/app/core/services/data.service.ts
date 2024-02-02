import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { Observable, finalize, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export abstract class DataService {
  abstract readonly path: string;
  private readonly baseUrl = '/api';
  protected http: HttpClient;
  constructor(injector: Injector) {
    this.http = injector.get(HttpClient);
  }

  getList<T>(path?: string, queryParameters: { [key: string]: string | number } | null = null): Observable<T[]> {
    const url = this.getUrl(path, null, this.createHttpParams(queryParameters));
    const headers = { headers: {} };
    return this.http.get(url, headers)
      .pipe(map((response) => {
        return Object.assign([], response);
      }));
  }

  getSingle<T>(path?: string, id?: string | number | null, queryParameters: { [key: string]: string | number } | null = null): Observable<T> {
    const url = this.getUrl(path, id, this.createHttpParams(queryParameters));
    return this.http.get(url)
      .pipe(map(response => {
        return response as T;
      }));
  }

  post<T, R>(data: T, path?: string, queryParameters: { [key: string]: string | number } | null = null): Observable<R> {
    const url = this.getUrl(path, null, this.createHttpParams(queryParameters));
    let httpOptions;
    if (typeof data === 'string' || data instanceof String) {
      httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/x-www-form-urlencoded'
        })
      };
    }
    return this.http.post(url, data, httpOptions)
      .pipe(map(response => {
        return response as R;
      }));
  }

  postAsFormData<T, R>(data: T, path?: string, queryParameters: { [key: string]: string | number } | null = null): Observable<R> {
    const url = this.getUrl(path, null, this.createHttpParams(queryParameters));
    const formData: FormData = new FormData();
    if (data) {
      Object.keys(data).forEach(key => {
        const val = (data as any)[key];
        if (Array.isArray(val)) {
          for (const av of val) {
            formData.append(key + '[]', av);
          }
        } else {
          formData.append(key, val);
        }
      });
    }

    return this.http.post(url, formData)
      .pipe(map(response => {
        return response as R;
      }));
  }

  put<T, R>(id: string | number, data: T, path?: string, queryParameters: { [key: string]: string | number } | null = null): Observable<R> {
    const url = this.getUrl(path, id, this.createHttpParams(queryParameters));
    return this.http.put(url, data)
      .pipe(map(response => {
        return response as R;
      }));
  }

  putAsFormData<T, R>(id: string | number, data: T, path?: string, queryParameters: { [key: string]: string | number } | null = null): Observable<R> {
    const url = this.getUrl(path, id, this.createHttpParams(queryParameters));
    const formData: any = new FormData();
    if (data) {
      Object.keys(data).forEach(key => formData.append(key, (data as any)[key]));
    }
    return this.http.put(url, formData)
      .pipe(map(response => {
        return response as R;
      }));
  }

  delete(id: string | number, path?: string): Observable<any> {
    const url = this.getUrl(path, id);
    return this.http.delete(url)
      .pipe(map(response => {
        return response;
      }));
  }
  deleteSelected(ids: string[], path?: string): any {
    const url = this.getUrl(path);
    return this.http.post(url, { body: { ids } })
      .pipe(map(response => {
        return response;
      }));
  }

  // checkAvailability(path: string, id?: string | number, queryParameters?: any): Observable<DuplicateCheckResult | null> {
  //   return timer(500).pipe(switchMap(() => {
  //     return this.getSingle<DuplicateCheckResult | null>(path, id, queryParameters);
  //   }));
  // }

  protected getUrl(path?: string, id?: string | number | null, queryParameters?: HttpParams): string {
    let url = `${this.baseUrl}/${this.path}`;
    if (path) {
      url += (url.endsWith('/') ? '' : '/') + path;
    }
    if (id) {
      url += (url.endsWith('/') ? '' : '/') + id;
    }
    if (queryParameters && queryParameters.keys().length > 0) {
      url = url.concat('?' + queryParameters.toString());
    }
    return url;
  }

  protected createHttpParams(params: { [key: string]: string | number } | null): HttpParams {
    let httpParams: HttpParams = new HttpParams();
    if (!params || this.isEmpty(params)) {
      return httpParams;
    }
    Object.keys(params).forEach(param => {
      const itm = params[param];
      if (itm) {
        if (Array.isArray(itm)) {
          itm.forEach(val => {
            httpParams = httpParams.append(param, val);
          });
        } else {
          httpParams = httpParams.set(param, itm);
        }
      }
    });

    return httpParams;
  }
  private isEmpty(obj: any): boolean {
    for (const key in obj) {
      if (obj.hasOwnProperty(key)) {
        return false;
      }
    }
    return true;
  }
}
