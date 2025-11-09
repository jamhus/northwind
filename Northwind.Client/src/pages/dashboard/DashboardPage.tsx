import Loader from "../../components/common/Loader";
import DashboardRenderer from "./DashboardRenderer";
import { useDashboard } from "../../hooks/useDashboard";

export default function DashboardPage() {
  const { data, isLoading, isError } = useDashboard();

  if (isLoading) return <Loader />;
  if (isError) return <div>Kunde inte hämta dashboard.</div>;

  return (
    <div className="p-6">
      <DashboardRenderer definition={data} />
    </div>
  );
}
