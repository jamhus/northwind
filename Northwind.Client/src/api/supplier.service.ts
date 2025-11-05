import api, { type PagedResult } from "././axios.client";

export type Supplier = {
  supplierId: number;
  companyName: string;
  contactName: string;
  contactTitle: string;
};

export const supplierService = {
  getAll: async (page: number, pageSize: number): Promise<PagedResult<Supplier>> => {
    const res = await api.get<PagedResult<Supplier>>("/suppliers", {
      params: { page, pageSize },
    });
    return res.data;
  },
  getAllForSelect: async (): Promise<Supplier[]> => {
    const res = await api.get<Supplier[]>("/suppliers/for-select");
    return res.data;
  },
  create: async (supplier: Supplier): Promise<void> => {
    await api.post("/suppliers", supplier);
  },
  update: async (supplier: Supplier): Promise<void> => {
    await api.put(`/suppliers/${supplier.supplierId}`, supplier);
  },
  delete: async (supplierId: number): Promise<void> => {
    await api.delete(`/suppliers/${supplierId}`);
  },
};
