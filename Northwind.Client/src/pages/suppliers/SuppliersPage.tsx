import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import Table from "../../components/common/Table";
import Loader from "../../components/common/Loader";
import { notify } from "../../components/common/Notify";
import UpsertSupplierModal from "./modals/UpsertSupplierModal";
import DeleteSupplierModal from "./modals/DeleteSupplierModal";
import { supplierService, type Supplier } from "../../api/supplier.service";
import Pagination from "../../components/common/Pagination";

export default function SuppliersPage() {
  const pageSize = 10;
  const [page, setPage] = useState(1);
  const [editOpen, setEditOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);
  const [selectedSupplier, setSelectedSupplier] = useState<Supplier | null>(null);

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ["suppliers", page],
    queryFn: () => supplierService.getAll(page, pageSize),
  });

  const handleAdd = () => {
    setSelectedSupplier(null);
    setEditOpen(true);
  };

  const handleEdit = (s: Supplier) => {
    setSelectedSupplier(s);
    setEditOpen(true);
  };

  const handleDelete = (s: Supplier) => {
    setSelectedSupplier(s);
    setDeleteOpen(true);
  };

  const handleSave = async (updated: Supplier) => {
    try {
      if (updated.supplierId) {
        await supplierService.update(updated);
        notify("Leverantören uppdaterades!", "success");
      } else {
        await supplierService.create(updated);
        notify("Ny leverantör skapades!", "success");
      }
      await refetch();
    } catch {
      notify("Kunde inte spara leverantören", "error");
    }
  };

  const handleConfirmDelete = async (s: Supplier) => {
    try {
      await supplierService.delete(s.supplierId);
      notify(`"${s.companyName}" raderades`, "success");
      await refetch();
    } catch {
      notify("Kunde inte radera leverantören", "error");
    }
  };

  if (isLoading) return <Loader />;
  if (isError) return <div>Det gick inte att hämta leverantörer.</div>;

  const suppliers = data?.items || [];

  return (
    <div className="p-8">
      <Table<Supplier>
        title="Leverantörer"
        data={suppliers}
        columns={[
          { key: "supplierId", label: "ID", width: "40px" },
          { key: "companyName", label: "Företagsnamn" },
          { key: "contactName", label: "Kontakt" },
          { key: "contactTitle", label: "Titel" },
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

      {editOpen && (
        <UpsertSupplierModal
          supplier={selectedSupplier}
          isOpen={editOpen}
          onClose={() => setEditOpen(false)}
          onSave={handleSave}
        />
      )}

      {deleteOpen && (
        <DeleteSupplierModal
          supplier={selectedSupplier}
          isOpen={deleteOpen}
          onClose={() => setDeleteOpen(false)}
          onConfirm={handleConfirmDelete}
        />
      )}
    </div>
  );
}
