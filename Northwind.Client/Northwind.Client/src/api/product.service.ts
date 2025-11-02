import api, { type PagedResult } from "././axios.client";

export interface Product {
  id: number;
  productName: string;
  unitPrice?: number;
  unitsInStock?: number;
  categoryName: string;
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

/**
 * Exempel på användning av productService:
 * 
 * const products = await productService.getAll();
 * const single = await productService.getById(1);
 * await productService.create({ productName: "New Product", unitPrice: 19.99 });
 * await productService.update(1, { unitPrice: 25 });
 * await productService.delete(2);
 */
export default productService;