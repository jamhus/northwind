import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

type Props = {
  settings: { title: string; color?: string };
  data?: { name: string; total: number }[];
};

export default function TopEmployeesChart({ settings, data = [] }: Props) {
  return (
    <div className="border rounded-lg p-4 bg-white shadow-sm">
      <div className="text-sm font-medium text-gray-700 mb-2">
        {settings.title}
      </div>
      <ResponsiveContainer width="100%" height={250}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="name" />
          <YAxis />
          <Tooltip />
          <Bar dataKey="total" fill={settings.color || "#f97316"} />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
}
