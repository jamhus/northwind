import {
  Listbox,
  ListboxButton,
  ListboxOption,
  ListboxOptions,
} from "@headlessui/react";
import { useQuery } from "@tanstack/react-query";
import { Building2, Check, ChevronDown } from "lucide-react";
import { supplierService, type Supplier } from "../../api/supplier.service";
import { useSyncedSelection } from "../../hooks/useSyncedSelectionHook";

type Props = {
  value: number;
  onChange: (supplier: Supplier) => void;
  className?: string;
};

export default function SupplierSelect({
  value,
  onChange,
  className = "",
}: Props) {

  const {
    data: suppliers = [],
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["suppliers"],
    queryFn: supplierService.getAllForSelect,
  });
  
 const { selected, setSelected } = useSyncedSelection<Supplier>(
    suppliers,
    value,
    "supplierId"
  );

  const handleChange = (supplier: Supplier) => {
    setSelected(supplier);
    onChange(supplier);
  };

  if (isLoading)
    return (
      <div className="border rounded p-2 text-gray-500 text-sm bg-gray-50">
        Laddar...
      </div>
    );

  if (isError)
    return (
      <div className="border rounded p-2 text-red-500 text-sm bg-red-50">
        Kunde inte hämta leverantörer
      </div>
    );

  return (
    <Listbox value={selected ?? suppliers[0]} onChange={handleChange}>
      {({ open }) => (
        <div className={`relative ${className}`}>
          {/* Dropdown-knappen */}
          <ListboxButton className="flex justify-between items-center w-full border rounded p-2 bg-white hover:border-gray-400">
            <div className="flex items-center gap-2">
              {selected ? (
                <>
                  <Building2 className="text-blue-500" size={18} />
                  <span>{selected.companyName}</span>
                </>
              ) : (
                <span className="text-gray-400">Välj leverantör...</span>
              )}
            </div>
            <ChevronDown size={16} className="text-gray-500" />
          </ListboxButton>

          {/* Alternativ */}
          {open && (
            <ListboxOptions className="absolute mt-1 w-full bg-white shadow-lg border rounded max-h-60 overflow-auto z-50">
              {suppliers.map((s) => (
                <ListboxOption
                  key={s.supplierId}
                  value={s}
                  className={({ active }) =>
                    `cursor-pointer flex items-center justify-between px-3 py-2 ${
                      active ? "bg-blue-100" : ""
                    }`
                  }
                >
                  {({ selected }) => (
                    <>
                      <div className="flex items-center gap-2">
                        <Building2 className="text-blue-500" size={16} />
                        <span>{s.companyName}</span>
                      </div>
                      {selected && (
                        <Check className="text-blue-600" size={16} />
                      )}
                    </>
                  )}
                </ListboxOption>
              ))}
            </ListboxOptions>
          )}
        </div>
      )}
    </Listbox>
  );
}
