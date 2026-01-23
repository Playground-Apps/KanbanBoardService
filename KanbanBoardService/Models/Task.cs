using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanBoardService.Models
{
    /// <summary>
    /// Table for holding task overview info.
    /// </summary>
    [Table("Tasks")]
    public class Task
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of task.
        /// </summary>
        [Required]
        public required string Title { get; set; }

        /// <summary>
        /// Description or short sumary of task.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Description or short sumary of task.
        /// </summary>
        [ForeignKey(nameof(Phase))]
        public int PhaseId { get; set; }

        /// <summary>
        /// Foreign key property of PhaseId.
        /// </summary>
        public Phase Phase { get; set; }

        /// <summary>
        /// Description or short sumary of task.
        /// </summary>
        public Priority Priority { get; set; } // e.g., 1 (High), 2 (Medium), 3 (Low)

        /// <summary>
        /// Specify story points.
        /// </summary>
        public int StoryPoints { get; set; }

        /// <summary>
        /// Description or short sumary of task.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Description or short sumary of task.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    public enum Priority
    {
        Highest,
        High,
        Medium,
        Low,
        Lowest,
    }
}
