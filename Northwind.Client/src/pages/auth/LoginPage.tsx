import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/auth/useAuth";
import { notify } from "../../components/common/Notify";

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    try {
      await login(email, password);
      notify("Inloggad!", "success");
      navigate("/");
    } catch {
      notify("Felaktig e-post eller lösenord", "error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="flex justify-center items-center h-[80vh]">
      <form
        onSubmit={handleSubmit}
        className="bg-white shadow-md rounded p-8 w-[400px] flex flex-col gap-4"
      >
        <h1 className="text-2xl font-semibold mb-2 text-center text-blue-600">
          Logga in
        </h1>

        <input
          type="email"
          placeholder="E-post"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="border rounded p-2"
          required
        />
        <input
          type="password"
          placeholder="Lösenord"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="border rounded p-2"
          required
        />

        <button
          type="submit"
          disabled={loading}
          className="bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-60"
        >
          {loading ? "Loggar in..." : "Logga in"}
        </button>
      </form>
    </div>
  );
}
