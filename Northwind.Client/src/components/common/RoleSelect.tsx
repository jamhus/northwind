import { Listbox, ListboxButton, ListboxOption, ListboxOptions } from "@headlessui/react";
import { ChevronDown, Check } from "lucide-react";

const roles = ["Employee", "Supplier", "Manager", "Admin"];

export default function RoleSelect({ value, onChange }: { value: string; onChange: (v: string) => void }) {
  return (
    <Listbox value={value} onChange={onChange}>
      {({ open }) => (
        <div className="relative">
          <ListboxButton className="flex justify-between items-center w-full border rounded p-2 bg-white hover:border-gray-400">
            <span>{value || "Välj roll..."}</span>
            <ChevronDown size={16} className="text-gray-500" />
          </ListboxButton>
          {open && (
            <ListboxOptions className="absolute mt-1 w-full bg-white border rounded shadow-lg z-10">
              {roles.map((role) => (
                <ListboxOption
                  key={role}
                  value={role}
                  className={({ active }) =>
                    `cursor-pointer px-3 py-2 ${active ? "bg-blue-100" : ""}`
                  }
                >
                  {({ selected }) => (
                    <div className="flex justify-between items-center">
                      <span>{role}</span>
                      {selected && <Check size={16} className="text-blue-600" />}
                    </div>
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
