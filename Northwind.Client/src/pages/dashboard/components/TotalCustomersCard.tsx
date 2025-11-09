type Props = {
  settings: { title: string; icon?: string };
  data?: { totalCustomers?: number };
};

export default function TotalCustomersCard({ settings, data }: Props) {
  return (
    <div className="border rounded-lg p-4 shadow-sm bg-white">
      <div className="text-sm text-gray-500">{settings.title}</div>
      <div className="text-2xl font-bold text-green-600">
        {data?.totalCustomers ?? 0}
      </div>
    </div>
  );
}
