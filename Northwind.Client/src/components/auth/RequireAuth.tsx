import { Navigate } from "react-router-dom";
import { useAuth } from "../../contexts/auth/useAuth";
import type { JSX } from "react";

export function RequireAuth({ children }: { children: JSX.Element }) {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" />;
  return children;
}

export function RequireRole({
  roles,
  children,
}: {
  roles: string[];
  children: JSX.Element;
}) {
  const { user } = useAuth();
  if (!user || !roles.some((r) => user.roles.includes(r)))
    return <Navigate to="/" />;
  return children;
}
