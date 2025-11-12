import { Card, CardContent } from "../../../components/ui/Card";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";

type Props = {
  settings: {
    title: string;
    chartType?: "bar" | "list";
    color?: string;
  };
  data: { customer: string; totalSales: number }[];
};

export default function TopCustomersCard({ settings, data }: Props) {
  const color = settings.color || "#14b8a6"; // teal som standard

  return (
    <Card className="h-full">
      <CardContent className="p-6">
        <h2 className="text-lg font-semibold mb-4">{settings.title}</h2>

        {!data?.length ? (
          <div className="text-gray-500 italic">Inga kunder hittades.</div>
        ) : settings.chartType === "bar" ? (
          <div className="h-72">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart
                data={data.map((d) => ({
                  name: d.customer,
                  value: d.totalSales,
                }))}
                margin={{ top: 5, right: 20, left: 10, bottom: 30 }}
              >
                <XAxis
                  dataKey="name"
                  angle={-20}
                  textAnchor="end"
                  interval={0}
                  height={50}
                  tick={{ fontSize: 12 }}
                />
                <YAxis tick={{ fontSize: 12 }} />
                <Tooltip
                  formatter={(value: number) =>
                    `${value.toLocaleString("sv-SE", {
                      minimumFractionDigits: 1,
                      maximumFractionDigits: 1,
                    })} $`
                  }
                />
                <Bar dataKey="value" fill={color} radius={[6, 6, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        ) : (
          <div className="space-y-2">
            {data.map((item, i) => (
              <div
                key={i}
                className="flex justify-between items-center border-b last:border-none py-1"
              >
                <span className="truncate text-gray-700">{item.customer}</span>
                <span className="font-semibold text-gray-800">
                  {item.totalSales.toLocaleString("sv-SE", {
                    minimumFractionDigits: 1,
                    maximumFractionDigits: 1,
                  })}{" "}
                  $
                </span>
              </div>
            ))}
          </div>
        )}
      </CardContent>
    </Card>
  );
}
