import { useQuery } from "@tanstack/react-query";
import { dashboardService } from "../../api/dashboard.service";
import Loader from "../../components/common/Loader";
import DashboardNav from "../../components/layout/DashboardNav";
import DashboardRoutes from "../../routes/DashboardRoutes";


export default function DashboardLayout() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ["dashboard-rendered"],
    queryFn: dashboardService.render,
  });

  if (isLoading) return <Loader />;
  if (isError) return <div className="text-red-600">Kunde inte ladda dashboard.</div>;
  if (!data) return <div className="text-gray-500">Ingen dashboard tillgänglig.</div>;

  return (
    <div className="flex gap-6">
      {/* 🔹 Sidemeny */}
      <div className="w-56">
        <DashboardNav definition={data} />
      </div>

      {/* 🔹 Dashboard-innehåll */}
      <div className="flex-1">
        <DashboardRoutes definition={data} />
      </div>
    </div>
  );
}
