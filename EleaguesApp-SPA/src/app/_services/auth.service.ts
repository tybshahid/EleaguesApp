import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { UserForRegister } from '../_models/userForRegister';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  formData: FormData;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) {}

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      })
    );
  }

  // register(user: User) {
  //   return this.http.post(this.baseUrl + 'register', user);
  // }

  register(user: UserForRegister) {
    this.formData = new FormData();
    // Object.keys(user).forEach(key => this.formData.append(key, user[key]));
    if (user.file) {
      this.formData.append('file', user.file, user.file.name);
    }
    this.formData.append('username', user.username);
    this.formData.append('knownAs', user.knownAs);
    this.formData.append('psn', user.psn);
    this.formData.append('country', user.country);
    this.formData.append('email', user.email);
    this.formData.append('mobile', user.mobile);
    this.formData.append(
      'dateOfBirth',
      new Date(user.dateOfBirth).toUTCString()
    );
    this.formData.append('password', user.password);

    return this.http.post(this.baseUrl + 'register', this.formData);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  isAdmin() {
    return this.currentUser.isAdmin;
  }
}
