import api, { type PagedResult } from "././axios.client";
export interface Product {
  id: number;
  unitPrice?: number;
  categoryId: number;
  supplierId: number;
  productName: string;
  categoryName: string;
  companyName: string;
  unitsInStock?: number;
  quantityPerUnit?: string;
}


export const productService = {

  async getAll(page: number, pageSize: number): Promise<PagedResult<Product>> {
    const response = await  api.get<PagedResult<Product>>("/products", {
      params: { page, pageSize },
    });
    return response.data;
  },

  async getById(id: number): Promise<Product> {
    const response = await api.get<Product>(`/products/${id}`);
    return response.data;
  },

  async create(product: Omit<Product, "id">): Promise<Product> {
    const response = await api.post<Product>("/products", product);
    return response.data;
  },

  async update (product: Partial<Product>) : Promise<Product> {
    const res = await api.put<Product>(`/products/${product.id}`, product);
    return res.data;
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/products/${id}`);
  },
};

export default productService;