using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ECommerceBE.Models;
using ECommerceBE.Database;


namespace AddProductTest;

public class ExampleIntegrationTests : IClassFixture<ApplicationFactory<ECommerceBE.Program>>
{
    ApplicationFactory<ECommerceBE.Program> factory;

    public ExampleIntegrationTests(ApplicationFactory<ECommerceBE.Program> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void AddProduct_WhenCreate_ThenSuccess()
    {
        // given
        var client = factory.CreateClient();
        var productName = "Skirt";

        // when
        var request = await client.PostAsync($"/product?name={productName}", null);
        ECommerceBE.Models.ProductDto? response =
            await request.Content.ReadFromJsonAsync<ECommerceBE.Models.ProductDto>();

        // then
        request.EnsureSuccessStatusCode();
        Assert.NotNull(response);
        Assert.Equal(productName, response.Name);
    }

    [Fact]
    public async void GetAll_WhenEmpty_ThenReturnEmpty()
    {
        // given
        var client = factory.CreateClient();

        // Radera databasen och skapa den igen specifikt för detta test.
        var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ECommerceBE.Database.MyDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // when
        var response = await client.GetFromJsonAsync<List<ECommerceBE.Models.ProductDto>>("/product");

        // then
        Assert.NotNull(response);
        Assert.Empty(response);
    }
}