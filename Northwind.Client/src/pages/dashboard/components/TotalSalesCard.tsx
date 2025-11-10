import { DollarSign } from "lucide-react";
import StatCard from "./childs/StateCard";

type Props = {
  settings: { title: string; prefix?: string; suffix?: string };
  data?: { totalSales?: number };
};

export default function TotalSalesCard({ settings, data }: Props) {
  return (
    <StatCard icon={<DollarSign />} label="Total försäljning" value={`${settings.prefix} ${data?.totalSales?.toLocaleString("sv-SE")} ${settings.suffix}`} />
  );
}
