import { useQuery } from "@tanstack/react-query";
import Loader from "../../components/common/Loader";
import api from "../../api/axios.client";
import { Package, DollarSign, ClipboardList } from "lucide-react";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { Card, CardContent } from "../../components/ui/Card";

type DashboardData = {
  totalSales: number;
  orderCount: number;
  performance: { employeeName: string; totalSales: number; orderCount: number }[];
  salesByMonth: { month: string; totalSales: number }[];
  recentOrders: { orderId: number; customer: string; date: string; total: number }[];
};

export default function EmployeeDashboardPage() {
  const { data, isLoading, isError } = useQuery<DashboardData>({
    queryKey: ["employeeDashboard"],
    queryFn: async () => (await api.get("/dashboard/orders")).data,
  });

  if (isLoading) return <Loader />;
  if (isError) return <div className="p-6 text-red-600">Kunde inte hämta dashboard-data.</div>;

  const stats = data!.performance[0];
  const COLORS = ["#2563eb", "#10b981", "#f59e0b", "#ef4444"];

  return (
    <div className="p-8 max-w-[1100px] mx-auto space-y-8">
      <h1 className="text-2xl font-semibold text-gray-800 mb-4">
        👋 Hej {stats?.employeeName || "Employee"}!
      </h1>

      {/* Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-6">
        <StatCard icon={<DollarSign />} label="Total försäljning" value={`${stats.totalSales.toLocaleString()} kr`} />
        <StatCard icon={<Package />} label="Totala ordrar" value={stats.orderCount.toString()} />
        <StatCard icon={<ClipboardList />} label="Aktiva kunder" value={data!.orderCount.toString()} />
      </div>

      {/* Sales by Month */}
      <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold mb-4">Din försäljning per månad</h2>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={data!.salesByMonth}>
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip formatter={(v: number) => `${v.toLocaleString()} kr`} />
              <Bar dataKey="totalSales" fill="#2563eb" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {/* Recent Orders */}
      <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold mb-4">Senaste ordrarna</h2>
          <div className="overflow-x-auto max-h-[400px] overflow-y-auto border rounded">
            <table className="w-full text-left text-sm">
              <thead className="bg-gray-100 sticky top-0">
                <tr>
                  <th className="p-2 border-b">Order #</th>
                  <th className="p-2 border-b">Kund</th>
                  <th className="p-2 border-b">Datum</th>
                  <th className="p-2 border-b">Totalt</th>
                </tr>
              </thead>
              <tbody>
                {data!.recentOrders.length ? (
                  data!.recentOrders.map((o) => (
                    <tr key={o.orderId} className="hover:bg-blue-50">
                      <td className="p-2 border-b">{o.orderId}</td>
                      <td className="p-2 border-b">{o.customer}</td>
                      <td className="p-2 border-b">{new Date(o.date).toLocaleDateString()}</td>
                      <td className="p-2 border-b">{o.total.toLocaleString()} kr</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan={4} className="text-center p-4 text-gray-500 italic">
                      Inga ordrar hittades.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

function StatCard({ icon, label, value }: { icon: React.ReactNode; label: string; value: string }) {
  return (
    <Card className="bg-white border border-gray-200 shadow-sm hover:shadow-md transition">
      <CardContent className="flex items-center gap-4 p-4">
        <div className="text-blue-600 bg-blue-50 p-3 rounded-lg">{icon}</div>
        <div>
          <div className="text-sm text-gray-500">{label}</div>
          <div className="text-xl font-semibold text-gray-800">{value}</div>
        </div>
      </CardContent>
    </Card>
  );
}
