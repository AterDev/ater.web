import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class AuthService {
    isLogin = false;
    userName?: string | null = null;
    constructor() {
    }

    authorize(): void {}
     
    logout(): void{}
}