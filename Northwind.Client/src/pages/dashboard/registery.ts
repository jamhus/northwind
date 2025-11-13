
import TotalSalesCard from "./components/TotalSalesCard";
import TotalOrdersCard from "./components/TotalOrdersCard";
import LatestOrdersCard from "./components/LatestOrdersCard";
import TopProductsChart from "./components/TopProductsChart";
import SalesByMonthChart from "./components/SalesByMonthChart";
import TopEmployeesChart from "./components/TopEmployeesChart";
import TotalCustomersCard from "./components/TotalCustomersCard";
import SalesPerRegionChart from "./components/SalesByRegionChart";
import TopCustomersCard from "./components/TopCustomersCard";
import SupplierTotalSalesCard from "./components/SupplierTotalSalesCard";
import AvgOrderProcessingTimeCard from "./components/AvgOrderProcessingTimeCard";
import ProductLifecycleChart from "./components/ProductLifecycleChart";
import EmployeeEfficiencyCard from "./components/EmployeeEfficencyCard";
import EmployeeWorkloadChart from "./components/EmployeeWorkloadChart";
import OutlierProductsChart from "./components/OutlierProductsChart";
import SalesForecastChart from "./components/SalesForecastCard";
import DiscountImpactChart from "./components/DiscountImpactChart";
import DiscountEfficiencyCard from "./components/DiscountEfficiencyCard";
import DiscountByCategoryChart from "./components/DiscountByCategoryCard";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const componentRegistry: Record<string, any> = {
  TotalSales: TotalSalesCard,
  TotalOrders: TotalOrdersCard,
  TopProducts: TopProductsChart,
  LatestOrders: LatestOrdersCard,
  TopCustomers: TopCustomersCard,
  TopEmployees: TopEmployeesChart,
  SalesPerMonth: SalesByMonthChart,
  TotalCustomers: TotalCustomersCard,
  SalesPerRegion: SalesPerRegionChart,
  ProductLifecycle: ProductLifecycleChart,
  EmployeeEfficiency: EmployeeEfficiencyCard,
  SupplierTotalSales: SupplierTotalSalesCard,
  AvgOrderProcessingTime: AvgOrderProcessingTimeCard,
  SalesForecast: SalesForecastChart,
  EmployeeWorkload: EmployeeWorkloadChart,
  OutlierProducts: OutlierProductsChart,
  DiscountImpact: DiscountImpactChart,
  DiscountEfficiency: DiscountEfficiencyCard,
  DiscountByCategory: DiscountByCategoryChart,
};
