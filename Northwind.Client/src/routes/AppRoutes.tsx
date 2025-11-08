import HomePage from "../pages/HomePage";
import AppLayout from "../components/layout/AppLayout";
import ProductsPage from "../pages/products/ProductsPage";
import SuppliersPage from "../pages/suppliers/SuppliersPage";
import CategoriesPage from "../pages/categories/CategoriesPage";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import LoginPage from "../pages/auth/LoginPage";
import { RequireAuth } from "../components/auth/RequireAuth";
import { RequireRole } from "../components/auth/RequireRole";
import AddUserPage from "../pages/admin/AddUserPage";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <AppLayout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route
            path="/suppliers"
            element={
              <RequireAuth>
                <RequireRole roles={["Admin", "Supplier"]}>
                  <SuppliersPage />
                </RequireRole>
              </RequireAuth>
            }
          />

          <Route
            path="/products"
            element={
              <RequireAuth>
                <RequireRole roles={["Admin"]}>
                  <ProductsPage />
                </RequireRole>
              </RequireAuth>
            }
          />
          <Route
            path="/admin/add-user"
            element={
              <RequireAuth>
                <RequireRole roles={["Admin"]}>
                  <AddUserPage />
                </RequireRole>
              </RequireAuth>
            }
          />
          <Route path="/categories" element={
              <RequireAuth>
                <RequireRole roles={["Admin"]}>
                  <CategoriesPage />
                </RequireRole>
              </RequireAuth>
            } />
        </Routes>
      </AppLayout>
    </BrowserRouter>
  );
}
