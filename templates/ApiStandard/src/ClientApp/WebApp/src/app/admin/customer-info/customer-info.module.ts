import { NgModule } from '@angular/core';
import { CustomerInfoRoutingModule } from './customer-info-routing.module';
import { ShareModule } from 'src/app/share/share.module';
import { ComponentsModule } from 'src/app/components/components.module';
import { IndexComponent } from './index/index.component';
import { DetailComponent } from './detail/detail.component';
import { AddComponent } from './add/add.component';
import { EditComponent } from './edit/edit.component';
import { EnumTextPipeModule } from 'src/app/pipe/admin/enum-text.pipe';

@NgModule({
  declarations: [IndexComponent, DetailComponent, AddComponent, EditComponent],
  imports: [
    ComponentsModule,
    ShareModule,
    CustomerInfoRoutingModule,
    EnumTextPipeModule
  ]
})
export class CustomerInfoModule { }
