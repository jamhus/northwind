import { useState } from "react";
import { notify } from "../../components/common/Notify";
import api from "../../api/axios.client";
import RoleSelect from "../../components/common/RoleSelect";
import SupplierSelect from "../../components/common/SupplierSelect";

type Form = {
  email: string;
  password: string;
  role: string;
  supplierId?: number;
};

export default function AddUserPage() {
  const [form, setForm] = useState<Form>({
    email: "",
    password: "",
    role: "Employee",
  });

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      await api.post("/auth/register", form);
      notify(`Användare ${form.email} skapades!`, "success");
      setForm({ email: "", password: "", role: "Employee" });
    } catch {
      notify("Misslyckades att skapa användare", "error");
    }
  }

  function handleChange(
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  return (
    <div className="max-w-[600px] mx-auto p-8">
      <h1 className="text-2xl font-semibold mb-6">Skapa ny användare</h1>

      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <label>
          <span className="text-sm text-gray-600">E-post</span>
          <input
            name="email"
            value={form.email}
            onChange={handleChange}
            className="w-full border p-2 rounded"
            required
          />
        </label>

        <label>
          <span className="text-sm text-gray-600">Lösenord</span>
          <input
            type="password"
            name="password"
            value={form.password}
            onChange={handleChange}
            className="w-full border p-2 rounded"
            required
          />
        </label>

        <label>
          <span className="text-sm text-gray-600">Roll</span>
          <RoleSelect
            value={form.role}
            onChange={(role) => setForm((f) => ({ ...f, role }))}
          />
        </label>

        {form.role === "Supplier" && (
          <label>
            <span className="text-sm text-gray-600">Välj leverantör</span>
            <SupplierSelect
              value={form.supplierId ?? 0}
              onChange={(supplier) =>
                setForm((prev) => ({
                  ...prev,
                  supplierId: supplier.supplierId,
                }))
              }
            />
          </label>
        )}

        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Skapa användare
        </button>
      </form>
    </div>
  );
}
