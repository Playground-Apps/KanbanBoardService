FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine
WORKDIR /app
COPY /publish .
USER $APP_UID
ENTRYPOINT ["dotnet KanbanBoard.Service.dll"]