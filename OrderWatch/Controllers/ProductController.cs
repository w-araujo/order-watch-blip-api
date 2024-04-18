using Microsoft.AspNetCore.Mvc;
using OrderWatch.Database;
using OrderWatch.Models;

namespace OrderWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DbConnection _dbConnection;

        public ProductController(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet(Name = "Product")]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _dbConnection.Set<Product>().ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao listar os produtos: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                var product = _dbConnection.Set<Product>().Find(id);

                if (product == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter o produto: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product newProduct)
        {
            try
            {
                _dbConnection.Products.Add(newProduct);
                _dbConnection.SaveChanges();
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar o produto: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdatedProduct")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            try
            {
                var existingProduct = _dbConnection.Set<Product>().Find(id);

                if (existingProduct == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Image = updatedProduct.Image;
                existingProduct.UpdatedAt = DateTime.UtcNow;

                _dbConnection.SaveChanges();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar o produto: {ex.Message}");

            }

        }

        [HttpDelete("{id}", Name = "DeletedProduct")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var productToDelete = _dbConnection.Set<Product>().Find(id);

                if (productToDelete == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                _dbConnection.Set<Product>().Remove(productToDelete);
                _dbConnection.SaveChanges();

                return Ok($"Produto com ID {id} removido com sucesso.");
            }

            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir o Produto: {ex.Message}");
            }

        }
    }
}
