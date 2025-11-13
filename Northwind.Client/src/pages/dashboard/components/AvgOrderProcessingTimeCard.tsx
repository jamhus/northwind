import { Card, CardContent } from "../../../components/ui/Card";

type Props = {
  settings: { title: string };
  data: number | null;
};

export default function AvgOrderProcessingTimeCard({ settings, data }: Props) {
  return (
    <Card>
      <CardContent className="p-6 text-center">
        <h2 className="text-lg font-semibold mb-2">{settings.title}</h2>
        <div className="text-3xl font-bold text-blue-600">
          {data ?? 0} dagar
        </div>
      </CardContent>
    </Card>
  );
}
