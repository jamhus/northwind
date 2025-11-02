import api from "././axios.client";

export type Category = {
  categoryId: number;
  categoryName: string;
};

export const categoryService = {
  getAll: async (): Promise<Category[]> => {
    const res = await api.get<Category[]>("/categories");
    return res.data;
  },
};
