import { PieChart, Pie, Tooltip, Cell, ResponsiveContainer } from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";

type Props = {
  settings: { title: string; suffix?: string };
  data?: { region: string; totalSales: number }[];
};

const COLORS = ["#3b82f6", "#10b981", "#f59e0b", "#ef4444", "#6366f1"];

export default function SalesPerRegionChart({ settings, data = [] }: Props) {
  return (
    <Card>
      <CardContent className="p-6">
        <h2 className="text-lg font-semibold mb-4">{settings.title}</h2>
          <ResponsiveContainer width="100%" height={400}>
            <PieChart>
              <Pie
                dataKey="totalSales"
                data={data}
                nameKey="region"
                outerRadius={90}
                fill={"#3b82f6"}
                label
              >
                {data.map((_, i) => (
                  <Cell key={i} fill={COLORS[i % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip formatter={(v: number) => `${v.toLocaleString()} ${settings.suffix}`} />
            </PieChart>
          </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
