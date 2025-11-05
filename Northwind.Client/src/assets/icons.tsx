import {
  Leaf,
  Milk,
  Fish,
  Wheat,
  Candy,
  Coffee,
  Dumbbell,
  Drumstick,
  ShoppingBasket,
  UtensilsCrossed,
} from "lucide-react";
import type { JSX } from "react";

export const categoryIcons: Record<string, JSX.Element> = {
  Seafood: <Fish className="text-cyan-500" />,
  Produce: <Leaf className="text-green-600" />,
  Beverages: <Coffee className="text-amber-500" />,
  Confections: <Candy className="text-pink-500" />,
  "Dairy Products": <Milk className="text-blue-400" />,
  default: <ShoppingBasket className="text-gray-400" />,
  "Meat/Poultry": <Drumstick className="text-red-500" />,
  "Grains/Cereals": <Wheat className="text-yellow-500" />,
  Condiments: <UtensilsCrossed className="text-orange-600" />,
  "Workout Supplements": <Dumbbell className="text-purple-500" />,
};
