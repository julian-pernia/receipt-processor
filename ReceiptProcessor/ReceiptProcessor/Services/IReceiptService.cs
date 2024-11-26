using ReceiptProcessor.Models;

namespace ReceiptProcessor.Services
{
    public interface IReceiptService
    {
        public bool Exists(Guid receiptId);

        public Guid Process(Receipt receipt);

        public int CalculatePoints(Guid receiptId);

        public void ClearAllReceipts();
    }
}
