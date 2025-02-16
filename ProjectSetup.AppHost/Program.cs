IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres");

builder.AddProject<Projects.Web_Api>("web-api")
    .WithReference(postgres)
    .WaitFor(postgres);

await builder.Build().RunAsync();
