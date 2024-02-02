export declare class MenuItem {
  id: string;
  name: string;
  route?: string;
  description?: string;
  isMainMenu: boolean;
  isDisabled?: boolean;
  icon?: string;
  svgIcon?: string;
  badgeValue?: number | string;
  badgePath?: string;
  canRead?: boolean;
  canWrite?: boolean;
  canApprove?: boolean;
  subMenu?: MenuItem[];
  expanded?: boolean;
  isSubMenuActive?: boolean;
}
