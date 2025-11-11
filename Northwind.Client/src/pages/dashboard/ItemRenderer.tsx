import { componentRegistry } from "./registery";
import type { PageItem } from "../../api/dashboard.service";
type Props = {
  itemRef: string;
  items: PageItem[];
};

export default function ItemRenderer({ itemRef, items }: Props) {
  const item = items.find((x) => x.key === itemRef);
  if (!item) return null;
  const Comp = componentRegistry[item.type];

  if (!Comp) return <div>❌ {item.type} saknas</div>;
  
  return <Comp settings={item.settings} data={item.data} />;
}
