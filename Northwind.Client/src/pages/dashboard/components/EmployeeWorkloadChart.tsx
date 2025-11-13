import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";

export type EmployeeWorkloadData = {
  employee: string;
  orderCount: number;
};

export default function EmployeeWorkloadChart({ data }: { data: EmployeeWorkloadData[] }) {
  return (
    <Card>
      <CardContent className="p-4">
        <h3 className="text-lg font-semibold mb-4">Belastning per anställd</h3>
        <ResponsiveContainer width="100%"  height={400}>
          <BarChart data={data}>
            <XAxis dataKey="employee" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="orderCount" fill="#3b82f6" />
          </BarChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
