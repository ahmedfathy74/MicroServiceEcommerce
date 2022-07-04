using CatalogApi.Entities;
using CatalogApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {

        private readonly IProductRepository productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GETAll()
        {
            var products =await productRepository.GetProducts();
            return  Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "Get Product")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]

        public async Task<ActionResult> GetbyId(string id)
        {
            // if (id == null) return NotFound();
            var product = await productRepository.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"product with {id} not found");
                return NotFound();
            }
            else
            {
                return Ok(product);
            }

        }

        [Route("[action]/{category}")]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GETProductByCatagoryName(string category)
        {
            var products = await productRepository.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]

        public async Task<ActionResult> Create([FromBody]Product product)
        {
             await productRepository.Create(product);
            //  return CreatedAtRoute("Get Product",new {id=product.Id,product });
            return NoContent();
        }


        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]

        public async Task<ActionResult> update([FromBody] Product product)
        {

            return Ok(await productRepository.Update(product));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]

        public async Task<ActionResult> delete(string id)
        {

            return Ok(await productRepository.Delete(id));
        }

    }
}
