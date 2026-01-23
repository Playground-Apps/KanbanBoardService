using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanBoardService.Models
{

    /// <summary>
    /// Represents a Kanban board which contains a collection of ordered <see cref="Phase"/>s.
    /// </summary>
    [Table("Boards")]
    public class Board
    {
        /// <summary>
        /// Gets or sets the primary key identifier for the board.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the board.
        /// This property is required and should be set when creating a new board.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Navigation property for the phases that belong to this board.
        /// </summary>
        /// <remarks>
        /// - The <see cref="InversePropertyAttribute"/> maps this collection to the <c>Board</c>
        ///   navigation property on the <see cref="Phase"/> entity for a one-to-many relationship.
        /// - Initialized to an empty <see cref="List{T}"/> to avoid null reference issues when
        ///   enumerating phases before any are added by EF or application logic.
        /// </remarks>
        [InverseProperty(nameof(Phase.Board))]
        public ICollection<Phase> Phases { get; set; } = new List<Phase>();

    }
}
