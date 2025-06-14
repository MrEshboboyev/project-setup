using Contracts.Data;
using Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
internal sealed class ProductsController(
    AppDbContext dbContext,
    ILogger<ProductsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
    {
        List<Product> products = await dbContext.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetByIdAsync(int id)
    {
        Product? product = await dbContext.Products.FindAsync(id);

        if (product is not null)
        {
            return Ok(product);
        }

        logger.LogWarning("Product with ID {Id} not found.", id);
        return NotFound();

    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateAsync([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        product.CreatedAt = DateTime.UtcNow;

        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] Product updated)
    {
        if (id != updated.Id)
        {
            return BadRequest("ID mismatch.");
        }

        Product? product = await dbContext.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        product.Name = updated.Name;
        product.Description = updated.Description;
        product.Price = updated.Price;

        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        Product? product = await dbContext.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
