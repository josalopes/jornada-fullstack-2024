using System.ComponentModel.DataAnnotations.Schema;

namespace Fina.Core.Models
{
    public class Category
    {
        [ForeignKey("Transaction")]
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
