import { useEffect, useState } from "react";
import type { Supplier } from "../../../api/supplier.service";
import ModalWrapper from "../../products/modals/ModalWrapper";

type Props = {
  supplier: Supplier | null;
  isOpen: boolean;
  onClose: () => void;
  onSave: (updated: Supplier) => void;
};

export default function UpsertSupplierModal({
  supplier,
  isOpen,
  onClose,
  onSave,
}: Props) {
  const empty: Supplier = {
    supplierId: 0,
    companyName: "",
    contactName: "",
    contactTitle: "",
  };

  const [form, setForm] = useState<Supplier>(empty);

  useEffect(() => {
    if (supplier && isOpen) setForm(supplier);
    else setForm(empty);
  }, [supplier, isOpen]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = () => {
    onSave(form);
    setForm(empty);
    onClose();
  };

  return (
    <ModalWrapper
      isOpen={isOpen}
      onClose={onClose}
      title={supplier ? "Redigera leverantör" : "Lägg till leverantör"}
    >
      <div className="flex flex-col gap-3">
        <label>
          <span className="text-sm text-gray-600">Företagsnamn</span>
          <input
            name="companyName"
            value={form.companyName}
            onChange={handleChange}
            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Ange företagsnamn..."
          />
        </label>

        <label>
          <span className="text-sm text-gray-600">Kontaktperson</span>
          <input
            name="contactName"
            value={form.contactName}
            onChange={handleChange}
            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Ange kontaktperson..."
          />
        </label>

        <label>
          <span className="text-sm text-gray-600">Titel</span>
          <input
            name="contactTitle"
            value={form.contactTitle}
            onChange={handleChange}
            className="w-full border p-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Ange titel..."
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
