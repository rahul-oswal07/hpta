import { FilterType } from "src/app/core/models";

export class Column {
  id!: string;
  label!: string;
  hidden?: boolean = false;
  filtered?: boolean = false;
  dataType: FilterType = FilterType.String;
}
