import { Directive, Input } from "@angular/core";
import {MatCellDef, MatTable} from "@angular/material/table";

@Directive({
  selector: "[matCellDef]",
  providers: [{ provide: MatCellDef, useExisting: TypedCellDefDirective }],
})
export class TypedCellDefDirective<T> extends MatCellDef {
  @Input() matCellDefTable?: MatTable<T>;
  static ngTemplateContextGuard<T>(
    dir: TypedCellDefDirective<T>,
    ctx: unknown,
  ): ctx is { $implicit: T; index: number } {
    return true;
  }
}
