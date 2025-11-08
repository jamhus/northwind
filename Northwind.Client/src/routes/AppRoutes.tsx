import HomePage from "../pages/HomePage";
import LoginPage from "../pages/auth/LoginPage";
import AddUserPage from "../pages/admin/AddUserPage";
import AppLayout from "../components/layout/AppLayout";
import ProductsPage from "../pages/products/ProductsPage";
import { RequireAuth } from "../components/auth/RequireAuth";
import SuppliersPage from "../pages/suppliers/SuppliersPage";
import { RequireRole } from "../components/auth/RequireRole";
import CategoriesPage from "../pages/categories/CategoriesPage";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import OrdersPage from "../pages/orders/OrdersPage";

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
                <RequireRole roles={["Admin"]}>
                  <SuppliersPage />
                </RequireRole>
              </RequireAuth>
            }
          />
          <Route
            path="/products"
            element={
              <RequireAuth>
                <RequireRole roles={["Admin","Supplier","Manager"]}>
                  <ProductsPage />
                </RequireRole>
              </RequireAuth>
            }
          />
           <Route
            path="/orders"
            element={
              <RequireAuth>
                <RequireRole roles={["Admin","Supplier","Manager"]}>
                  <OrdersPage />
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
