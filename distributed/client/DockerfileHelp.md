# Common variables

```powershell
$projectName="conways.client"
$version="0.2.0"
```

# Docker Build

```powershell
docker build . -t "${projectName}:${version}"
```

# Docker Buildx

```powershell
docker buildx build --platform linux/amd64,linux/arm64 -t "${projectName}:${version}" .
```

# Docker Run

```powershell
docker run --name $projectName -p 3000:3000 "${projectName}:${version}"
```

# Envirnment variables

According to Vite's documentation:

"Vite exposes env variables on the special import.meta.env object, which are statically replaced at build time"

Since variables are replaced at compile time, there is a specific .env file (.env.docker-local) for docker build that will replace the production .env.