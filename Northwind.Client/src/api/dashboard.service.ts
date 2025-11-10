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

export interface ReportPageItem {
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
    operator: "Eq" | "Ne" | "Gt" | "Lt" | "Gte" | "Lte";
    leftSource: "Parameter" | "Const" | "None";
    leftField?: string;
    rightSource: "Parameter" | "Const" | "None";
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

export interface ReportPage {
  key: string;
  name?: { language: string; text: string }[];
  enabled: boolean;
  layout: LayoutDefinition;
  reportPageItems: ReportPageItem[];
}

// ---------- ROOT DASHBOARD ----------

export interface DashboardDefinition {
  version: number;
  companyId: number;
  type: "Dashboard" | string;
  parameters: ParameterDefinition[];
  pages: ReportPage[];
}

export const dashboardService = {
  get: async () => {
    const res = await api.get("/dashboard/render");
    return res.data;
  },
};
