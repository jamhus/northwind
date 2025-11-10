import { Card, CardContent } from "../../../../components/ui/Card";

export default function StatCard({ icon, label, value }: { icon: React.ReactNode; label: string; value: string }) {
  return (
    <Card className="bg-white border border-gray-200 shadow-sm hover:shadow-md transition">
      <CardContent className="flex items-center gap-4 p-4">
        <div className="text-blue-600 bg-blue-50 p-3 rounded-lg">{icon}</div>
        <div>
          <div className="text-sm text-gray-500">{label}</div>
          <div className="text-xl font-semibold text-gray-800">{value}</div>
        </div>
      </CardContent>
    </Card>
  );
}
