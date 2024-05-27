using System.ComponentModel.DataAnnotations;

namespace SmileChef.ViewModels
{
    public class InstructionViewModel
    {
        public int InstructionId { get; set; }
        [Required(ErrorMessage = "Instruction description is required")]
        [MaxLength(150, ErrorMessage = "Instruction description cannot exceed 150 characters")]
        public string? Description { get; set; }
        public int OrderId { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsRemoved { get; set; } = false; // Add this property
    }
}
