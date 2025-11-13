import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";
import { Car } from "lucide-react";

type Props = {
  settings: { title: string; color?: string };
  data?: { name: string; total: number }[];
};

export default function TopEmployeesChart({ settings, data = [] }: Props) {
  return (
    <Card>
            <CardContent className="p-4">
        <h2 className="text-lg font-semibold mb-4">{settings.title}</h2>
          <ResponsiveContainer width="100%"  height={400}>
            <BarChart data={data}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="employeeName" />
              <YAxis />
              <Tooltip />
              <Bar dataKey="totalSales" fill={settings.color || "#f97316"} />
            </BarChart>
          </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
