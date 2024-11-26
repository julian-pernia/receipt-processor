using Microsoft.EntityFrameworkCore;
using ReceiptProcessor.Models;
using ReceiptProcessor.Models.Persistence;
using System.Text.RegularExpressions;

namespace ReceiptProcessor.Services
{
    public class ReceiptService : IReceiptService
    {
        public int CalculatePoints(Guid receiptId)
        {
            int points = 0;

            using ReceiptContext context = new();

            Receipt? receipt = context.Receipts
                .Include(r => r.Items)
                .FirstOrDefault(r => r.Id == receiptId);

            if (receipt == null)
            {
                return -1;
            }

            decimal total = decimal.Parse(receipt.Total);

            points += Regex.Replace(receipt.Retailer, "[^a-zA-Z0-9]", "").Length;

            points += total % 1 == 0 ? 50 : 0;

            points += total % 0.25M == 0 ? 25 : 0;

            points += (int)(Math.Floor(receipt.Items.Count / 2M) * 5);

            foreach (Item item in receipt.Items)
            {
                if (item.ShortDescription.Trim().Length % 3 == 0)
                {
                    points += (int)Math.Ceiling(decimal.Parse(item.Price) * 0.2M);
                }
            }

            if (receipt.PurchaseDate.Day % 2 != 0)
            {
                points += 6;
            }

            // Note: TimeOnly.IsBetween() is inclusive on the StartTime side and exclusive on the EndTime side.
            if (receipt.PurchaseTime.IsBetween(new TimeOnly(14, 0, 1), new TimeOnly(16, 0)))
            {
                points += 10;
            }

            return points;
        }

        // ! Normally I would not include such a nuclear option, but wanted to add it for the sake of demonstration convenience. !
        public void ClearAllReceipts()
        {
            using ReceiptContext context = new();

            context.Database.ExecuteSqlRaw("DELETE FROM Receipts");
            context.SaveChanges();
        }

        public bool Exists(Guid receiptId)
        {
            using ReceiptContext context = new();

            return context.Receipts.Any(r => r.Id == receiptId);
        }

        public List<Receipt> GetAll()
        {
            using ReceiptContext context = new();

            return [.. context.Receipts
                .Include(r => r.Items)
                .OrderBy(r => r.PurchaseDate)
                .ThenBy(r => r.PurchaseTime)];
        }

        public Guid Process(Receipt receipt)
        {
            using ReceiptContext context = new();

            var processed = context.Receipts.Add(receipt);
            context.SaveChanges();

            return processed.Entity.Id;
        }
    }
}
