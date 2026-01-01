import axios, { type AxiosInstance, type AxiosResponse } from "axios";


const axiosInstance: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// const sleep = (delay: number) => {
//   return new Promise((resolve) => {
//     setTimeout(resolve, delay);
//   })
// }

// axiosInstance.interceptors.response.use(async response => {
//   try {
//     await sleep(2000);
//     return response;
//   } catch (error) {
//     console.log(error);
//     return await Promise.reject(error)
//   }
// })

// Extract only the data
const getResponseBody = <T>(response: AxiosResponse<T>) => response.data;

// Declarative HTTP verbs
export const httpClient = {
  get: <T>(url: string) => axiosInstance.get<T>(url).then(getResponseBody),
  post: <T>(url: string, data?: any, config?: any) => axiosInstance.post<T>(url, data, config).then(getResponseBody),
  put: <T>(url: string, data?: any, config?: any) => axiosInstance.put<T>(url, data, config).then(getResponseBody),
  patch: <T>(url: string, data?: any, config?: any) => axiosInstance.patch<T>(url, data, config).then(getResponseBody),
  del: <T>(url: string) => axiosInstance.delete<T>(url).then(getResponseBody),
};
