import { Routes, Route, Navigate } from "react-router-dom";
import type { DashboardDefinition, Page } from "../api/dashboard.service";
import PageRenderer from "../pages/dashboard/PageRenderer";

type Props = { definition: DashboardDefinition };

export default function DashboardRoutes({ definition }: Props) {
  const pages = definition.pages;
  const firstPageKey = pages[0]?.key || "mainDashboard";

  return (
    <Routes>
      <Route index element={<Navigate to={firstPageKey} replace />} />
      {pages.map((p: Page) => (
        <Route
          key={p.key}
          path={p.key}
          element={ <PageRenderer page={p} />}
        />
      ))}
    </Routes>
  );
}
