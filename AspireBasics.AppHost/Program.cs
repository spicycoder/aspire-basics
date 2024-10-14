var builder = DistributedApplication.CreateBuilder(args);

var sqlDB = builder.AddSqlServer("sales-db")
    .AddDatabase("SalesDbConnection");

var paymentService = builder.AddProject<Projects.PaymentService>("paymentservice");

builder.AddProject<Projects.OrdersService>("ordersservice")
    .WithReference(sqlDB)
    .WithReference(paymentService);

builder.Build().Run();
