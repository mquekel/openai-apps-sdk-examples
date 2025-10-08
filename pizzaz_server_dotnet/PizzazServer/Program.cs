using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.Text.Json;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add MCP server with HTTP transport
builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new Implementation
        {
            Name = "pizzaz-dotnet",
            Version = "1.0.0"
        };

        options.Handlers = new McpServerHandlers
        {
            ListToolsHandler = PizzazHandlers.ListToolsAsync,
            ListResourcesHandler = PizzazHandlers.ListResourcesAsync,
            ListResourceTemplatesHandler = PizzazHandlers.ListResourceTemplatesAsync,
            ReadResourceHandler = PizzazHandlers.ReadResourceAsync,
            CallToolHandler = PizzazHandlers.CallToolAsync
        };
    })
    .WithHttpTransport();

var app = builder.Build();
app.UseCors();

// Map MCP endpoints
app.MapMcp("/mcp");

// Configure port from environment or default to 8000
var port = Environment.GetEnvironmentVariable("PORT");
var portNumber = int.TryParse(port, out var p) ? p : 8000;

app.Urls.Clear();
app.Urls.Add($"http://0.0.0.0:{portNumber}");

Console.WriteLine($"Pizzaz MCP server starting on http://0.0.0.0:{portNumber}");
Console.WriteLine("SSE endpoint: /mcp");
Console.WriteLine("Messages endpoint: /mcp/messages");

app.Run();

// Models and Handlers
public record PizzazWidget(
    string Identifier,
    string Title,
    string TemplateUri,
    string Invoking,
    string Invoked,
    string Html,
    string ResponseText
);

public static class PizzazHandlers
{
    private const string MimeType = "text/html+skybridge";

    private static readonly List<PizzazWidget> Widgets =
    [
        new PizzazWidget(
            Identifier: "pizza-map",
            Title: "Show Pizza Map",
            TemplateUri: "ui://widget/pizza-map.html",
            Invoking: "Hand-tossing a map",
            Invoked: "Served a fresh map",
            Html: """
                <div id="pizzaz-root"></div>
                <link rel="stylesheet" href="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-0038.css">
                <script type="module" src="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-0038.js"></script>
                """,
            ResponseText: "Rendered a pizza map!"
        ),
        new PizzazWidget(
            Identifier: "pizza-carousel",
            Title: "Show Pizza Carousel",
            TemplateUri: "ui://widget/pizza-carousel.html",
            Invoking: "Carousel some spots",
            Invoked: "Served a fresh carousel",
            Html: """
                <div id="pizzaz-carousel-root"></div>
                <link rel="stylesheet" href="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-carousel-0038.css">
                <script type="module" src="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-carousel-0038.js"></script>
                """,
            ResponseText: "Rendered a pizza carousel!"
        ),
        new PizzazWidget(
            Identifier: "pizza-albums",
            Title: "Show Pizza Album",
            TemplateUri: "ui://widget/pizza-albums.html",
            Invoking: "Hand-tossing an album",
            Invoked: "Served a fresh album",
            Html: """
                <div id="pizzaz-albums-root"></div>
                <link rel="stylesheet" href="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-albums-0038.css">
                <script type="module" src="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-albums-0038.js"></script>
                """,
            ResponseText: "Rendered a pizza album!"
        ),
        new PizzazWidget(
            Identifier: "pizza-list",
            Title: "Show Pizza List",
            TemplateUri: "ui://widget/pizza-list.html",
            Invoking: "Hand-tossing a list",
            Invoked: "Served a fresh list",
            Html: """
                <div id="pizzaz-list-root"></div>
                <link rel="stylesheet" href="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-list-0038.css">
                <script type="module" src="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-list-0038.js"></script>
                """,
            ResponseText: "Rendered a pizza list!"
        ),
        new PizzazWidget(
            Identifier: "pizza-video",
            Title: "Show Pizza Video",
            TemplateUri: "ui://widget/pizza-video.html",
            Invoking: "Hand-tossing a video",
            Invoked: "Served a fresh video",
            Html: """
                <div id="pizzaz-video-root"></div>
                <link rel="stylesheet" href="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-video-0038.css">
                <script type="module" src="https://persistent.oaistatic.com/ecosystem-built-assets/pizzaz-video-0038.js"></script>
                """,
            ResponseText: "Rendered a pizza video!"
        )
    ];

    private static readonly Dictionary<string, PizzazWidget> WidgetsById =
        Widgets.ToDictionary(w => w.Identifier);

    private static readonly Dictionary<string, PizzazWidget> WidgetsByUri =
        Widgets.ToDictionary(w => w.TemplateUri);

    private static readonly JsonElement ToolInputSchema = JsonSerializer.Deserialize<JsonElement>("""
        {
            "type": "object",
            "properties": {
                "pizzaTopping": {
                    "type": "string",
                    "description": "Topping to mention when rendering the widget."
                }
            },
            "required": ["pizzaTopping"],
            "additionalProperties": false
        }
        """);

