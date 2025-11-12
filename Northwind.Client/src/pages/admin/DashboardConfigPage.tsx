import AceEditor from "react-ace";
import { useState, useEffect } from "react";
import "ace-builds/src-noconflict/mode-json";
import "ace-builds/src-noconflict/theme-github";
import { notify } from "../../components/common/Notify";
import ModalWrapper from "../products/modals/ModalWrapper";
import DashboardRenderer from "../dashboard/DashboardRenderer";
import SupplierSelect from "../../components/common/SupplierSelect";
import defaultDashboard from "../../structures/compactDashboard.json";
import { dashboardConfigService } from "../../api/dashboardConfig.service";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";

export default function DashboardConfigPage() {
  const [jsonValue, setJsonValue] = useState("{}");
  const [previewData, setPreviewData] = useState(null);
  const [previewOpen, setPreviewOpen] = useState(false);
  const [selectedSupplier, setSelectedSupplier] = useState<number>(0);

  const queryClient = useQueryClient();

  // 🔹 Hämtar config beroende på vald supplier
  const {
    data: config,
    isPending,
    isError,
  } = useQuery({
    queryKey: ["dashboard-config", selectedSupplier],
    queryFn: async () => await dashboardConfigService.get(selectedSupplier),
    enabled: selectedSupplier !== undefined,
  });

  useEffect(() => {
    if (config) {
      setJsonValue(JSON.stringify(config, null, 2));
    } else {
      setJsonValue("{}");
    }
  }, [config]);

  // 🔹 Mutation för att spara config
  const { mutate: saveConfig, isPending: isSaving } = useMutation({
    mutationFn: ({
      companyId,
      parsed,
    }: {
      companyId: number;
      parsed: object;
    }) => dashboardConfigService.update(companyId, parsed),
    onSuccess: () => {
      notify("Konfiguration sparad!", "success");
      queryClient.invalidateQueries({ queryKey: ["dashboard-config"] });
    },
    onError: () => notify("Kunde inte spara konfigurationen", "error"),
  });

  const handleSave = () => {
    try {
      const parsed = JSON.parse(jsonValue);
      saveConfig({ companyId: selectedSupplier, parsed });
    } catch {
      notify("JSON:et är ogiltigt!", "error");
    }
  };

  const handleReset = () => {
    setJsonValue(JSON.stringify(defaultDashboard, null, 2));
    notify("Återställd till standardlayout!", "success");
  };

  const handlePreview = async () => {
  try {
    const parsed = JSON.parse(jsonValue);
    const rendered = await dashboardConfigService.preview(parsed);
    setPreviewData(rendered);
    setPreviewOpen(true);
  } catch {
    notify("Kunde inte rendera förhandsgranskning", "error");
  }
};

  const handleFetch = async () => {
    try {
      await queryClient.invalidateQueries({
        queryKey: ["dashboard-config", selectedSupplier],
      });
      notify("Konfiguration hämtad!", "success");
    } catch {
      notify("Kunde inte hämta konfigurationen", "error");
    }
  };

  return (
    <div className="p-8 max-w-[1100px] mx-auto">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-semibold text-gray-700">
          Dashboard-konfiguration
        </h1>
        <div className="flex gap-3">
          <button
            onClick={handleReset}
            className="px-3 py-2 bg-gray-200 rounded hover:bg-gray-300 text-gray-800"
          >
            Återställ
          </button>
          <button
            onClick={handlePreview}
            className="px-3 py-2 bg-yellow-500 text-white rounded hover:bg-yellow-600"
          >
            Förhandsgranska
          </button>
          <button
            onClick={handleSave}
            disabled={isSaving}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            {isSaving ? "Sparar..." : "Spara"}
          </button>
        </div>
      </div>

      {/* 🔹 Välj leverantör */}
      <div className="mb-4">
        <label className="block text-sm text-gray-600 mb-1">
          Företag / Leverantör
        </label>
        <SupplierSelect
          value={selectedSupplier ?? undefined}
          onChange={(supplier) =>
            setSelectedSupplier(supplier?.supplierId ?? null)
          }
        />
        <button
          onClick={handleFetch}
          disabled={isPending}
          className="mt-2 px-3 py-2 bg-green-600 text-white rounded hover:bg-green-700"
        >
          {isPending ? "Hämtar..." : "Hämta konfiguration"}
        </button>
      </div>

      {isError ? (
        <div className="text-red-500">Kunde inte hämta konfiguration.</div>
      ) : isPending ? (
        <div className="text-gray-500">Laddar konfiguration...</div>
      ) : (
        <AceEditor
          mode="json"
          theme="github"
          name="dashboard-json-editor"
          width="100%"
          height="600px"
          fontSize={14}
          value={jsonValue}
          onChange={setJsonValue}
          editorProps={{ $blockScrolling: true }}
          setOptions={{
            useWorker: false,
            showLineNumbers: true,
            tabSize: 2,
          }}
        />
      )}

      {/* Förhandsgranskning */}
      {previewOpen && (
        <ModalWrapper
          isOpen={previewOpen}
          onClose={() => setPreviewOpen(false)}
          title="Förhandsgranskning"
        >
          <div className="h-[80vh] overflow-auto bg-gray-50 rounded p-4">
            <DashboardRenderer definition={previewData!} />
          </div>
        </ModalWrapper>
      )}
    </div>
  );
}