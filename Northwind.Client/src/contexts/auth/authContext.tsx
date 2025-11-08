import { createContext } from "react";

export type User = {
  email: string;
  roles: string[];
  supplierId?: number;
  employeeId?: number;
};

export type AuthContextType = {
  user: User | null;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
};

export const AuthContext = createContext<AuthContextType | undefined>(undefined);
