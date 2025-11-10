import { PieChart, Pie, Tooltip, Cell, ResponsiveContainer } from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";

type Props = {
  settings: { title: string; color?: string };
  data?: { region: string; total: number }[];
};

const COLORS = ["#3b82f6", "#10b981", "#f59e0b", "#ef4444", "#6366f1"];

export default function SalesPerRegionChart({ settings, data = [] }: Props) {
  return (
    <Card>
      <CardContent className="p-6">
        <h2 className="text-lg font-semibold mb-4">{settings.title}</h2>
        <ResponsiveContainer width="100%" height={250}>
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
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
