import { Package } from "lucide-react";
import StatCard from "./childs/StateCard";

type Props = {
  settings: { title: string; icon?: string };
  data?: number;
};

export default function TotalOrdersCard({ settings, data }: Props) {
  return (
    <StatCard
      icon={<Package />}
      label={settings.title}
      value={data?.toString() || "0"}
    />
  );
}
