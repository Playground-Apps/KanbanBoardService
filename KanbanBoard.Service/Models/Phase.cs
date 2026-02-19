using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KanbanBoardService.Models
{
    /// <summary>
    /// Table for holding phases (columns) of a Kanban board.
    /// Represents a single phase such as "To Do", "In Progress", or "Done".
    /// </summary>
    [Table("Phases")]
    public class Phase
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the phase.
        /// This is a required value and is used to label the column on the board
        /// (for example: "Backlog", "To Do", "In Progress", or "Done").
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Specifies the board to which this phase belongs.
        /// This is the foreign key referencing the parent <see cref="Board"/>.
        /// </summary>
        [ForeignKey("Board")]
        public int BoardId { get; set; }

        /// <summary>
        /// Navigation property for the parent board.
        /// May be null if the related <see cref="Board"/> entity is not loaded.
        /// </summary>
        [JsonIgnore]
        public Board? Board { get; set; }

        /// <summary>
        /// Navigational property of <see cref="PhaseTransitions"/>
        /// </summary>
        [InverseProperty(nameof(PhaseTransitions.FromPhase))]
        public ICollection<PhaseTransitions> PhaseMovement { get; set; } = new List<PhaseTransitions>();

        /// <summary>
        /// Navigational property of <see cref="Task"/>
        /// </summary>
        [InverseProperty(nameof(Task.Phase))]
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
