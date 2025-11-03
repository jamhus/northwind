import { useEffect, useState } from "react";

/**
 * En generisk hook som synkar en "value" (t.ex. id) mot en lista av objekt
 * och returnerar det aktuella objektet + en setter.
 */

export function useSyncedSelection<T extends Record<string, unknown>>(
  items: T[],
  value: number | string | undefined | null,
  key: keyof T
) {
  const [selected, setSelected] = useState<T | undefined>(undefined);

  useEffect(() => {
    if (!items?.length) return;
    if (value != null) {
      const found = items.find((item) => item[key] === value);
      setSelected(found ?? undefined);
    } else {
      setSelected(undefined);
    }
  }, [items, value, key]);

  return { selected, setSelected };
}
