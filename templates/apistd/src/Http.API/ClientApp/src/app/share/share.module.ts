import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EnumPipe, MapEnumPipe } from './pipe/enum.pipe';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ToKeyValuePipe } from './pipe/to-key-value.pipe';
import { ComponentsModule } from '../components/components.module';

@NgModule({
  declarations: [EnumPipe, ToKeyValuePipe, MapEnumPipe],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    ComponentsModule
  ],
  exports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    ComponentsModule,
    EnumPipe,
    MapEnumPipe,
    ToKeyValuePipe
  ]
})
export class ShareModule { }
