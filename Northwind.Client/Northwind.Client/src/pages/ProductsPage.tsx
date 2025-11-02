import { useState } from "react";
import Loader from "../components/common/loader";
import { productService, type Product } from "../api/product.service";
import { useQuery } from "@tanstack/react-query";
import Pagination from "../components/common/pagination";
import Table from "../components/common/table";

export default function ProductsPage() {
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const { data, isLoading, isError } = useQuery({
    queryKey: ["products", page],
    queryFn: () => productService.getAll(page, pageSize),
  });

  const handleAdd = () => alert("Lägg till ny produkt");
  const handleEdit = (p: Product) => alert(`Redigera ${p.productName}`);
  const handleDelete = (p: Product) => alert(`Radera ${p.productName}`);

  if (isLoading) return <Loader />;

  if (isError) return <div>Det gick inte att hämta produkter.</div>;
  const products = data?.items || [];
  return (
    <div className="p-8">
        <Table<Product>
          title="Produkter"
          data={products}
          columns={[
            { key: "productId", label: "ID", width: "40px" },
            { key: "productName", label: "Produktnamn", width: "25%" },
            { key: "categoryName", label: "Kategori", width: "25%" },
            { key: "unitPrice", label: "Pris", width: "25%" },
            { key: "unitsInStock", label: "Lager", width: "25%" },
          ]}
          onAdd={handleAdd}
          onEdit={handleEdit}
          onDelete={handleDelete}
      />

      <Pagination
        page={data?.page ?? 1}
        totalPages={data?.totalPages ?? 1}
        onPageChange={setPage}
      />
    </div>
  );
}
