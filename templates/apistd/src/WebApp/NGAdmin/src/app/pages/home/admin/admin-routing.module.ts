import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { AdminLayoutComponent } from 'src/app/components/admin-layout/admin-layout.component';
import { IndexComponent } from './index/index.component';

const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayoutComponent,
    data: {reuse: true},
    canActivate: [AuthGuard],
    children:
      [
        { path: '', pathMatch: 'full', redirectTo: 'index' },
        { path: 'index', component: IndexComponent },
      ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
