import AceEditor from "react-ace";
import { useState, useEffect } from "react";
import "ace-builds/src-noconflict/mode-json";
import "ace-builds/src-noconflict/theme-github";
import { notify } from "../../components/common/Notify";
import ModalWrapper from "../products/modals/ModalWrapper";
import { useQuery, useMutation } from "@tanstack/react-query";
import DashboardRenderer from "../dashboard/DashboardRenderer";
import defaultDashboard from "../../structures/defaultDashboard.json";
import { dashboardConfigService } from "../../api/dashboardConfig.service";

export default function DashboardConfigPage() {
  const [jsonValue, setJsonValue] = useState("{}");
  const [previewOpen, setPreviewOpen] = useState(false);

  const { data, isLoading, refetch } = useQuery({
    queryKey: ["dashboard-config"],
    queryFn: dashboardConfigService.get,
  });

  useEffect(() => {
    if (data) {
      setJsonValue(JSON.stringify(data, null, 2));
    }
  }, [data]);

  const { mutate: saveConfig, isPending } = useMutation({
    mutationFn: dashboardConfigService.update,
    onSuccess: () => {
      notify("Konfiguration sparad!", "success");
      refetch();
    },
    onError: () => notify("Kunde inte spara konfigurationen", "error"),
  });

  const handleSave = () => {
    try {
      const parsed = JSON.parse(jsonValue);
      saveConfig(parsed);
    } catch {
      notify("JSON:et är ogiltigt!", "error");
    }
  };

  const handleReset = () => {
    setJsonValue(JSON.stringify(defaultDashboard, null, 2));
    notify("Återställd till standardlayout!", "success");
  };

  const handlePreview = () => {
    try {
      JSON.parse(jsonValue);
      setPreviewOpen(true);
    } catch {
      notify("Ogiltig JSON – kan inte förhandsgranska!", "error");
    }
  };

  return (
    <div className="p-8 max-w-[1100px] mx-auto">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-semibold text-gray-700">
          Dashboard-konfiguration
        </h1>
        <div className="flex items-center gap-3">
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
            disabled={isPending}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            {isPending ? "Sparar..." : "Spara"}
          </button>
        </div>
      </div>

      {isLoading ? (
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

      {/* 🔹 Förhandsgranskning */}
      {previewOpen && (
        <ModalWrapper
          isOpen={previewOpen}
          onClose={() => setPreviewOpen(false)}
          title="Förhandsgranskning"
        >
          <div className="h-[80vh] overflow-auto bg-gray-50 rounded p-4">
            <DashboardRenderer
              definition={JSON.parse(jsonValue)}
            />
          </div>
        </ModalWrapper>
      )}
    </div>
  );
}
