var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var postgresdb = postgres.AddDatabase("postgresdb");

var migration = builder.AddProject<Projects.PersonalWeb_MigrationService>("migrations")
    .WithReference(postgresdb, connectionName: "Default")
    .WithExplicitStart();

var webApi = builder.AddProject<Projects.PersonalWeb_Api>("apiservice")
    .WithHttpEndpoint(name: "apiservice")
    .WithReference(postgresdb, connectionName: "Default")
    .WaitFor(postgresdb)
    .WaitForCompletion(migration);

builder.AddNpmApp("admin", "../PersonalWeb.Admin")
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();