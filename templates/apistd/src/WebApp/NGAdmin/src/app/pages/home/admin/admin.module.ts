import { NgModule } from '@angular/core';
import { AdminRoutingModule } from './admin-routing.module';
import { IndexComponent } from './index/index.component';
import { ShareModule } from 'src/app/share/share.module';
import { ComponentsModule } from 'src/app/components/components.module';
import { RouteReuseStrategy } from '@angular/router';
import { CustomRouteReuseStrategy } from 'src/app/custom-route-strategy';


@NgModule({
  declarations: [
    IndexComponent
  ],
  imports: [
    ComponentsModule,
    ShareModule,
    AdminRoutingModule
  ],
  providers: [{
    provide: RouteReuseStrategy,
    useClass: CustomRouteReuseStrategy
  }]
})
export class AdminModule { }
