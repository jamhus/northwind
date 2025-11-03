import ModalWrapper from "./ModalWrapper";
import type { Product } from "../../../api/product.service";

type Props = {
  product: Product | null;
  isOpen: boolean;
  onClose: () => void;
  onConfirm: (p: Product) => void;
};

export default function DeleteProductModal({ product, isOpen, onClose, onConfirm }: Props) {
  if (!product) return null;

  return (
    <ModalWrapper
      isOpen={isOpen}
      onClose={onClose}
    >
      <h2 className="text-lg font-semibold mb-3 text-red-600">Radera produkt</h2>
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
    </ModalWrapper>
  );
}
