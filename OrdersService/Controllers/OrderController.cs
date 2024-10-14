using System.Net.Mime;
using System.Text;
using System.Text.Json;

using Bogus;

using DataAccess;
using DataAccess.Entities;

using Microsoft.AspNetCore.Mvc;

namespace OrdersService.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController(
    ILogger<OrderController> logger,
    SalesDbContext db,
    IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpPost("place-order")]
    public async Task<ActionResult<Product[]>> PlaceOrder()
    {
        // generate random products
        var products = new Faker<Product>()
            .RuleFor(x => x.Name, x => x.Commerce.ProductName())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
            .RuleFor(x => x.Price, x => x.Random.Decimal(100, 1_000))
            .Generate(10);

        // persist into database
        logger.LogInformation("Saving order to database: {Products}", JsonSerializer.Serialize(products));
        await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync();


        // Call payment service
        logger.LogInformation("Calling Payment Service");
        var httpClient = httpClientFactory.CreateClient("PaymentService");
        await httpClient.PostAsync(
            "/api/payment/process",
            new StringContent(
                JsonSerializer.Serialize(products),
                Encoding.UTF8,
                MediaTypeNames.Application.Json));

        return Ok(products);
    }
}