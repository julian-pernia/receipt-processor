using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReceiptProcessor.Models.Responses;
using ReceiptProcessor.Models;
using ReceiptProcessor.Services;

namespace ReceiptProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiptsController(IReceiptService receiptService, ILogger<ReceiptsController> logger) : Controller
    {
        private readonly IReceiptService _receiptService = receiptService;
        private readonly ILogger<ReceiptsController> _logger = logger;

        [HttpGet("{id}/points")]
        public IActionResult GetPoints(Guid id)
        {
            try
            {
                if (!_receiptService.Exists(id)) {
                    return NotFound($"Receipt with GUID \"{id}\" not found.");
                }

                int points = _receiptService.CalculatePoints(id);

                return Ok(new PointsResponse
                {
                    Points = points
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("process")]
        public IActionResult ProcessReceipt([FromBody] Receipt receipt)
        {
            try
            {
                decimal listedTotal = decimal.Parse(receipt.Total);
                decimal actualTotal = receipt.Items.Select(i => decimal.Parse(i.Price)).Sum();

                if (listedTotal != actualTotal)
                {
                    return BadRequest($"The listed Total ({listedTotal}) does not match the total calculated from the sum of all item prices ({actualTotal})");
                }

                Guid id = _receiptService.Process(receipt);

                return Ok(new ProcessResponse()
                {
                    Id = id.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("clear")]
        public IActionResult ProcessReceipt()
        {
            // WARNING: Clears all submitted receipts!
            _receiptService.ClearAllReceipts();

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAllReceipts()
        {
            var receipts = _receiptService.GetAll();

            return Ok(receipts);
        }
    }
}
