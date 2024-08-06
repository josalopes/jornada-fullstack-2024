namespace Fina.Core.Requests.Transactions
{
    public class GetTransactionByPeriodRequest : PageRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
