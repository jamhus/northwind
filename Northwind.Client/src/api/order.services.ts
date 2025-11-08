import api, { type PagedResult } from "./axios.client";

export type Order = {
  orderId: number;
  orderDate?: string;
  customerName: string;
  employeeName: string;
  shipCountry?: string;
  total: number;
};

export type OrderDetails = {
  orderId: number;
  orderDate?: string;
  customerName: string;
  employeeName: string;
  shipCountry?: string;
  details: {
    productName: string;
    quantity: number;
    unitPrice: number;
    discount: number;
    supplierName: string;
  }[];
};

export const orderService = {
  getAll: async (page = 1, pageSize = 10) => {
    const res = await api.get<PagedResult<Order>>(`/orders?page=${page}&pageSize=${pageSize}`);
    return res.data;
  },
  getById: async (id: number) => {
    const res = await api.get<OrderDetails>(`/orders/${id}`);
    return res.data;
  },
};
