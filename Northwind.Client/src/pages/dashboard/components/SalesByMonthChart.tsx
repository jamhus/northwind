import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

type Props = {
  settings: { title: string; color?: string };
  data?: { month: string; total: number }[];
};

export default function SalesByMonthChart({ settings, data = [] }: Props) {
  return (
    <div className="border rounded-lg p-4 bg-white shadow-sm">
      <div className="text-sm font-medium text-gray-700 mb-2">
        {settings.title}
      </div>
      <ResponsiveContainer width="100%" height={250}>
        <LineChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="month" />
          <YAxis />
          <Tooltip />
          <Line
            type="monotone"
            dataKey="total"
            stroke={settings.color || "#3b82f6"}
            strokeWidth={2}
          />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
}
