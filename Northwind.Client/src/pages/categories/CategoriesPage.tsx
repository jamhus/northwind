import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import Table from "../../components/common/Table";
import { categoryIcons } from "../../assets/icons";
import Loader from "../../components/common/Loader";
import { notify } from "../../components/common/Notify";
import Pagination from "../../components/common/Pagination";
import { categoryService, type Category } from "../../api/category.service";
import UpsertCategoryModal from "./modals/UpsertCategoryModal";
import DeleteCategoryModal from "./modals/DeleteCategoryModal";

export default function CategoriesPage() {
  const [selectedCategory, setSelectedCategory] = useState<Category | null>(
    null
  );
  const [editOpen, setEditOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ["categories", page],
    queryFn: () => categoryService.getAll(page, pageSize),
  });

  const handleAdd = () => {
    setSelectedCategory(null);
    setEditOpen(true);
  };

  const handleEdit = (c: Category) => {
    setSelectedCategory(c);
    setEditOpen(true);
  };
  const handleDelete = (c: Category) => {
    setSelectedCategory(c);
    setDeleteOpen(true);
  };

  const handleSave = async (updated: Category) => {
    console.log("Saving category:", updated);
    try {
      if (updated.categoryId) {
        await categoryService.update(updated);
        notify("Kategorin uppdaterades!", "success");
      } else {
        await categoryService.create(updated);
        notify("Ny kategori skapades!", "success");
      }
      await refetch();
    } catch {
      notify("Kunde inte spara Kategorin", "error");
    }
  };

  const handleConfirmDelete = async (c: Category) => {
    try {
      await categoryService.delete(c.categoryId);
      notify(`"${c.categoryName}" raderades`, "success");
      await refetch();
    } catch {
      notify("Kunde inte radera kategorin", "error");
    }
  };

  if (isLoading) return <Loader />;

  if (isError) return <div>Det gick inte att hämta kategorier.</div>;

  const categories = data?.items || [];
  return (
    <div className="p-8">
      <Table<Category>
        title="Kategorier"
        data={categories}
        columns={[
          { key: "categoryId", label: "ID", width: "40px" },
          {
            key: "categoryName",
            label: "Kategori",
            render: (c) => (
              <div className="flex items-center gap-2">
                {categoryIcons[c.categoryName] && (
                  <>{categoryIcons[c.categoryName]}</>
                )}
                <span>{c.categoryName}</span>
              </div>
            ),
          },
          { key: "description", label: "Beskrivning" },
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
        <UpsertCategoryModal
          category={selectedCategory}
          isOpen={editOpen}
          onClose={() => setEditOpen(false)}
          onSave={handleSave}
        />
      )}

      {deleteOpen && (
        <DeleteCategoryModal
          category={selectedCategory}
          isOpen={deleteOpen}
          onClose={() => setDeleteOpen(false)}
          onConfirm={handleConfirmDelete}
        />
      )}
    </div>
  );
}
