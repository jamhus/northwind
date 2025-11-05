import { Trash2 } from "lucide-react";
import type { Supplier } from "../../../api/supplier.service";
import ModalWrapper from "../../products/modals/ModalWrapper";

type Props = {
  supplier: Supplier | null;
  isOpen: boolean;
  onClose: () => void;
  onConfirm: (supplier: Supplier) => void;
};

export default function DeleteSupplierModal({
  supplier,
  isOpen,
  onClose,
  onConfirm,
}: Props) {
  const handleConfirm = () => {
    if (supplier) onConfirm(supplier);
    onClose();
  };

  return (
    <ModalWrapper
      isOpen={isOpen}
      onClose={onClose}
      title="Ta bort leverantör"
    >
      <div className="flex flex-col gap-4">
        <div className="flex items-center gap-2">
          <Trash2 className="text-red-500" size={20} />
          <span className="font-medium text-gray-700">
            Är du säker på att du vill ta bort{" "}
            <strong>{supplier?.companyName}</strong>?
          </span>
        </div>

        <p className="text-sm text-gray-500">
          Den här åtgärden kan inte ångras.
        </p>

        <div className="flex justify-end gap-3 mt-4">
          <button
            onClick={onClose}
            className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
          >
            Avbryt
          </button>
          <button
            onClick={handleConfirm}
            className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
          >
            Radera
          </button>
        </div>
      </div>
    </ModalWrapper>
  );
}
