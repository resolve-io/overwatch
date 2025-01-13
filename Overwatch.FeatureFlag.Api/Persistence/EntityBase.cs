using System.ComponentModel.DataAnnotations;

namespace Overwatch.FeatureFlag.Api.Persistence;

/// <summary>
/// An abstract record providing shared properties for entities in the system. 
/// Includes an immutable ID and timestamps for creation and modification.
/// </summary>
public abstract record EntityBase
{
    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the timestamp indicating when the entity was created.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Gets the timestamp indicating the last time the entity was modified.
    /// </summary>
    public DateTime DateModified { get; set; }
}