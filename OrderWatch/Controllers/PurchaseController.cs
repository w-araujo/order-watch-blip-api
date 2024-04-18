using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWatch.Database;
using OrderWatch.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using SQLitePCL;

namespace OrderWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly DbConnection _dbConnection;

        public PurchaseController(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet(Name = "Purchase")]
        public IActionResult GetPurchases()
        {
            try
            {
                var purchases = _dbConnection.Purchases
                    .Select(p => new 
                    {
                        p.Id,
                        p.UserId,
                        p.ProductId,
                        p.Status,
                        p.IsPaid,
                        p.CreatedAt,
                        p.UpdatedAt,
                        User = _dbConnection.Users.FirstOrDefault(u => u.Id == p.UserId),
                        Product = _dbConnection.Products.FirstOrDefault(pr => pr.Id == p.ProductId),
                    }).ToList();

                var purchasesJson = JsonSerializer.Serialize(purchases);
                return Ok(purchasesJson);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao listar as compras: {ex.Message}");
            }
        }


        [HttpGet("email/{email}", Name = "GetPurchasesByUserEmail")]
        public IActionResult GetPurchaseByUserEmail(string email)
        {
            try
            {
                var purchases = _dbConnection.Purchases
                    .Join(_dbConnection.Users, p => p.UserId, u => u.Id, (p, u) => new { Purchase = p, User = u})
                    .Where(pu => pu.User.Email == email)
                    .Select(pu => new
                   {
                        pu.Purchase.Id,
                        pu.Purchase.UserId,
                        pu.Purchase.ProductId,
                        pu.Purchase.Status,
                        pu.Purchase.IsPaid,
                        pu.Purchase.CreatedAt,
                        pu.Purchase.UpdatedAt,
                        User = pu.User,
                        Product = _dbConnection.Products.FirstOrDefault(pr => pr.Id == pu.Purchase.ProductId),
                    }).ToList();

                var purchasesJson = JsonSerializer.Serialize(purchases);
                return Ok(purchasesJson);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter a compra: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetPurchase")]
        public IActionResult GetPurchase(int id)
        {
            try
            {
                var purchase = _dbConnection.Set<Purchase>().Find(id);

                if (purchase == null)
                {
                    return NotFound($"Compra com ID {id} não encontrada.");
                }

                return Ok(purchase);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter a compra: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreatePurchase([FromBody] Purchase purchase)
        {
            try
            {
                var user = _dbConnection.Users.Find(purchase.UserId);
                if (user == null)
                {
                    return NotFound($"Usuário com ID {purchase.UserId} não encontrado.");
                }

                var product = _dbConnection.Products.Find(purchase.ProductId);
                if(product == null)
                {
                    return NotFound($"Produto com ID {purchase.ProductId} não encontrado.");
                }
                Console.WriteLine($"{purchase.UserId}, {purchase.ProductId}");
                var newPurchase = new Purchase
                {
                    UserId = purchase.UserId,
                    ProductId = purchase.ProductId,
                    Status = purchase.Status,
                    IsPaid = purchase.IsPaid,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _dbConnection.Purchases.Add(newPurchase);
                _dbConnection.SaveChanges();
                return Ok(newPurchase);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar a compra: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdatedPurchase")]
        public IActionResult UpdatePurchase(int id, [FromBody] Purchase updatedPurchase)
        {
            try
            {
                var existingPurchase = _dbConnection.Set<Purchase>().Find(id);

                if (existingPurchase == null)
                {
                    return NotFound($"Compra com ID {id} não encontrada.");
                }

                existingPurchase.Status = updatedPurchase.Status;
                existingPurchase.IsPaid = updatedPurchase.IsPaid;
                existingPurchase.UpdatedAt = DateTime.UtcNow;

                _dbConnection.SaveChanges();

                return Ok(existingPurchase);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar a compra: {ex.Message}");

            }

        }

        [HttpDelete("{id}", Name = "DeletedPurchase")]
        public IActionResult DeletePurchase(int id)
        {
            try
            {
                var purchaseToDelete = _dbConnection.Set<Purchase>().Find(id);

                if (purchaseToDelete == null)
                {
                    return NotFound($"Compra com ID {id} não encontrada.");
                }

                _dbConnection.Set<Purchase>().Remove(purchaseToDelete);
                _dbConnection.SaveChanges();

                return Ok($"Compra com ID {id} removida com sucesso.");
            }

            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir a compra: {ex.Message}");
            }

        }
    }
}
