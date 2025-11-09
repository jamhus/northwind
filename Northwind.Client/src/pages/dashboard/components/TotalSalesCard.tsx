type Props = {
  settings: { title: string; prefix?: string; suffix?: string };
  data?: { totalSales?: number };
};

export default function TotalSalesCard({ settings, data }: Props) {
  return (
    <div className="border rounded-lg p-4 shadow-sm bg-white">
      <div className="text-sm text-gray-500">{settings.title}</div>
      <div className="text-2xl font-bold">
        {settings.prefix}
        {data?.totalSales?.toLocaleString("sv-SE")}
        {settings.suffix}
      </div>
    </div>
  );
}
