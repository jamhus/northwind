import type { DashboardDefinition } from "../../api/dashboard.service";
import PageRenderer from "./PageRenderer";

type Props = {
  definition: DashboardDefinition;
};

export default function DashboardRenderer({ definition }: Props) {
  const page = definition.pages[0];
  if (!page) return <div>Ingen dashboard att visa</div>;

  return (
    <div className="flex flex-col gap-6">
      <h1 className="text-2xl font-semibold">{page.name?.[0]?.text}</h1>
      <PageRenderer page={page} />
    </div>
  );
}
