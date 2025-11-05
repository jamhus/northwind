import toast, { type Toast } from "react-hot-toast";
import {
  CheckCircle2,
  XCircle,
  Info,
  AlertTriangle,
  X,
} from "lucide-react";
import type { JSX } from "react";

type ToastType = "success" | "error" | "info" | "warning";

export const notify = (
  message: string,
  type: ToastType = "info",
  duration = 3000
) => {
  toast.custom((t: Toast) => {
    const base =
      "flex items-start gap-3 p-4 rounded-xl shadow-lg border backdrop-blur-md transition-all duration-300";
    const isVisible = t.visible ? "opacity-100 translate-y-0" : "opacity-0 -translate-y-2";

    const styles: Record<ToastType, string> = {
      success: "bg-green-50/90 border-green-300 text-green-800",
      error: "bg-red-50/90 border-red-300 text-red-800",
      info: "bg-blue-50/90 border-blue-300 text-blue-800",
      warning: "bg-amber-50/90 border-amber-300 text-amber-800",
    };

    const icons: Record<ToastType, JSX.Element> = {
      success: <CheckCircle2 className="text-green-500" size={22} />,
      error: <XCircle className="text-red-500" size={22} />,
      info: <Info className="text-blue-500" size={22} />,
      warning: <AlertTriangle className="text-amber-500" size={22} />,
    };

    return (
      <div
        className={`${base} ${styles[type]} ${isVisible}`}
        onClick={() => toast.dismiss(t.id)}
      >
        <div>{icons[type]}</div>
        <div className="flex-1 text-sm leading-tight">{message}</div>
        <button className="text-gray-400 hover:text-gray-600">
          <X size={16} />
        </button>
      </div>
    );
  }, { duration });
};
