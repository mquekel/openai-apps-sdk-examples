# Pizzaz MCP server (.NET)

This directory packages a .NET implementation of the Pizzaz demo server using the official Model Context Protocol SDK for C#. It mirrors the Python and Node examples and exposes each pizza widget as both a resource and a tool.

## Prerequisites

- .NET 8.0+ SDK
- The ModelContextProtocol.AspNetCore NuGet package (prerelease)

## Installation

Navigate to the PizzazServer directory:

```bash
cd pizzaz_server_dotnet/PizzazServer
```

Restore dependencies:

```bash
dotnet restore
```

> **Note:** This project uses the official `ModelContextProtocol.AspNetCore` package which is currently in preview. The package provides HTTP/SSE transport capabilities for MCP servers built with ASP.NET Core.

## Run the server

```bash
dotnet run
```

This boots an ASP.NET Core app with Kestrel on `http://0.0.0.0:8000`. The endpoints mirror the Node and Python demos:

- `GET /mcp` exposes the SSE stream.
- `POST /mcp/messages?sessionId=...` accepts follow-up messages for an active session.

Cross-origin requests are allowed so you can drive the server from local tooling or the MCP Inspector. Each tool returns structured content that echoes the requested topping plus metadata that points to the correct Skybridge widget shell, matching the original Pizzaz documentation.

## Project Structure

The project uses a minimal ASP.NET Core web application structure:

- **Program.cs** - Contains all the MCP server logic including:
  - Widget definitions (5 pizza widgets: map, carousel, albums, list, video)
  - MCP request handlers for tools, resources, and resource templates
  - HTTP/SSE transport configuration
  - CORS configuration for cross-origin requests

## Implementation Details

This .NET implementation demonstrates:

1. **MCP Server Configuration** - Using `AddMcpServer()` and `WithHttpTransport()` to configure the server with dependency injection
2. **Handler Registration** - Implementing handlers for:
   - `ListToolsAsync` - Lists available tools
   - `ListResourcesAsync` - Lists available resources
   - `ListResourceTemplatesAsync` - Lists available resource templates
   - `ReadResourceAsync` - Returns widget HTML content
   - `CallToolAsync` - Executes tools and returns results with metadata
3. **Widget Metadata** - Proper serialization of OpenAI-specific metadata for widget rendering
4. **Type Safety** - Leveraging C# records and strong typing for widget definitions

## Next steps

Use these handlers as a starting point when wiring in real data, authentication, or localization support. The structure demonstrates how to:

1. Register reusable UI resources that load static HTML bundles.
2. Associate tools with those widgets via `Meta["openai/outputTemplate"]`.
3. Ship structured JSON alongside human-readable confirmation text.
4. Use ASP.NET Core's built-in dependency injection and configuration systems.

## Building and Publishing

To build the project for production:

```bash
dotnet build -c Release
```

To publish a self-contained executable:

```bash
dotnet publish -c Release -o ./publish
```

## Environment Variables

- `PORT` - Override the default port (8000)
- `ASPNETCORE_ENVIRONMENT` - Set the environment (Development, Staging, Production)
