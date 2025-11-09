import ItemRenderer from "./ItemRenderer";
import type { ReportPage } from "../../api/dashboard.service";

type Props = { page: ReportPage };

export default function ReportPageRenderer({ page }: Props) {
  return (
    <div className="flex flex-col gap-6">
      {page.layout?.rows.map((row, i) => (
        <div key={i} className="flex gap-4">
          {row.columns.map((col, j) => (
            <div key={j} className="flex-1">
              <ItemRenderer itemRef={col.itemRef} items={page.reportPageItems} />
            </div>
          ))}
        </div>
      ))}
    </div>
  );
}
