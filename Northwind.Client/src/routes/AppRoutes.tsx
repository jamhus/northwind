import { BrowserRouter, Routes, Route } from "react-router-dom";
import HomePage from "../pages/HomePage";
import ProductsPage from "../pages/products/ProductsPage";
import AppLayout from "../components/layout/AppLayout";
import CategoriesPage from "../pages/categories/CategoriesPage";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <AppLayout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route path="/categories" element={<CategoriesPage />} />
        </Routes>
      </AppLayout>
    </BrowserRouter>
  );
}
