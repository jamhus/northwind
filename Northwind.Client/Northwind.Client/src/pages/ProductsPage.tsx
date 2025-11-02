import { useState } from "react";
import Loader from "../components/common/loader";
import { productService } from "../api/product.service";
import { useQuery } from "@tanstack/react-query";
import Pagination from "../components/common/pagination";

export default function ProductsPage() {
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const { data, isLoading, isError } = useQuery({
    queryKey: ["products", page],
    queryFn: () => productService.getAll(page, pageSize),
  });

  if (isLoading) return <Loader />;

  if (isError) return <div>Det gick inte att hämta produkter.</div>;
  const products = data?.items || [];
  return (
    <div className="p-8">
      <h2 className="text-2xl font-semibold mb-4">Produkter</h2>
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

      <Pagination
        page={data?.page ?? 1}
        totalPages={data?.totalPages ?? 1}
        onPageChange={setPage}
      />
    </div>
  );
}
