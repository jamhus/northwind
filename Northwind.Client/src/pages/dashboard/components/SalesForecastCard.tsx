import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";

export type ForecastData = {
  label: string;
  value: number;
};
type Props = { data: ForecastData[] };

export default function SalesForecastChart({ data }: Props) {
  return (
    <Card>
      <CardContent className="p-4">
        <h3 className="text-lg font-semibold mb-4">Försäljningsprognos</h3>
        <ResponsiveContainer width="100%"  height={400}>
          <LineChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="label" />
            <YAxis tickFormatter={(v) => `${v.toLocaleString()}$`} />
            <Tooltip
              formatter={(v: number) => `${v.toLocaleString()}$`}
              labelFormatter={(v) => `Månad: ${v}`}
            />
            <Line
              type="monotone"
              dataKey="value"
              stroke="#3b82f6"
              strokeWidth={3}
              dot={{ r: 4 }}
            />
          </LineChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
