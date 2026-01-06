import axios, { type AxiosInstance, type AxiosResponse } from 'axios';
import { ZodError } from 'zod';

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

// Response interceptor for error handling
axiosInstance.interceptors.response.use(
  (response) => {
    // If the response is successful, just return it
    return response;
  },
  (error) => {
    // Determine the error message based on the error type
    let errorMessage = 'An unexpected error occurred. Please try again.';

    // Handle Zod validation errors
    if (error instanceof ZodError) {
      errorMessage = 'The server response format is invalid. Please contact support.';
    } else if (error.code === 'ECONNABORTED') {
      // Timeout error
      errorMessage = 'The request took too long. Please check your connection and try again.';
    } else if (error.code === 'ERR_NETWORK' || !error.response) {
      // Network error - backend is not available
      errorMessage = 'Cannot connect to the server. Please make sure the backend is running.';
    } else if (error.response) {
      // Server responded with an error status
      const status = error.response.status;

      if (status >= 500) {
        // Server error
        errorMessage = 'The server encountered an error. Please try again later.';
      } else if (status === 404) {
        // Not found
        errorMessage = 'The requested resource was not found.';
      } else if (status === 400) {
        // Bad request
        errorMessage = 'Invalid request. Please check your input and try again.';
      } else if (status >= 400 && status < 500) {
        // Other client errors
        errorMessage = 'There was a problem with your request. Please try again.';
      }
    }

    // Attach the user-friendly message to the error
    error.userMessage = errorMessage;

    // Reject the promise so the calling code can handle it
    return Promise.reject(error);
  }
);

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
