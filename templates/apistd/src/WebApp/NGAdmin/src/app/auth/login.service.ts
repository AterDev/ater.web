import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class LoginService {
  isLogin = false;
  userName?: string | null = null;
  constructor() {
  }


  saveLoginState(data: any): void {
    // TODO:实现状态存储
    this.isLogin = true;
  }

  logout(): void {
    // TODO:清空状态
    this.isLogin = false;
  }
}
