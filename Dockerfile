FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "KanbanBoard.Service.dll"]