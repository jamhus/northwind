import type { JSX } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../../contexts/auth/useAuth";

type Props = {
  roles: string[];
  children: JSX.Element;
};

export function RequireRole({ roles, children }: Props) {
  const { user } = useAuth();

  if (!user || !roles.some((r) => user.roles.includes(r))) {
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
}
