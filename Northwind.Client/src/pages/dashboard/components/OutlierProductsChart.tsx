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

export type OutlierProductData = {
  product: string;
  total: number;
};
type Props = { data: OutlierProductData[] };

export default function OutlierProductsChart({ data }: Props) {
  if (!data?.length)
    return (
      <Card>
        <CardContent className="p-6 text-gray-500 text-center">
          Inga avvikande produkter hittades.
        </CardContent>
      </Card>
    );

  return (
    <Card>
      <CardContent className="p-4">
        <h3 className="text-lg font-semibold mb-3">Avvikande produkter</h3>
        <ResponsiveContainer width="100%"  height={400}>
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="product" />
            <YAxis tickFormatter={(v) => `${v.toLocaleString()} kr`} />
            <Tooltip
              formatter={(v: number) => `${v.toLocaleString()} kr`}
              labelFormatter={(v) => `Produkt: ${v}`}
            />
            <Bar dataKey="total" fill="#f59e0b" />
          </BarChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
