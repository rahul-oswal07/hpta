import { FilterType } from 'src/app/core/models'
export class Indent {
  id!: string

  name?: string

  dateCreated?: string

  status?: number

  startDate?: Date

  endDate?: Date
  vehicleMaps?: VehicleMap[];
  indentRule?: IndentRule;
  selectedTransporters: IndentTransporter[] = [];
  myQuotations: TransporterQuotationDetail[] = [];
}

export class TransporterQuotationDetail {
  vehicleMapMaterialId!: string;
  amount!: number;
  status!: number;
  createdOn!: string;
  quotationId!: string;
}

export class VehicleMap {
  id!: string;
  vehicleTypeId?: string;
  vehicleTypeName?: string;
  points: VehicleMapPoint[] = [];
}

export class VehicleMapPoint {
  id!: string;
  location?: string;
  serviceLocationId?: string;
  dateAndTime?: Date;
  materials: VehicleMapMaterial[] = [];
}

export class VehicleMapMaterial {
  id!: string;
  materialType?: string;
  packingType?: string;
  load?: number;
  typeOfActivity?: number;
}

export class IndentTransporter {
  transporterId!: string;
  transporterName?: string;
  indentId?: string;
  isDifferentRoute!: boolean;
}

export class IndentRule {
  indentRequestType?: number;
  isEntireDay?: boolean;
  activeStartTime?: string;
  activeEndTime?: string;
  enableSpotBidding?: boolean;
  spotBiddingtype?: number;
  biddingType?: number;
  costCappingEnabled?: boolean;
  maxCostCap?: number;
  startPriceEnabled?: boolean;
  maxStartPrice?: number;
  indentQuotingTime?: TimeValueUnit;
  indentApprovalTime?: TimeValueUnit;
  indentCoolingPeriod?: TimeValueUnit;
}

export class TimeValueUnit {
  timeValue?: number;
  timeUnit?: number;
}

export const indentFields = [

  { id: 'name', label: 'Title', dataType: FilterType.String },

  { id: 'dateCreated', label: 'Created On', dataType: FilterType.String },

  { id: 'startDate', label: 'Start Date', dataType: FilterType.Date },

  { id: 'endDate', label: 'End Date', dataType: FilterType.Date },
  { id: 'status', label: 'Status', dataType: FilterType.String }

]

export const activityTypes = [
  { id: 0, value: 'Load' },
  { id: 1, value: 'Unload' },
  { id: 2, value: 'Transfer' }
]

export const timeUnits = [
  { id: 0, value: 'Minutes' },
  { id: 1, value: 'Hours' }
]

export const spotBiddingTypes = [
  { id: 0, value: 'Before Indent' },
  { id: 1, value: 'During Indent' },
  { id: 2, value: 'After Indent' }
]

export const biddingTypes = [
  { id: 0, value: 'Forward' },
  { id: 1, value: 'Reverse' }
]
