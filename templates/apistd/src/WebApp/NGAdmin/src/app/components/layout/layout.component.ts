import { isDataSource } from '@angular/cdk/collections';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/auth/login.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {
  isLogin = false;
  username = '';
  constructor(
    private auth: LoginService,
    private router: Router
  ) { }
  ngOnInit(): void {
    this.isLogin = this.auth.isLogin;
    if (this.isLogin) {
      this.username = this.auth.userName!;
    }
  }
  login(): void {
    this.router.navigateByUrl('/login')
    // this.router.navigateByUrl('/login');
  }

  logout(): void {
    this.auth.logout();
  }
}
