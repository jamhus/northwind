import { useState } from "react";
import type { DashboardDefinition } from "../../api/dashboard.service";
import PageRenderer from "../dashboard/PageRenderer";

type Props = {
  definition: DashboardDefinition;
  onClose?: () => void;
};

export default function DashboardPreviewPortal({ definition, onClose }: Props) {
  const pages = definition.pages ?? [];
  const [activeKey, setActiveKey] = useState(pages[0]?.key ?? "");
  const activePage = pages.find(p => p.key === activeKey);

  return (
    <div className="flex flex-col h-[90vh] bg-gray-50 rounded-lg shadow-lg overflow-hidden">
      {/* Header */}
      <div className="flex items-center justify-between bg-white px-6 py-3">
        <h1 className="text-lg font-semibold text-gray-700">Dashboard-förhandsgranskning</h1>
        {onClose && (
          <button
            onClick={onClose}
            className="px-3 py-1.5 bg-red-500 text-white rounded hover:bg-red-600"
          >
            Stäng
          </button>
        )}
      </div>

      {/* Navigation */}
      <div className="flex gap-2 bg-white px-4 py-2 overflow-x-auto">
        {pages.map((page) => (
          <button
            key={page.key}
            onClick={() => setActiveKey(page.key)}
            className={`px-3 py-1.5 rounded transition-colors ${
              page.key === activeKey
                ? "bg-blue-600 text-white"
                : "bg-gray-100 hover:bg-gray-200 text-gray-700"
            }`}
          >
            {page.name?.[0]?.text ?? page.key}
          </button>
        ))}
      </div>

      {/* Page Content */}
      <div className="flex-1 overflow-auto p-6">
        {activePage ? (
          <PageRenderer page={activePage} />
        ) : (
          <div className="text-center text-gray-500 italic mt-20">
            Ingen sida vald.
          </div>
        )}
      </div>
    </div>
  );
}
