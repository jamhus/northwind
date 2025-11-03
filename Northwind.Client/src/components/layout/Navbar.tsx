import { Link, useLocation } from "react-router-dom";

export default function Navbar() {
  const location = useLocation();

  const isActive = (path: string) =>
    location.pathname === path ? "text-blue-500 font-semibold" : "text-gray-700";

  return (
    <nav className="flex justify-between items-center px-6 py-4 bg-gray-100 border-b">
      <h1 className="text-xl font-bold text-gray-800">Northwind</h1>
      <div className="flex gap-6">
        <Link className={isActive("/")} to="/">Hem</Link>
        <Link className={isActive("/products")} to="/products">Produkter</Link>
      </div>
    </nav>
  );
}
