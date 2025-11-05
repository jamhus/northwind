import { BrowserRouter, Routes, Route } from "react-router-dom";
import HomePage from "../pages/HomePage";
import ProductsPage from "../pages/products/ProductsPage";
import Navbar from "../components/layout/Navbar";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <div className="app flex flex-col min-h-screen justify-between">
        <div>
          <Navbar />
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/products" element={<ProductsPage />} />
          </Routes>
        </div>
      </div>
    </BrowserRouter>
  );
}
