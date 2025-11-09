import { Navigate } from "react-router-dom";
import { useAuth } from "../../contexts/auth/useAuth";

type Props = {
  children: React.ReactNode;
  roles: string[];
};

export default function ProtectedRoute({ children, roles }: Props) {
  const { isAuthenticated, user } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  if (!user || !roles.some((r) => user.roles.includes(r))) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <>{children}</>;
}