    public static ValueTask<ListToolsResult> ListToolsAsync(
        RequestContext<ListToolsRequestParams> request,
        CancellationToken cancellationToken)
    {
        var tools = Widgets.Select(widget => new Tool
        {
            Name = widget.Identifier,
            Description = widget.Title,
            InputSchema = ToolInputSchema,
            Meta = CreateToolMeta(widget)
        }).ToList();

        return ValueTask.FromResult(new ListToolsResult { Tools = tools });
    }

    public static ValueTask<ListResourcesResult> ListResourcesAsync(
        RequestContext<ListResourcesRequestParams> request,
        CancellationToken cancellationToken)
    {
        var resources = Widgets.Select(widget => new Resource
        {
            Uri = widget.TemplateUri,
            Name = widget.Title,
            Description = $"{widget.Title} widget markup",
            MimeType = MimeType,
            Meta = CreateToolMeta(widget)
        }).ToList();

        return ValueTask.FromResult(new ListResourcesResult { Resources = resources });
    }

    public static ValueTask<ListResourceTemplatesResult> ListResourceTemplatesAsync(
        RequestContext<ListResourceTemplatesRequestParams> request,
        CancellationToken cancellationToken)
    {
        var templates = Widgets.Select(widget => new ResourceTemplate
        {
            UriTemplate = widget.TemplateUri,
            Name = widget.Title,
            Description = $"{widget.Title} widget markup",
            MimeType = MimeType,
            Meta = CreateToolMeta(widget)
        }).ToList();

        return ValueTask.FromResult(new ListResourceTemplatesResult { ResourceTemplates = templates });
    }

    public static ValueTask<ReadResourceResult> ReadResourceAsync(
        RequestContext<ReadResourceRequestParams> request,
        CancellationToken cancellationToken)
    {
        if (!WidgetsByUri.TryGetValue(request.Params.Uri ?? "", out var widget))
        {
            return ValueTask.FromResult(new ReadResourceResult
            {
                Contents = [],
                Meta = JsonSerializer.SerializeToNode(new { error = $"Unknown resource: {request.Params.Uri}" }) as JsonObject
            });
        }

        var contents = new List<ResourceContents>
        {
            new TextResourceContents
            {
                Uri = widget.TemplateUri,
                MimeType = MimeType,
                Text = widget.Html,
                Meta = CreateToolMeta(widget)
            }
        };

        return ValueTask.FromResult(new ReadResourceResult { Contents = contents });
    }

    public static ValueTask<CallToolResult> CallToolAsync(
        RequestContext<CallToolRequestParams> request,
        CancellationToken cancellationToken)
    {
        if (!WidgetsById.TryGetValue(request.Params.Name ?? "", out var widget))
        {
            return ValueTask.FromResult(new CallToolResult
            {
                Content =
                [
                    new TextContentBlock { Text = $"Unknown tool: {request.Params.Name}" }
                ],
                IsError = true
            });
        }

        // Parse arguments
        if (request.Params.Arguments?.TryGetValue("pizzaTopping", out var toppingElement) != true)
        {
            return ValueTask.FromResult(new CallToolResult
            {
                Content =
                [
                    new TextContentBlock { Text = "Missing required argument: pizzaTopping" }
                ],
                IsError = true
            });
        }

        var topping = toppingElement.GetString() ?? "";
        var widgetResource = CreateEmbeddedWidgetResource(widget);

        var meta = new JsonObject
        {
            ["openai.com/widget"] = JsonSerializer.SerializeToNode(widgetResource),
            ["openai/outputTemplate"] = widget.TemplateUri,
            ["openai/toolInvocation/invoking"] = widget.Invoking,
            ["openai/toolInvocation/invoked"] = widget.Invoked,
            ["openai/widgetAccessible"] = true,
            ["openai/resultCanProduceWidget"] = true
        };

        return ValueTask.FromResult(new CallToolResult
        {
            Content =
            [
                new TextContentBlock { Text = widget.ResponseText }
            ],
            Meta = meta
        });
    }

    private static JsonObject CreateToolMeta(PizzazWidget widget)
    {
        return new JsonObject
        {
            ["openai/outputTemplate"] = widget.TemplateUri,
            ["openai/toolInvocation/invoking"] = widget.Invoking,
            ["openai/toolInvocation/invoked"] = widget.Invoked,
            ["openai/widgetAccessible"] = true,
            ["openai/resultCanProduceWidget"] = true,
            ["annotations"] = new JsonObject
            {
                ["destructiveHint"] = false,
                ["openWorldHint"] = false,
                ["readOnlyHint"] = true
            }
        };
    }

    private static object CreateEmbeddedWidgetResource(PizzazWidget widget)
    {
        return new
        {
            type = "resource",
            resource = new
            {
                uri = widget.TemplateUri,
                mimeType = MimeType,
                text = widget.Html,
                title = widget.Title
            }
        };
    }
}
