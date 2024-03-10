1. Run all services and Redis.
```
docker compose up -d --build
```
2. Call http://localhost:8088/track.
3. See that logs/visits.log file was updated.