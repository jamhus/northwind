import api, { type PagedResult } from "././axios.client";

export type Category = {
  categoryId: number;
  categoryName: string;
  description: string;
};

export const categoryService = {
  async getAll(page: number, pageSize: number): Promise<PagedResult<Category>> {
    const response = await  api.get<PagedResult<Category>>("/categories", {
      params: { page, pageSize },
    });
    return response.data;
  },
  getAllForSelect: async (): Promise<Category[]> => {
    const res = await api.get<Category[]>("/categories/for-select");
    return res.data;
  },
  create: async (category: Category): Promise<void> => {
    await api.post("/categories", category);
  },
  update: async (category: Category): Promise<void> => {
    await api.put(`/categories/${category.categoryId}`, category);
  },
  delete: async (id: number): Promise<void> => {
    await api.delete(`/categories/${id}`);
  },
};
