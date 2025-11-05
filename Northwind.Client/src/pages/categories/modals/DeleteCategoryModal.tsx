import type { Category } from "../../../api/category.service";
import ModalWrapper from "../../products/modals/ModalWrapper";

type Props = {
  category: Category | null;
  isOpen: boolean;
  onClose: () => void;
  onConfirm: (c: Category) => void;
};

export default function DeleteCategoryModal({ category, isOpen, onClose, onConfirm }: Props) {
  if (!category) return null;

  return (
    <ModalWrapper
      isOpen={isOpen}
      onClose={onClose}
    >
      <h2 className="text-lg font-semibold mb-3 text-red-600">Radera kategori</h2>
      <p className="text-gray-700">
        Är du säker på att du vill radera <strong>{category.categoryName}</strong>?
      </p>

      <div className="flex justify-end gap-3 mt-6">
        <button onClick={onClose} className="px-4 py-2 bg-gray-200 rounded">
          Avbryt
        </button>
        <button
          onClick={() => {
            onConfirm(category);
            onClose();
          }}
          className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
        >
          Ja, radera
        </button>
      </div>
    </ModalWrapper>
  );
}
