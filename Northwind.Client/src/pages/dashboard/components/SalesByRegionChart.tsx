import { PieChart, Pie, Tooltip, Cell, ResponsiveContainer } from "recharts";

type Props = {
  settings: { title: string; color?: string };
  data?: { region: string; total: number }[];
};

const COLORS = ["#3b82f6", "#10b981", "#f59e0b", "#ef4444", "#6366f1"];

export default function SalesPerRegionChart({ settings, data = [] }: Props) {
  return (
    <div className="border rounded-lg p-4 bg-white shadow-sm">
      <div className="text-sm font-medium text-gray-700 mb-2">
        {settings.title}
      </div>
      <ResponsiveContainer width="100%" height={450}>
        <PieChart>
          <Pie
            dataKey="total"
            data={data}
            nameKey="region"
            outerRadius={90}
            fill={settings.color || "#3b82f6"}
            label
          >
            {data.map((_, i) => (
              <Cell key={i} fill={COLORS[i % COLORS.length]} />
            ))}
          </Pie>
          <Tooltip />
        </PieChart>
      </ResponsiveContainer>
    </div>
  );
}
