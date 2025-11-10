import { Routes, Route, Navigate } from "react-router-dom";
import { useAuth } from "./../contexts/auth/useAuth";
import ProductsPage from "./../pages/products/ProductsPage";
import CategoriesPage from "./../pages/categories/CategoriesPage";
import SuppliersPage from "./../pages/suppliers/SuppliersPage";
import OrdersDashboardPage from "./../pages/dashboard/OrdersDashboardPage";
import EmployeeDashboardPage from "./../pages/dashboard/EmployeeDashboardPage";
import LoginPage from "./../pages/auth/LoginPage";
import AddUserPage from "./../pages/admin/AddUserPage";
import ProtectedRoute from "../components/auth/ProtectedRoutes";
import AppLayout from "../components/layout/AppLayout";
import OrdersPage from "../pages/orders/OrdersPage";
import DashboardConfigPage from "../pages/admin/DashboardConfigPage";
import DashboardPage from "../pages/dashboard/DashboardPage";

export default function AppRoutes() {
  const { user, isAuthenticated } = useAuth();

  // Standard dashboard
  const dashboard = user?.roles.includes("Employee") ? (
    <EmployeeDashboardPage />
  ) : (
    <OrdersDashboardPage />
  );

  return (
    <AppLayout>
      <Routes>
        {/* Login */}
        <Route path="/login" element={<LoginPage />} />

        {/* Dashboard - kräver inloggning */}
        <Route
          path="/"
          element={
            <ProtectedRoute roles={["Admin", "Employee", "Supplier"]}>
              {dashboard}
            </ProtectedRoute>
          }
        />
        <Route
          path="/dashboardV2"
          element={
            <ProtectedRoute roles={["Admin"]}>
              <DashboardPage />
            </ProtectedRoute>
          }
        />

        {/* Admin routes */}
        <Route
          path="/admin/add-user"
          element={
            <ProtectedRoute roles={["Admin"]}>
              <AddUserPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/dashboard-config"
          element={
            <ProtectedRoute roles={["Admin"]}>
              <DashboardConfigPage />
            </ProtectedRoute>
          }
        />

        {/* Products/Categories/Suppliers */}
        <Route
          path="/products"
          element={
            <ProtectedRoute roles={["Admin", "Supplier"]}>
              <ProductsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/categories"
          element={
            <ProtectedRoute roles={["Admin"]}>
              <CategoriesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/suppliers"
          element={
            <ProtectedRoute roles={["Admin", "Supplier"]}>
              <SuppliersPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/orders"
          element={
            <ProtectedRoute roles={["Admin", "Employee", "Supplier"]}>
              <OrdersPage />
            </ProtectedRoute>
          }
        />
        {/* Fallback */}
        <Route
          path="*"
          element={
            isAuthenticated ? (
              <Navigate to="/" replace />
            ) : (
              <Navigate to="/login" replace />
            )
          }
        />
      </Routes>
    </AppLayout>
  );
}
