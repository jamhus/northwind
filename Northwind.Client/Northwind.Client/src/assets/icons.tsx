import {
  Coffee,
  UtensilsCrossed,
  Candy,
  Milk,
  Wheat,
  Drumstick,
  Leaf,
  Fish,
  ShoppingBasket,
} from "lucide-react";
import type { JSX } from "react";

export const categoryIcons: Record<string, JSX.Element> = {
  Beverages: <Coffee className="text-amber-500" />,
  Condiments: <UtensilsCrossed className="text-orange-600" />,
  Confections: <Candy className="text-pink-500" />,
  "Dairy Products": <Milk className="text-blue-400" />,
  "Grains/Cereals": <Wheat className="text-yellow-500" />,
  "Meat/Poultry": <Drumstick className="text-red-500" />,
  Produce: <Leaf className="text-green-600" />,
  Seafood: <Fish className="text-cyan-500" />,
  default: <ShoppingBasket className="text-gray-400" />,
};
