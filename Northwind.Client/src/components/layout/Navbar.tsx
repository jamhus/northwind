import { useState, useEffect, useRef } from "react";
import { useAuth } from "../../contexts/auth/useAuth";
import { Link, NavLink, useNavigate } from "react-router-dom";
import { ChevronDown, LogIn, LogOut, Shield } from "lucide-react";

export default function Navbar() {
  const { user, logout, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const [openCatalog, setOpenCatalog] = useState(false);
  const [openAdmin, setOpenAdmin] = useState(false);

  const catalogRef = useRef<HTMLDivElement>(null);
  const adminRef = useRef<HTMLDivElement>(null);

  // Stänger dropdowns när man klickar utanför
  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (
        catalogRef.current &&
        !catalogRef.current.contains(e.target as Node)
      ) {
        setOpenCatalog(false);
      }
      if (adminRef.current && !adminRef.current.contains(e.target as Node)) {
        setOpenAdmin(false);
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <nav className="bg-white border border-gray-200 rounded-lg shadow-sm">
      <div className="max-w-[1100px] mx-auto flex justify-between items-center px-6 py-3">
        {/* Logo */}
        <Link to="/" className="font-semibold text-lg tracking-wide">
          Northwind Portal
        </Link>

        <div className="flex items-center gap-6">
          {/* ===== KATALOG MENY ===== */}
          <div className="relative" ref={catalogRef}>
            <button
              onClick={() => setOpenCatalog(!openCatalog)}
              className="flex items-center gap-1 hover:text-blue-200"
            >
              Katalog
              <ChevronDown size={16} />
            </button>

            {openCatalog && (
              <div className="absolute right-0 mt-2 bg-white text-gray-700 rounded shadow-lg w-48 z-50">
                <NavLink
                  to="/products"
                  className={({ isActive }) =>
                    `block px-4 py-2 hover:bg-blue-100 ${
                      isActive ? "font-medium text-blue-700" : ""
                    }`
                  }
                  onClick={() => setOpenCatalog(false)}
                >
                  Produkter
                </NavLink>
                <NavLink
                  to="/categories"
                  className={({ isActive }) =>
                    `block px-4 py-2 hover:bg-blue-100 ${
                      isActive ? "font-medium text-blue-700" : ""
                    }`
                  }
                  onClick={() => setOpenCatalog(false)}
                >
                  Kategorier
                </NavLink>
                <NavLink
                  to="/suppliers"
                  className={({ isActive }) =>
                    `block px-4 py-2 hover:bg-blue-100 ${
                      isActive ? "font-medium text-blue-700" : ""
                    }`
                  }
                  onClick={() => setOpenCatalog(false)}
                >
                  Leverantörer
                </NavLink>
              </div>
            )}
          </div>

          {/* ===== ADMIN TOOLS ===== */}
          {user?.roles.includes("Admin") && (
            <div className="relative" ref={adminRef}>
              <button
                onClick={() => setOpenAdmin(!openAdmin)}
                className="flex items-center gap-1 hover:text-blue-200"
              >
                <Shield size={16} /> Admin Tools
                <ChevronDown size={16} />
              </button>

              {openAdmin && (
                <div className="absolute right-0 mt-2 bg-white text-gray-700 rounded shadow-lg w-52 z-50">
                  <NavLink
                    to="/admin/add-user"
                    className={({ isActive }) =>
                      `block px-4 py-2 hover:bg-blue-100 ${
                        isActive ? "font-medium text-blue-700" : ""
                      }`
                    }
                    onClick={() => setOpenAdmin(false)}
                  >
                    Lägg till användare
                  </NavLink>
                  <NavLink
                    to="/admin/reports"
                    className={({ isActive }) =>
                      `block px-4 py-2 hover:bg-blue-100 ${
                        isActive ? "font-medium text-blue-700" : ""
                      }`
                    }
                    onClick={() => setOpenAdmin(false)}
                  >
                    Rapporter (kommande)
                  </NavLink>
                </div>
              )}
            </div>
          )}

          {/* ===== AUTH-KNAPPAR ===== */}
          {!isAuthenticated ? (
            <NavLink
              to="/login"
              className="flex items-center gap-1 hover:text-blue-200"
            >
              <LogIn size={16} /> Logga in
            </NavLink>
          ) : (
            <button
              onClick={handleLogout}
              className="flex items-center gap-1 hover:text-blue-200"
            >
              <LogOut size={16} /> Logga ut
            </button>
          )}
        </div>
      </div>
    </nav>
  );
}
