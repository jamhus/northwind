import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer
} from "recharts";
import { Card, CardContent } from "../../../components/ui/Card";

type Props = {
  settings: { title: string };
  data: { discount: string; avgOrderValue: number }[];
};

export default function DiscountEfficiencyCard({ settings, data }: Props) {
  return (
    <Card>
      <CardContent className="p-6">
        <h2 className="text-lg font-semibold mb-4">{settings.title}</h2>
        <ResponsiveContainer width="100%" height={400}>
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="discount" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="avgOrderValue" barSize={35} fill="#f97316" />
          </BarChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
}
