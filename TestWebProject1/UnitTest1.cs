using Microsoft.AspNetCore.Mvc.Testing;
using Serilog;

namespace TestWebProject1;

public class UnitTest1  :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program>
        _factory;

    public UnitTest1(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Test1()
    {
        var message = await _client.GetAsync("/hello");
        var text = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
}

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Serilog:MinimumLevel:Default", "Debug"},
            });
        });

    builder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
            configuration.WriteTo.Debug();
        });
        return base.CreateHost(builder);
    }
}