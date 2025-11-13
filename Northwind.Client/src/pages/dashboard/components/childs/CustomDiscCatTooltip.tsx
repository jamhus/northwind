
type TooltipEntry = { name: string; value: number; color?: string; dataKey?: string };

interface CustomTooltipProps {
  active?: boolean;
  payload?: TooltipEntry[];
  label?: string;
}

export default function CustomDiscCatTooltip({ active, payload, label }: CustomTooltipProps) {
  if (!active || !payload || !payload.length) return null;

  return (
    <div className="bg-white shadow-md border border-gray-200 rounded-md p-3 text-sm">
      <div className="font-semibold text-gray-800 mb-2">
        Rabattnivå: {label}
      </div>
      <table className="text-xs">
        <tbody>
          {payload.map((entry: TooltipEntry) => (
            <tr key={entry.dataKey}>
              <td>
                <div
                  className="w-3 h-3 rounded-full"
                  style={{ backgroundColor: entry.color || "#8884d8" }}
                />
              </td>
              <td className="px-2 text-gray-700">{entry.name}</td>
              <td className="text-gray-600 text-right">
                {Number(entry.value).toLocaleString("sv-SE")} $
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
