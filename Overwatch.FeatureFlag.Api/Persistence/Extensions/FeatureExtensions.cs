using Overwatch.FeatureFlag.Interface.Models;

namespace Overwatch.FeatureFlag.Api.Persistence.Extensions;

public static class FeatureExtensions
{
    /// <summary>
    /// Projects a FeatureEntity into a Feature model, including its rules and their associated environments.
    /// </summary>
    public static IQueryable<Feature> ProjectToFeature(this IQueryable<FeatureEntity> query)
    {
        return query.Select(f => new Feature
        {
            Id = f.Id,
            DateCreated = f.DateCreated,
            DateModified = f.DateModified,
            Name = f.Name,
            Rules = f.Rules.Select(r => new Rule
            {
                Id = r.Id,
                FeatureId = r.FeatureId,
                EnvironmentId = r.EnvironmentId,
                Tenant = r.Tenant ?? Rule.Wildcard,
                IsEnabled = r.IsEnabled,
                DateCreated = r.DateCreated,
                DateModified = r.DateModified,
                EnvironmentName = r.Environment != null ? r.Environment.Name : Rule.Wildcard,
                FeatureName = f.Name
            }).ToList()
        });
    }

    public static IQueryable<Rule> ProjectToRule(this IQueryable<RuleEntity> query)
    {
        return query.Select(r => new Rule
        {
            Id = r.Id,
            FeatureId = r.FeatureId,
            EnvironmentId = r.EnvironmentId,
            Tenant = r.Tenant ?? Rule.Wildcard,
            IsEnabled = r.IsEnabled,
            DateCreated = r.DateCreated,
            DateModified = r.DateModified,
            EnvironmentName = r.Environment != null ? r.Environment.Name : Rule.Wildcard,
            FeatureName = r.Feature != null ? r.Feature.Name : Rule.Wildcard
        });
    }


}