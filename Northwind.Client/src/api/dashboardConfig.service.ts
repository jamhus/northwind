import api from "./axios.client";

export const dashboardConfigService = {
  get: async () => {
    const res = await api.get("/dashboardConfig");
    return res.data;
  },
  update: async (json: object) => {
    const res = await api.put("/dashboardConfig", json);
    return res.data;
  },
};
