
import TotalSalesCard from "./components/TotalSalesCard";
import TotalOrdersCard from "./components/TotalOrdersCard";
import TopProductsChart from "./components/TopProductsChart";
import SalesByMonthChart from "./components/SalesByMonthChart";
import TopEmployeesChart from "./components/TopEmployeesChart";
import TotalCustomersCard from "./components/TotalCustomersCard";
import SalesPerRegionChart from "./components/SalesByRegionChart";
import LatestOrdersCard from "./components/childs/LatestOrdersCard";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const componentRegistry: Record<string, any> = {
  TotalSales: TotalSalesCard,
  TotalOrders: TotalOrdersCard,
  TopProducts: TopProductsChart,
  LatestOrders: LatestOrdersCard,
  TopEmployees: TopEmployeesChart,
  SalesPerMonth: SalesByMonthChart,
  TotalCustomers: TotalCustomersCard,
  SalesPerRegion: SalesPerRegionChart,
};
