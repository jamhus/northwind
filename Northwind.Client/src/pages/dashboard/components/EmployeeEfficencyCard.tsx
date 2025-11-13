import { Card, CardContent } from "../../../components/ui/Card";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  CartesianGrid,
} from "recharts";

type Props = {
  settings: { title: string };
  data: { employee: string; efficiency: number }[];
};

export default function EmployeeEfficiencyCard({ settings, data }: Props) {
  return (
    <Card>
      <CardContent className="p-6">
        <h2 className="text-lg font-semibold mb-4 text-gray-700">
          {settings.title}
        </h2>

        {data && data.length > 0 ? (
          <ResponsiveContainer width="100%"  height={400}>
            <BarChart data={data} margin={{ top: 10, right: 20, bottom: 5, left: 0 }}>
              <CartesianGrid strokeDasharray="3 3" stroke="#e2e8f0" />
              <XAxis dataKey="employee" tick={{ fontSize: 12 }} />
              <YAxis
                tickFormatter={(v) => `${v.toLocaleString()} kr`}
                tick={{ fontSize: 12 }}
              />
              <Tooltip
                cursor={{ fill: "rgba(0,0,0,0.05)" }}
                formatter={(value: number) => `${value.toLocaleString()} kr/order`}
                labelFormatter={(label) => `Anställd: ${label}`}
              />
              <Bar dataKey="efficiency" fill="#f91674ff" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        ) : (
          <p className="text-gray-500 italic text-sm">Ingen data tillgänglig.</p>
        )}
      </CardContent>
    </Card>
  );
}
