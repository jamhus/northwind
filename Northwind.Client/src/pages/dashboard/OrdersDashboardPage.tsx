import { useQuery } from "@tanstack/react-query";

import { TrendingUp, Users, Package, DollarSign } from "lucide-react";
import {
  LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer,
  BarChart, Bar, PieChart, Pie, Cell, Legend
} from "recharts";
import Loader from "../../components/common/Loader";
import { useAuth } from "../../contexts/auth/useAuth";
import api from "../../api/axios.client";
import { Card, CardContent } from "../../components/ui/Card";

type DashboardData = {
  totalSales: number;
  orderCount: number;
  customerCount: number;
  salesByMonth: { month: string; totalSales: number }[];
  topProducts: { name: string; totalSales: number }[];
  salesByRegion: { region: string; totalSales: number }[];
  performance: { employeeName: string; totalSales: number; orderCount: number }[];
};

export default function OrdersDashboardPage() {
  const { user } = useAuth();

  const { data, isLoading, isError } = useQuery<DashboardData>({
    queryKey: ["ordersDashboard"],
    queryFn: async () => (await api.get("/dashboard/orders")).data,
  });

  if (isLoading) return <Loader />;
  if (isError) return <div className="p-6 text-red-600">Kunde inte hämta dashboard-data.</div>;

  const COLORS = ["#2563eb", "#10b981", "#f59e0b", "#ef4444", "#8b5cf6"];

  return (
    <div className="p-8 max-w-[1100px] mx-auto space-y-8">
      <h1 className="text-2xl font-semibold text-gray-800 mb-4">📊 Orderöversikt</h1>

      {/* Stat Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-6">
        <StatCard icon={<DollarSign />} label="Total försäljning" value={`${data!.totalSales.toLocaleString()} kr`} />
        <StatCard icon={<Package />} label="Antal ordrar" value={data!.orderCount.toString()} />
        <StatCard icon={<Users />} label="Kunder" value={data!.customerCount.toString()} />
      </div>

      {/* Sales by Month */}
      <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold mb-4">Försäljning per månad</h2>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={data!.salesByMonth}>
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip formatter={(v: number) => `${v.toLocaleString()} kr`} />
              <Line type="monotone" dataKey="totalSales" stroke="#2563eb" strokeWidth={2} dot={false} />
            </LineChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {/* Top Products */}
      <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold mb-4">Topp 5 produkter</h2>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={data!.topProducts}>
              <XAxis dataKey="name" />
              <YAxis />
              <Tooltip formatter={(v: number) => `${v.toLocaleString()} kr`} />
              <Bar dataKey="totalSales" fill="#10b981" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {/* Sales by Region */}
      <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold mb-4">Försäljning per region</h2>
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={data!.salesByRegion}
                dataKey="totalSales"
                nameKey="region"
                cx="50%"
                cy="50%"
                outerRadius={100}
                label
              >
                {data!.salesByRegion.map((_, i) => (
                  <Cell key={i} fill={COLORS[i % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip formatter={(v: number) => `${v.toLocaleString()} kr`} />
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>

      {user?.roles.includes("Admin") && (
        <Card>
          <CardContent className="p-6">
            <h2 className="text-lg font-semibold mb-4">Prestation per anställd</h2>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={data!.performance}>
                <XAxis dataKey="employeeName" />
                <YAxis />
                <Tooltip formatter={(v: number) => `${v.toLocaleString()} kr`} />
                <Bar dataKey="totalSales" fill="#f59e0b" />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      )}
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
