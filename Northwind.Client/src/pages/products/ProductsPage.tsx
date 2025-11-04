import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import Table from "../../components/common/Table";
import { categoryIcons } from "../../assets/icons";
import Loader from "../../components/common/Loader";
import Pagination from "../../components/common/Pagination";
import UpsertProductModal from "./modals/UpsertProductModal";
import DeleteProductModal from "./modals/DeleteProductModal";
import { productService, type Product } from "../../api/product.service";

export default function ProductsPage() {
  const [page, setPage] = useState(1);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);
  const [editOpen, setEditOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);
  const pageSize = 10;

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ["products", page],
    queryFn: () => productService.getAll(page, pageSize),
  });

  const handleAdd = () => {
    setSelectedProduct(null);
    setEditOpen(true);
  };

  const handleEdit = (p: Product) => {
    setSelectedProduct(p);
    setEditOpen(true);
  };
  const handleDelete = (p: Product) => {
    setSelectedProduct(p);
    setDeleteOpen(true);
  };

  const handleSave = async (updated: Product) => {
    if (updated.id) await productService.update(updated);
    else await productService.create(updated);
    await refetch();
  };

  const handleConfirmDelete = async (p: Product) => {
    await productService.delete(p.id);
    await refetch();
  };

  if (isLoading) return <Loader />;

  if (isError) return <div>Det gick inte att hämta produkter.</div>;
  const products = data?.items || [];
  return (
    <div className="p-8">
      <Table<Product>
        title="Produkter"
        data={products}
        columns={[
          { key: "id", label: "ID", width: "40px" },
          { key: "productName", label: "Produktnamn" },
          {
            key: "categoryName",
            label: "Kategori",
            
            render: (p: Product) => (
              <div className="flex items-center gap-2">
                {categoryIcons[p.categoryName] ?? categoryIcons.default}
                <span>{p.categoryName}</span>
              </div>
            ),
          },
          { key: "supplierName", label: "Leverantör" },
          { key: "quantityPerUnit", label: "Förpackning" },
          { key: "unitPrice", label: "Pris",  prefix: "kr" },
          { key: "unitsInStock", label: "Lager" },
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

      <UpsertProductModal
        product={selectedProduct}
        isOpen={editOpen}
        onClose={() => setEditOpen(false)}
        onSave={handleSave}
      />

      <DeleteProductModal
        product={selectedProduct}
        isOpen={deleteOpen}
        onClose={() => setDeleteOpen(false)}
        onConfirm={handleConfirmDelete}
      />
    </div>
  );
}
