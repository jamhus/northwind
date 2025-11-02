import { Listbox, ListboxButton, ListboxOptions, ListboxOption } from "@headlessui/react";
import { Check, ChevronDown } from "lucide-react";
import type { Category } from "../../api/category.service";
import { categoryIcons } from "../../assets/icons";

type Props = {
  value: Category | undefined;
  onChange: (category: Category) => void;
  categories: Category[];
};

export default function CategorySelect({ value, onChange, categories }: Props) {
  return (
    <Listbox value={value} onChange={onChange}>
      {({ open }) => (
        <div className="relative">
          {/* Selected button */}
          <ListboxButton className="flex justify-between items-center w-full border rounded p-2 bg-white hover:border-gray-400">
            <div className="flex items-center gap-2">
              {value && (categoryIcons[value.categoryName] ?? categoryIcons.default)}
              <span>{value?.categoryName ?? "Välj kategori..."}</span>
            </div>
            <ChevronDown size={16} className="text-gray-500" />
          </ListboxButton>

          {/* Options */}
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
