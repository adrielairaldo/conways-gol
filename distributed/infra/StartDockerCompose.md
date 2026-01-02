# Run Docker Compose

First, it is required to create the volumes, and then we can execute docker compose.

```powershell
docker volume create mongodbdata
docker volume create mongodbconfig
docker-compose up
```