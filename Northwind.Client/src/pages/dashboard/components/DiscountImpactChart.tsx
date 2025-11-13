import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  CartesianGrid,
} from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";

export type DiscountImpactData = {
  discount: number;
  totalSales: number;
};
type Props = { data: DiscountImpactData[] };

export default function DiscountImpactChart({ data }: Props) {
  return (
    <Card>
      <CardContent className="p-4">
        <h3 className="text-lg font-semibold mb-3">
          Rabatteffekt på försäljning
        </h3>
        <ResponsiveContainer width="100%"  height={400}>
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="discount" tickFormatter={(v) => `${v}%`} />
            <YAxis tickFormatter={(v) => `${v.toLocaleString()} $`} />
            <Tooltip
              formatter={(v: number) => `${v.toLocaleString()} $`}
              labelFormatter={(v: number) => `Rabatt: ${v}%`}
            />
            <Bar dataKey="totalSales" fill="#10b981" />
          </BarChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
