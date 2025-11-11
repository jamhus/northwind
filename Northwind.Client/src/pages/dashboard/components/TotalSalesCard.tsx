import { DollarSign } from "lucide-react";
import StatCard from "./childs/StateCard";

type Props = {
  settings: { title: string; suffix?: string };
  data?: number;
};

export default function TotalSalesCard({ settings, data }: Props) {
  return (
    <StatCard icon={<DollarSign />} label={settings.title} value={`${data?.toLocaleString("sv-SE")} ${settings.suffix}`} />
  );
}
