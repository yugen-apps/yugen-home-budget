using System.ComponentModel.DataAnnotations.Schema;

namespace Yugen.HomeBudget.Data.Models;

public abstract class Entity
{
    public int Id { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastModifiedOn { get; set; }

    [ForeignKey(nameof(CreatedByApplicationUser))]
    public int? CreatedByApplicationUserId { get; set; }

    public ApplicationUser? CreatedByApplicationUser { get; set; }

    [ForeignKey(nameof(LastModifiedByApplicationUser))]
    public int? LastModifiedByApplicationUserId { get; set; }

    public ApplicationUser? LastModifiedByApplicationUser { get; set; }
}