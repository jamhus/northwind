import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer
} from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";

type DiscountCategoryData = {
  avgDiscount: string;
  category: string;
  totalSales: number;
};

type Props = {
  settings: { title: string };
  data: DiscountCategoryData[];
};

type DiscountGroup = {
  avgDiscount: string;
  [key: string]: string | number;
};

// helper som konverterar kategorinamn till giltig nyckel
const toSafeKey = (name: string) => name.replace(/[^a-zA-Z0-9_]/g, "_");

export default function DiscountByCategoryCard({ settings, data }: Props) {
  // skapa en mappning: giltig nyckel -> originalnamn
  const categoryMap = Object.fromEntries(
    [...new Set(data.map((d) => d.category))].map((cat) => [toSafeKey(cat), cat])
  );

  // reducera datan till rabattnivåer
  const grouped: DiscountGroup[] = Object.values(
    data.reduce<Record<string, DiscountGroup>>((acc, curr) => {
      const safeKey = toSafeKey(curr.category);
      if (!acc[curr.avgDiscount]) {
        acc[curr.avgDiscount] = { avgDiscount: curr.avgDiscount };
      }
      acc[curr.avgDiscount][safeKey] = curr.totalSales;
      return acc;
    }, {})
  );

  console.table(grouped);

  const colors = [
    "#10b981", "#3b82f6", "#f97316", "#a855f7", "#ef4444", "#14b8a6", "#f59e0b",
  ];

  return (
    <Card>
      <CardContent className="p-6">
        <h2 className="text-lg font-semibold mb-4">{settings.title}</h2>
        <ResponsiveContainer width="100%" height={380}>
          <BarChart data={grouped}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="avgDiscount" />
            <YAxis />
            <Tooltip
              formatter={(v: number) => `$${v.toLocaleString()}`}
              labelFormatter={(l) => `Rabatt: ${l}`}
            />
            <Legend />
            {Object.entries(categoryMap).map(([safeKey, original], i) => (
              <Bar
                key={safeKey}
                dataKey={safeKey}
                name={original}
                fill={colors[i % colors.length]}
                barSize={35}
              />
            ))}
          </BarChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
