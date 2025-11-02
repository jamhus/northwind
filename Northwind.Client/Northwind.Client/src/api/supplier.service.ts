import api from "././axios.client";

export type Supplier = {
  supplierId: number;
  supplierName: string;
};

export const supplierService = {
  getAll: async (): Promise<Supplier[]> => {
    const res = await api.get<Supplier[]>("/suppliers");
    return res.data;
  },
};
