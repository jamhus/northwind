import { useState } from "react";
import { Card, CardContent } from "../../../components/ui/Card";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

type ProductLifecycleDatum = {
  product: string;
  data: { month: string; totalSales: number }[];
};

type Props = {
  settings: {
    title: string;
    color?: string;
    take?: number;
  };
  data: ProductLifecycleDatum[];
};

export default function ProductLifecycleCard({ settings, data }: Props) {
  const [take, setTake] = useState(settings.take ?? 5);

  if (!data?.length) {
    return (
      <Card>
        <CardContent className="p-6 text-gray-500 italic text-center">
          Ingen data tillgänglig.
        </CardContent>
      </Card>
    );
  }

  // 🔹 Filtrera de N första produkterna
  const visibleData = data.slice(0, take);

  // 🔹 Sammanställ alla månader på X-axeln
  const allMonths = Array.from(
    new Set(visibleData.flatMap((p) => p.data.map((d) => d.month)))
  ).sort();

  const chartData = allMonths.map((month) => {
    const row: Record<string, unknown> = { month };
    visibleData.forEach((p) => {
      const found = p.data.find((d) => d.month === month);
      row[p.product] = found?.totalSales ?? 0;
    });
    return row;
  });

  const palette = [
    "#1D4ED8", "#059669", "#D97706", "#7C3AED", "#DC2626", "#2563EB", "#6B7280",
  ];

  return (
    <Card>
            <CardContent className="p-4">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-lg font-semibold">{settings.title}</h2>

          {/* 🔹 Select för antal produkter */}
          <select
            value={take}
            onChange={(e) => setTake(Number(e.target.value))}
            className="border border-gray-300 rounded px-2 py-1 text-sm focus:ring focus:ring-blue-200"
          >
            {Array.from(new Set([3, 5, 8, 10, settings.take])).map((n) => (
              <option key={n} value={n}>
                Visa {n}
              </option>
            ))}
          </select>
        </div>

        <div className="w-full h-[400px]">
          <ResponsiveContainer>
            <LineChart data={chartData}>
              <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
              <XAxis dataKey="month" tick={{ fontSize: 12 }} />
              <YAxis tick={{ fontSize: 12 }} />
              <Tooltip
                contentStyle={{
                  backgroundColor: "#fff",
                  border: "1px solid #e5e7eb",
                  fontSize: 12,
                }}
                formatter={(value: number) =>
                  value.toLocaleString("sv-SE", {
                    minimumFractionDigits: 1,
                    maximumFractionDigits: 1,
                  }) + "$"
                }
              />
              <Legend wrapperStyle={{ fontSize: 12 }} />
              {visibleData.map((p, i) => (
                <Line
                  key={p.product}
                  type="monotone"
                  dataKey={p.product}
                  stroke={palette[i % palette.length]}
                  strokeWidth={2}
                  dot={false}
                  activeDot={{ r: 4 }}
                />
              ))}
            </LineChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  );
}
