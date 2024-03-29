using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceBE.Models;


[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService productService;

    public ProductController(ProductService productService)
    {
        this.productService = productService;
    }

    [HttpPost("AddProduct")]
    [Authorize]
    public IActionResult AddProduct(ProductDto productDto)
    {
        try
        {
            productService.AddProduct(productDto);
            return Ok("Product added to cart successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("UpdateProduct/{productId}")]
    [Authorize]
    public IActionResult UpdateProduct(int productId, Product product)
    {

        try
        {
            productService.UpdateProduct(productId, product);
            return Ok("Product updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("DeleteProduct/{productId}")]
    [Authorize]
    public IActionResult DeleteProduct(int productId)
    {
        try
        {
            productService.DeleteProduct(productId);
            return Ok("Product removed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}