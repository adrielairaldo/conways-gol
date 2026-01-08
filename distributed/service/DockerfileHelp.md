# Getting Started
Instructions for configuring, creating and running the Conways.Service Docker image.

## Common variables
```powershell
$projectName="conways.service"
$version="0.2.0"
```

# Docker Buildx

```powershell
docker buildx build --platform linux/amd64,linux/arm64 -t "${projectName}:${version}" .
```

# Docker Build
```powershell
docker build . -t "${projectName}:${version}"
```

# Docker Run

```powershell
docker run --name $projectName -p 5000:5000 "${projectName}:${version}"
```