import type { DashboardDefinition } from "../../api/dashboard.service";
import ReportPageRenderer from "./ReportPageRenderer";

type Props = {
  definition: DashboardDefinition;
};

export default function DashboardRenderer({ definition }: Props) {
  const page = definition.reportPages?.[0]; // Just nu visar vi första sidan
  if (!page) return <div>Ingen dashboard att visa</div>;

  return (
    <div className="flex flex-col gap-6">
      <h1 className="text-2xl font-semibold">{page.name?.[0]?.text}</h1>
      <ReportPageRenderer page={page} />
    </div>
  );
}
