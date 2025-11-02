import api, { type PagedResult } from "././axios.client";

export interface Product {
  productId: number;
  productName: string;
  unitPrice?: number;
  unitsInStock?: number;
  categoryName: string;
}


export const productService = {
  /**
   * Hämtar alla produkter (GET /api/products)
   */
  async getAll(page: number, pageSize: number): Promise<PagedResult<Product>> {
    const response = await  api.get<PagedResult<Product>>("/products", {
      params: { page, pageSize },
    });
    return response.data;
  },

  /**
   * Hämtar en specifik produkt via ID (GET /api/products/{id})
   */
  async getById(id: number): Promise<Product> {
    const response = await api.get<Product>(`/products/${id}`);
    return response.data;
  },

  /**
   * Skapar en ny produkt (POST /api/products)
   */
  async create(product: Omit<Product, "productId">): Promise<Product> {
    const response = await api.post<Product>("/products", product);
    return response.data;
  },

  /**
   * Uppdaterar en befintlig produkt (PUT /api/products/{id})
   */
  async update(product: Partial<Product>): Promise<void> {
    await api.put(`/products/${product.productId}`, product);
  },

  /**
   * Tar bort en produkt (DELETE /api/products/{id})
   */
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