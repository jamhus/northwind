import {
  Listbox,
  ListboxButton,
  ListboxOption,
  ListboxOptions,
} from "@headlessui/react";
import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Check, ChevronDown } from "lucide-react";
import { categoryIcons } from "../../assets/icons";
import { categoryService, type Category } from "../../api/category.service";

type Props = {
  value: number | undefined; 
  onChange: (category: Category) => void;
  className?: string;
};

export default function CategorySelect({ value, onChange, className = "" }: Props) {
  const {
    data: categories = [],
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["categories"],
    queryFn: categoryService.getAll,
  });

  const [selected, setSelected] = useState<Category | undefined>(undefined);

  useEffect(() => {
  if (!categories || categories.length === 0) return; // inget att göra ännu

  // om value finns, hitta matchande kategori
  if (value != null) {
    const found = categories.find((c) => c.categoryId === value);
    setSelected(found ?? undefined);
  } else {
    setSelected(undefined);
  }
}, [value, categories]);


  const handleChange = (category: Category) => {
    setSelected(category);
    onChange(category);
  };

  if (isLoading)
    return (
      <div className="border rounded p-2 text-gray-500 text-sm bg-gray-50">
        Laddar kategorier...
      </div>
    );

  if (isError)
    return (
      <div className="border rounded p-2 text-red-500 text-sm bg-red-50">
        Kunde inte hämta kategorier
      </div>
    );

  return (
    <Listbox value={selected} onChange={handleChange}>
      {({ open }) => (
        <div className={`relative ${className}`}>
          <ListboxButton className="flex justify-between items-center w-full border rounded p-2 bg-white hover:border-gray-400">
            <div className="flex items-center gap-2">
              {selected ? (
                <>
                  {categoryIcons[selected.categoryName] ?? categoryIcons.default}
                  <span>{selected.categoryName}</span>
                </>
              ) : (
                <span className="text-gray-400">Välj kategori...</span>
              )}
            </div>
            <ChevronDown size={16} className="text-gray-500" />
          </ListboxButton>

          {open && (
            <ListboxOptions className="absolute mt-1 w-full bg-white shadow-lg border rounded max-h-60 overflow-auto z-50">
              {categories.map((c) => (
                <ListboxOption
                  key={c.categoryId}
                  value={c}
                  className={({ active }) =>
                    `cursor-pointer flex items-center gap-2 px-3 py-2 ${
                      active ? "bg-blue-100" : ""
                    }`
                  }
                >
                  {({ selected }) => (
                    <>
                      {categoryIcons[c.categoryName] ?? categoryIcons.default}
                      <span className="flex-1">{c.categoryName}</span>
                      {selected && <Check className="text-blue-600" size={16} />}
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
