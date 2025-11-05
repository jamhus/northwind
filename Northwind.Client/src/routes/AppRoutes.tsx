import HomePage from "../pages/HomePage";
import AppLayout from "../components/layout/AppLayout";
import ProductsPage from "../pages/products/ProductsPage";
import SuppliersPage from "../pages/suppliers/SuppliersPage";
import CategoriesPage from "../pages/categories/CategoriesPage";
import { BrowserRouter, Routes, Route } from "react-router-dom";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <AppLayout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/suppliers" element={<SuppliersPage />} />
        </Routes>
      </AppLayout>
    </BrowserRouter>
  );
}
