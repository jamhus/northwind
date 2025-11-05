import { Link, useLocation } from "react-router-dom";

export default function Navbar() {
  const location = useLocation();

  const isActive = (path: string) =>
    location.pathname === path ? "text-blue-500 font-semibold" : "text-gray-700";

  return (
    <nav className="flex justify-between items-center px-6 py-4 bg-white border border-gray-200 rounded-lg shadow-sm">
      <h1 className="text-xl font-bold text-gray-800">Northwind</h1>
      <div className="flex gap-6">
        <Link className={isActive("/")} to="/">Hem</Link>
        <Link className={isActive("/products")} to="/products">Produkter</Link>
        <Link className={isActive("/categories")} to="/categories">Kategorier</Link>
        <Link className={isActive("/suppliers")} to="/suppliers">Leverantörer</Link>
      </div>
    </nav>
  );
}
