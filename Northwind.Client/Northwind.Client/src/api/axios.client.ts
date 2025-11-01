import axios from "axios";

const axiosClient = axios.create({
  baseURL: "/api", // proxas till .NET via vite.config.ts
  headers: { "Content-Type": "application/json" },
});

export default axiosClient;
    