using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanBoardService.Models
{
    /// <summary>
    /// Represents a permitted transition between two <see cref="Phase"/>s on a Kanban board.
    /// </summary>
    /// <remarks>
    /// Instances of this class model allowed moves from one phase to another (for example,
    /// "To Do" -> "In Progress"). Typical usage is to store these rules in a data store
    /// and use them to validate or drive UI/flow logic when moving work items between phases.
    /// </remarks>
    [Table("PhaseTransitions")]
    public class PhaseTransitions
    {
        /// <summary>
        /// Primary key / unique identifier for this phase transition entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key referencing the source phase (the phase from which a transition starts).
        /// </summary>
        [ForeignKey("FromPhase")]
        public int FromPhaseId { get; set; }

        /// <summary>
        /// Navigation property for the source <see cref="Phase"/>.
        /// </summary>
        public Phase FromPhase { get; set; }

        /// <summary>
        /// Foreign key referencing the destination phase (the phase to which a transition leads).
        /// </summary>
        [ForeignKey("ToPhase")]
        public int ToPhaseId { get; set; }

        /// <summary>
        /// Navigation property for the destination <see cref="Phase"/>.
        /// </summary>
        public Phase ToPhase { get; set; } 
    }
}
