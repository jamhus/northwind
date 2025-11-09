import { componentRegistry } from "./registery";
import type { ReportPageItem } from "../../api/dashboard.service";
type Props = {
  itemRef: string;
  items: ReportPageItem[];
};

export default function ItemRenderer({ itemRef, items }: Props) {
  const item = items.find((x) => x.key === itemRef);
  if (!item || !item.enabled) return null;

  const Comp = componentRegistry[item.reportPageItemType];
  if (!Comp) return <div>❌ {item.reportPageItemType} saknas</div>;

  return <Comp settings={item.settings} data={item.data} />;
}
