/* eslint-disable @typescript-eslint/no-explicit-any */

import api from "./axios.client";
// ---------- PARAMETRAR ----------

export type ParameterInitialization =
  | "Explicit"
  | "Expression"
  | "Handler";

export interface ParameterDefinition {
  key: string;
  initialization: ParameterInitialization;
  entityType?: string;
  expression?: string;
  description?: string;
  expiration?: string;
  value?: any;
}

// ---------- REPORT PAGE ----------

export interface PageItem {
  key: string;
  enabled: boolean;
  order: number;
  type: string;
  condition?: Condition;
  settings?: Record<string, any>;
  data?: any; // fylld av backend (handlers)
}

export interface Condition {
  logic: "And" | "Or";
  conditions: {
    operator: "Eq" | "Neq" | "Gt" | "Lt" | "Gte" | "Lte" | "Contains" | "NotContains" | "In" | "Nin";
    leftSource: "Parameter" | "Const" | "Claim";
    leftField?: string;
    rightSource: "Parameter" | "Const" | "Claim";
    rightValue?: any;
  }[];
}

export interface LayoutColumn {
  itemRef: string;
  style?: Record<string, string>;
}

export interface LayoutRow {
  columns: LayoutColumn[];
  style?: Record<string, string>;
}

export interface LayoutDefinition {
  type: "vertical" | "horizontal";
  rows: LayoutRow[];
}

// ---------- REPORT PAGE ----------

export interface Page {
  key: string;
  name?: { language: string; text: string }[];
  enabled: boolean;
  layout: LayoutDefinition;
  pageItems: PageItem[];
}

// ---------- ROOT DASHBOARD ----------

export interface DashboardDefinition {
  version?: number;
  companyId?: number;
  type?: "Dashboard" | string;
  parameters?: ParameterDefinition[];
  pages: Page[];
}

export const dashboardService = {
  render: async () => {
    const res = await api.get("/dashboard/render");
    return res.data;
  },
};
