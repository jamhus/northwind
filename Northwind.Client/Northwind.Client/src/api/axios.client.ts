import axios from "axios";

const axiosClient = axios.create({
  baseURL: "/api", // proxas till .NET via vite.config.ts
  headers: { "Content-Type": "application/json" },
});

export type PagedResult<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};

export default axiosClient;
    