# Adding a Response Delay Interceptor (Development Only)

This interceptor introduces an artificial delay to all Axios responses.  
It is useful for testing loading states, skeletons, and UI behavior under slow network conditions.

## Where to place it
Insert the following code **immediately after** creating the Axios instance inside `httpClient.ts`:

```ts
// Utility to simulate network latency
const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  })
}

// Interceptor to delay all responses
axiosInstance.interceptors.response.use(async response => {
  async response => {
    await sleep(2000); // adjust delay as needed
    return response;
  } catch (error) {
    console.log(error);
    return await Promise.reject(error)
  }
});
```