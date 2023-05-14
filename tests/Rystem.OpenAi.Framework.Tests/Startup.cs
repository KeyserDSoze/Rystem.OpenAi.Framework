﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Rystem.OpenAi.Framework.Tests
{
    public class Startup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2094:Classes should not be empty", Justification = "It's necessary to inject secrets in Dependency injection settings.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Test purposes.")]
        private sealed class ForUserSecrets { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "It's necessary to have this method as a non-static method because the dependency injection package needs a non-static method.")]
        public void ConfigureHost(IHostBuilder hostBuilder) =>
        hostBuilder
            .ConfigureHostConfiguration(builder => { })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("appsettings.test.json")
               .AddUserSecrets<ForUserSecrets>()
               .AddEnvironmentVariables();
            });
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "It's necessary to have this method as a non-static method because the dependency injection package needs a non-static method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2012:Use ValueTasks correctly", Justification = "Test purposes.")]
        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            var apiKey = Environment.GetEnvironmentVariable("OpenAiApiKey") ?? context.Configuration["OpenAi:ApiKey"];
            var azureApiKey = Environment.GetEnvironmentVariable("AzureApiKey") ?? context.Configuration["Azure:ApiKey"];
            var resourceName = Environment.GetEnvironmentVariable("AzureResourceName") ?? context.Configuration["Azure:ResourceName"];
            var clientId = Environment.GetEnvironmentVariable("AzureADClientId") ?? context.Configuration["AzureAd:ClientId"];
            var clientSecret = Environment.GetEnvironmentVariable("AzureADClientSecret") ?? context.Configuration["AzureAd:ClientSecret"];
            var tenantId = Environment.GetEnvironmentVariable("AzureADTenantId") ?? context.Configuration["AzureAd:TenantId"];
            var azureApiKey2 = Environment.GetEnvironmentVariable("Azure2ApiKey") ?? context.Configuration["Azure2:ApiKey"];
            var resourceName2 = Environment.GetEnvironmentVariable("Azure2ResourceName") ?? context.Configuration["Azure2:ResourceName"];
            services
                .AddOpenAiFrameworkWithDefaultActions(settings =>
                {
                    settings.ApiKey = apiKey;
                },
                builder =>
                {
                });
            services.AddOpenAiFrameworkWithDefaultActions(settings =>
            {
                settings.ApiKey = azureApiKey;
                settings
                    .UseVersionForChat("2023-03-15-preview");
                settings.Azure.ResourceName = resourceName;
                settings.Azure.AppRegistration.ClientId = clientId;
                settings.Azure.AppRegistration.ClientSecret = clientSecret;
                settings.Azure.AppRegistration.TenantId = tenantId;
                settings.Azure
                    .MapDeploymentTextModel("text-curie-001", TextModelType.CurieText)
                    .MapDeploymentTextModel("text-davinci-003", TextModelType.DavinciText3)
                    .MapDeploymentEmbeddingModel("OpenAiDemoModel", EmbeddingModelType.AdaTextEmbedding)
                    .MapDeploymentChatModel("gpt35turbo", ChatModelType.Gpt35Turbo0301)
                    .MapDeploymentCustomModel("ada001", "text-ada-001");
                settings.Price
                    .SetFineTuneForAda(0.0004M, 0.0016M)
                    .SetAudioForTranslation(0.006M);
            },
            builder =>
            {
            },
            "Azure");
            var tasks = new List<ValueTask<List<AutomaticallyDeploymentResult>>>
            {
                services.BuildServiceProvider()
                .MapOpenAiFrameworkActions()
                .MapDeploymentsAutomaticallyAsync(true, "Azure")
            };
            var results = Task.WhenAll(tasks.Select(x => x.AsTask())).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotEmpty(results);
        }
    }
}
