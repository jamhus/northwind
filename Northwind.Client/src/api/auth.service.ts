import api from "./axios.client";

export type LoginRequest = {
  email: string;
  password: string;
};

export type LoginResponse = {
  token: string;
  email: string;
  roles: string[];
  supplierId?: number;
  employeeId?: number;
};

export const authService = {
  async login(data: LoginRequest): Promise<LoginResponse> {
    const res = await api.post<LoginResponse>("/auth/login", data);
    return res.data;
  },

  logout() {
    localStorage.removeItem("token");
  },

  getToken(): string | null {
    return localStorage.getItem("token");
  },

  saveToken(token: string) {
    localStorage.setItem("token", token);
  },
};
