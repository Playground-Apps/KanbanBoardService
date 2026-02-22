FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

WORKDIR /app
COPY ./publish .

# Build-time argument (default value)
ARG BASE_PATH="/api/delivery_tracking/"
# Optional: also available at runtime
ENV BASE_PATH=${BASE_PATH}

# Traefik labels
LABEL traefik.enable=true \
      traefik.http.routers.delivery_tracking.tls=true \
      traefik.http.routers.delivery_tracking.rule="PathPrefix(\`${BASE_PATH}\`)" \
      traefik.http.routers.delivery_tracking.entrypoints="websecure" \
      traefik.http.routers.backend.rule="Host(`kanban-board.com`)" \
      traefik.http.routers.delivery_tracking.priority="1000" \
      traefik.http.services.delivery_tracking.loadbalancer.server.port="8080" \
      traefik.http.middlewares.delivery_tracking-strip.stripprefix.prefixes="${BASE_PATH}" \
      traefik.http.routers.delivery_tracking.middlewares="delivery_tracking-strip"

ENTRYPOINT ["dotnet", "KanbanBoard.Service.dll"]
