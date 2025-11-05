import { useEffect, useState } from "react";
import type { Category } from "../../../api/category.service";
import ModalWrapper from "../../products/modals/ModalWrapper";

type Props = {
  category: Category | null;
  isOpen: boolean;
  onClose: () => void;
  onSave: (updated: Category) => void;
};

export default function UpsertCategoryModal({
  category,
  isOpen,
  onClose,
  onSave,
}: Props) {
  const empty: Category = {
    categoryId: 0,
    categoryName: "",
    description: "",
  };

  const [form, setForm] = useState<Category>(empty);

  useEffect(() => {
    if (category && isOpen) setForm(category);
    else setForm(empty);
  }, [category, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = () => {
    if (!form.categoryName.trim()) return;
    onSave(form);
    setForm(empty);
    onClose();
  };

  return (
    <ModalWrapper
      isOpen={isOpen}
      onClose={onClose}
      title={category ? "Redigera kategori" : "Lägg till kategori"}
    >
      <div className="flex flex-col gap-4">
        <label>
          <span className="text-sm text-gray-600">Kategorinamn</span>
          <input
            name="categoryName"
            value={form.categoryName}
            onChange={handleChange}
            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Ange namn..."
          />
        </label>
        <label>
          <span className="text-sm text-gray-600">Beskrivning</span>
          <textarea
            name="description"
            value={form.description ?? ""}
            onChange={handleChange}
            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Ange beskrivning..."
            rows={2}
          />
        </label>

        <div className="flex justify-end gap-3 mt-4">
          <button
            onClick={onClose}
            className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
          >
            Avbryt
          </button>
          <button
            onClick={handleSubmit}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Spara
          </button>
        </div>
      </div>
    </ModalWrapper>
  );
}
