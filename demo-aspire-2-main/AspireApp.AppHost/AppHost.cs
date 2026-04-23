var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", "test123456", secret: true);

var postgres = builder.AddPostgres("postgres", password: postgresPassword, port: 6001);
var weatherDb     = postgres.AddDatabase("weatherdb");
var weatherReadDb = postgres.AddDatabase("weatherdb-read");

var redis = builder.AddRedis("redis");

var weatherApi = builder.AddProject<Projects.Weather_API>("weather-api")
    .WithHttpHealthCheck("/health")
    .WithReference(weatherDb)       // ← inject ConnectionStrings__weatherdb
    .WithReference(weatherReadDb)   // ← inject ConnectionStrings__weatherdb-read
    .WithReference(redis)           // ← inject ConnectionStrings__redis tự động
    .WaitFor(postgres)
    .WaitFor(redis);

builder.AddProject<Projects.AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(weatherApi)
    .WaitFor(weatherApi);

builder.Build().Run();
