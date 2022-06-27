import { Injectable } from "@angular/core";
import { AuthResult } from "../share/models/auth/auth-result.model";

@Injectable({ providedIn: 'root' })
export class LoginService {
  isLogin = false;
  userName?: string | null = null;
  id?: string | null = null;
  constructor() {
    this.updateUserLoginState();
  }

  saveLoginState(data: AuthResult): void {
    this.isLogin = true;
    this.userName = data.username;
    localStorage.setItem("id", data.id);
    localStorage.setItem("role", data.role);
    localStorage.setItem("username", data.username);
    localStorage.setItem("accessToken", data.token);
  }

  updateUserLoginState(): void {
    const userId = localStorage.getItem('id');
    const username = localStorage.getItem('username');
    const token = localStorage.getItem('accessToken');
    if (userId && token && username) {
      this.userName = username;
      this.isLogin = true;
    } else {
      this.isLogin = false;
    }
  }
  logout(): void {
    localStorage.clear();
    this.isLogin = false;
  }
}
