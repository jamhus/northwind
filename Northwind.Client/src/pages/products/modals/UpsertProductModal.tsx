import ModalWrapper from "./ModalWrapper";
import { useEffect, useState } from "react";
import type { Product } from "../../../api/product.service";
import CategorySelect from "../../../components/common/CategorySelect";
import SupplierSelect from "../../../components/common/SupplierSelect";

type Props = {
  product: Product | null;
  isOpen: boolean;
  onClose: () => void;
  onSave: (updated: Product) => void;
};

export default function UpsertProductModal({
  product,
  isOpen,
  onClose,
  onSave,
}: Props) {
  const empty: Product = {
    id: 0,
    productName: "",
    unitPrice: 0,
    unitsInStock: 0,
    categoryId: 0,
    categoryName: "",
    supplierId: 0,
    supplierName: "",
  };

  const [form, setForm] = useState<Product>(empty);

  useEffect(() => {
    if (product && isOpen) setForm(product);
  }, [product, isOpen]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]:
        name === "unitPrice"
          ? parseFloat(value)
          : name === "unitsInStock"
          ? parseInt(value)
          : value,
    }));
  };

  const handleSubmit = () => {
    onSave(form);
    onClose();
  };

  return (
    <ModalWrapper isOpen={isOpen} onClose={onClose} title="Redigera produkt">
      <div className="flex flex-col gap-3">
        <label>
          <span className="text-sm text-gray-600">Produktnamn</span>
          <input
            name="productName"
            value={form.productName}
            onChange={handleChange}
            className="w-full border p-2 rounded"
          />
        </label>

        <label>
          <span className="text-sm text-gray-600">Pris</span>
          <input
            type="number"
            name="unitPrice"
            value={form.unitPrice ?? ""}
            onChange={handleChange}
            className="w-full border p-2 rounded"
          />
        </label>

        <label>
          <span className="text-sm text-gray-600">Lager</span>
          <input
            type="number"
            name="unitsInStock"
            value={form.unitsInStock ?? ""}
            onChange={handleChange}
            className="w-full border p-2 rounded"
          />
        </label>

        <label>
          <span className="text-sm text-gray-600">Kategori</span>
          <CategorySelect
            value={
              form.categoryId
            }
            onChange={(cat) =>
              setForm((prev) => ({
                ...prev,
                categoryId: cat.categoryId,
              }))
            }
          />
        </label>

         <label>
          <span className="text-sm text-gray-600">Leverantör</span>
          <SupplierSelect
            value={
              form.supplierId
            }
            onChange={(supplier) =>
              setForm((prev) => ({  
                ...prev,
                supplierId: supplier.supplierId,
              }))
            }
          />
        </label>

        <div className="flex justify-end gap-3 mt-4">
          <button onClick={onClose} className="px-4 py-2 bg-gray-200 rounded">
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
