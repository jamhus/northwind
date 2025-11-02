import { Edit, Trash2, Plus } from "lucide-react";

type TableProps<T> = {
  data: T[];
  columns: { key: keyof T; label: string, width?: string }[];
  onAdd?: () => void;
  onEdit?: (item: T) => void;
  onDelete?: (item: T) => void;
  title?: string;
};

export default function Table<T extends object>({
  data,
  columns,
  onAdd,
  onEdit,
  onDelete,
  title = "Data",
}: TableProps<T>) {
  return (
    <div className="p-4 border border-gray-200 rounded-lg shadow-sm bg-white">
      {/* Header */}
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-lg font-semibold text-gray-700">{title}</h2>
        {onAdd && (
          <button
            onClick={onAdd}
            className="flex items-center gap-2 bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700 transition"
          >
            <Plus size={16} />
            Lägg till
          </button>
        )}
      </div>

      {/* Tabell */}
      <div className="overflow-x-auto">
        <table className="w-full border-collapse text-left text-sm">
          <thead className="bg-gray-100 text-gray-700 uppercase text-xs">
            <tr>
              {columns.map((col) => (
                <th key={String(col.key)} className="p-3 border-b" style={col.width ? { width: col.width } : {}}>
                  {col.label}
                </th>
              ))}
              {(onEdit || onDelete) && (
                <th className="p-3 border-b">Actions</th>
              )}
            </tr>
          </thead>

          <tbody>
            {data.length > 0 ? (
              data.map((item, idx) => (
                <tr
                  key={idx}
                  className="odd:bg-white even:bg-gray-50 hover:bg-blue-50 transition"
                >
                  {columns.map((col) => (
                    <td key={String(col.key)} className="p-3 border-b">
                      {item[col.key] != null ? String(item[col.key]) : "-"}
                    </td>
                  ))}
                  {(onEdit || onDelete) && (
                    <td className="p-3 border-b">
                      <div className="flex gap-3">
                        {onEdit && (
                          <button
                            onClick={() => onEdit(item)}
                            className="text-blue-600 hover:text-blue-800"
                          >
                            <Edit size={18} />
                          </button>
                        )}
                        {onDelete && (
                          <button
                            onClick={() => onDelete(item)}
                            className="text-red-600 hover:text-red-800"
                          >
                            <Trash2 size={18} />
                          </button>
                        )}
                      </div>
                    </td>
                  )}
                </tr>
              ))
            ) : (
              <tr>
                <td
                  colSpan={columns.length + 1}
                  className="text-center text-gray-500 p-4 italic bg-gray-50"
                >
                  Ingen data att visa
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
