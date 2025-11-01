import { useEffect, useState } from "react";
import Loader from "../components/common/loader";
import { productService, type Product } from "../api/product.service";

export default function ProductsPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    productService.getAll()
      .then(setProducts)
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <Loader />;

  return (
    <div className="p-8">
      <h2 className="text-2xl font-semibold mb-4">Produkter ({products.length})</h2>
      <table className="w-full border-collapse border border-gray-300">
        <thead>
          <tr className="bg-gray-100 text-left">
            <th className="border p-2">ID</th>
            <th className="border p-2">Namn</th>
            <th className="border p-2">Pris</th>
            <th className="border p-2">Lager</th>
          </tr>
        </thead>
        <tbody>
          {products.map(p => (
            <tr key={p.productId} className="hover:bg-gray-50">
              <td className="border p-2">{p.productId}</td>
              <td className="border p-2">{p.productName}</td>
              <td className="border p-2">{p.unitPrice ?? "-"}</td>
              <td className="border p-2">{p.unitsInStock ?? "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
