type Props = {
  settings: { title: string; icon?: string };
  data?: { totalOrders?: number };
};

export default function TotalOrdersCard({ settings, data }: Props) {
  return (
    <div className="border rounded-lg p-4 shadow-sm bg-white">
      <div className="text-sm text-gray-500">{settings.title}</div>
      <div className="text-2xl font-bold text-blue-600">
        {data?.totalOrders ?? 0}
      </div>
    </div>
  );
}
