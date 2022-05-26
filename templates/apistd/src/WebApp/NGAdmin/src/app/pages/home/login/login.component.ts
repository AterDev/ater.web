import { Component, OnInit } from '@angular/core';
// import { OAuthService, OAuthErrorEvent, UserInfo } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public loginForm!: FormGroup; constructor(
    // private oauthService: OAuthService,
    private router: Router

  ) {
  }
  get email() { return this.loginForm.get('email'); }
  get password() { return this.loginForm.get('password'); }
  ngOnInit(): void {
    // const token = this.oauthService.getAccessToken();
    // const cliams = this.oauthService.getIdentityClaims();
    // if (token && cliams) {
    //   this.router.navigateByUrl('/index');
    // }

    // this.oauthService.events.subscribe(event => {
    //   if (event instanceof OAuthErrorEvent) {
    //     // TODO:处理错误
    //     console.error(event);
    //   } else {
    //     if (event.type === 'token_received' || event.type === 'token_refreshed') {
    //       this.oauthService.loadUserProfile()
    //         .then(() => {
    //           this.router.navigateByUrl('/index');
    //         });
    //     }
    //   }
    // });
    console.log('asdas');

    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(50)])
    });
  }


  /**
   * 错误信息
   * @param type 字段名称
   */
  getValidatorMessage(type: string): string {
    switch (type) {
      case 'email':
        return this.email?.errors?.['required'] ? '邮箱必填' :
          this.email?.errors?.['email'] ? '错误的邮箱格式' : '';
      case 'password':
        return this.password?.errors?.['required'] ? '密码必填' :
          this.password?.errors?.['minlength'] ? '密码长度不可低于6位' :
            this.password?.errors?.['maxlength'] ? '密码长度不可超过50' : '';
      default:
        break;
    }
    return '';
  }
  doLogin(): void {

  }

  logout(): void {

  }

  get userName(): string | null {
    return '';
  }

}