import type { Product } from "../../../api/product.service";
import Modal from "../../../components/common/Modal";

type Props = {
  product: Product | null;
  isOpen: boolean;
  onClose: () => void;
  onConfirm: (product: Product) => void;
};

export default function DeleteProductModal({ product, isOpen, onClose, onConfirm }: Props) {
  if (!product) return null;

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Radera produkt">
      <p className="text-gray-700">
        Är du säker på att du vill radera <strong>{product.productName}</strong>?
      </p>

      <div className="flex justify-end gap-3 mt-6">
        <button onClick={onClose} className="px-4 py-2 bg-gray-200 rounded">
          Avbryt
        </button>
        <button
          onClick={() => {
            onConfirm(product);
            onClose();
          }}
          className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
        >
          Ja, radera
        </button>
      </div>
    </Modal>
  );
}
