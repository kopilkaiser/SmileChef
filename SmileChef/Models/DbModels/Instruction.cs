using System.ComponentModel.DataAnnotations.Schema;

namespace ChefApp.Models.DbModels
{
    [Table("Instructions")]
    public class Instruction
    {
        public int InstructionId { get; set; }

        public string Description { get; set; } = null!;

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }

        public int OrderId { get; set; }
        public TimeSpan? Duration { get; set; }
        public Recipe Recipe { get; set; }
    }
}
