import { type ReactNode, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { authService } from "../../api/auth.service";
import { AuthContext, type User } from "./authContext";

type DecodedToken = {
  email: string;
  Role: string;
  SupplierId?: string;
  EmployeeId?: string;
  exp: number;
};

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = authService.getToken();
    if (token) {
      try {
        const decoded = jwtDecode<
          DecodedToken & {
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: string;
          }
        >(token);

        const roles =
          decoded.Role ??
          decoded[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
          ] ??
          "";
        const roleList = roles.split(",");

        setUser({
          email: decoded.email,
          roles: roleList,
          supplierId: decoded.SupplierId
            ? parseInt(decoded.SupplierId)
            : undefined,
          employeeId: decoded.EmployeeId
            ? parseInt(decoded.EmployeeId)
            : undefined,
        });
      } catch {
        authService.logout();
      }
    }
    setLoading(false);
  }, []);

  if (loading) return <div className="p-8 text-center">Laddar...</div>;

  async function login(email: string, password: string) {
    const res = await authService.login({ email, password });
    authService.saveToken(res.token);
    setUser({
      email: res.email,
      roles: res.roles,
      supplierId: res.supplierId,
      employeeId: res.employeeId,
    });
  }

  function logout() {
    authService.logout();
    setUser(null);
  }

  return (
    <AuthContext.Provider
      value={{ user, isAuthenticated: !!user, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
}
