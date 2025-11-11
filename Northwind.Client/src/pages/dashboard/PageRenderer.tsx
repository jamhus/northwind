import ItemRenderer from "./ItemRenderer";
import type { Page } from "../../api/dashboard.service";

type Props = { page: Page };

export default function PageRenderer({ page }: Props) {
  return (
    <div className="flex flex-col gap-6">
      {page.layout?.rows.map((row, i) => (
        <div key={i} className="flex gap-4">
          {row.columns.map((col, j) => (
            <div key={j} className="flex-1">
              <ItemRenderer itemRef={col.itemRef} items={page.pageItems} />
            </div>
          ))}
        </div>
      ))}
    </div>
  );
}
