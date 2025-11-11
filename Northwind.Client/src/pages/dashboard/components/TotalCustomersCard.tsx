import { Users } from "lucide-react";
import StatCard from "./childs/StateCard";

type Props = {
  settings: { title: string; icon?: string };
  data?: number
};

export default function TotalCustomersCard({ settings, data }: Props) {
  return (
    <StatCard
      icon={<Users />}
      label={settings.title}
      value={data?.toString() || "0"}
    />
  );
}
