import { NavLink } from "react-router-dom";
import type { DashboardDefinition, Page } from "../../api/dashboard.service";

type Props = { definition: DashboardDefinition };

export default function DashboardNav({ definition }: Props) {
  const pages = definition.pages;
  return (
    <nav className="bg-white rounded shadow-sm p-3">
      <ul className="space-y-1">
        {pages.map((p: Page) => (
          <li key={p.key}>
            <NavLink
              to={`/dashboard/${p.key}`}
              className={({ isActive }) =>
                `block px-3 py-2 rounded hover:bg-gray-100 ${
                  isActive ? "bg-blue-600 text-white font-medium" : ""
                }`
              }
            >
              {p.name?.find((n: { language: string; text: string }) => n.language === "sv")?.text ?? p.key}
            </NavLink>
          </li>
        ))}
      </ul>
    </nav>
  );
}
