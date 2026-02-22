FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

WORKDIR /app
COPY ./publish .

# Build-time argument (default value)
ARG BASE_PATH="/api/kaban_board/"
# Optional: also available at runtime
ENV BASE_PATH=${BASE_PATH}

# Traefik labels
LABEL traefik.enable=true \
      traefik.http.routers.kaban_board.tls=true \
      traefik.http.routers.backend.rule="Host(`kanban-board.com`) && PathPrefix(`/api`)" \
      traefik.http.routers.kaban_board.entrypoints="websecure" \
      traefik.http.routers.kaban_board.priority="1000" \
      traefik.http.services.kaban_board.loadbalancer.server.port="8080" \
      traefik.http.middlewares.kaban_board-strip.stripprefix.prefixes="${BASE_PATH}" \
      traefik.http.routers.kaban_board.middlewares="kaban_board-strip"

ENTRYPOINT ["dotnet", "KanbanBoard.Service.dll"]
