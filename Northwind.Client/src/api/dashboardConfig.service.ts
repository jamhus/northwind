import api from "./axios.client";

// 📁 src/api/dashboardConfig.service.ts
export const dashboardConfigService = {
  get: async (companyId:number) => {
    const res = await api.get(`/dashboardConfig/${companyId}`);
    return res.data;
  },
  update: async (companyId:number, json: object) => {
    const res = await api.put("/dashboardConfig", { companyId, json: JSON.stringify(json) });
    return res.data;
  },
  preview: async (json: object) => {
    const res = await api.post("/dashboardConfig/preview", json);
    return res.data;
  },
};
