import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { NavigationComponent } from './navigation/navigation.component';
import { ShareModule } from '../share/share.module';


@NgModule({
  declarations: [AdminLayoutComponent, NavigationComponent],
  imports: [
    CommonModule,
    ShareModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
