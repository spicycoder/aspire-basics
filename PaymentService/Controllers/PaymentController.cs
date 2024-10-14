using System.Text.Json;

using DataAccess.Entities;

using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController(ILogger<PaymentController> logger) : ControllerBase
{
    [HttpPost("process")]
    public async Task<ActionResult<Product[]>> Process(Product[] products)
    {
        logger.LogInformation("Received products: {Products}", JsonSerializer.Serialize(products));
        return Ok(products);
    }
}