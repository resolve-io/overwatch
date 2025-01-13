namespace Overwatch.FeatureFlag.Api.Application.Handlers.Rules;

public record UpdateRuleRequest([Required]Guid RuleId, [Required] bool IsEnabled) : IRequest<bool>;

public class UpdateRuleHandler(ComponentDbContext db) : IRequestHandler<UpdateRuleRequest, bool>
{
    public async Task<bool> Handle(UpdateRuleRequest command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Fetch the rule by ID
        var rule = await db.Rules.FirstOrDefaultAsync(r => r.Id == command.RuleId, cancellationToken);

        // Validate that the rule exists
        if (rule == null)
            throw new KeyNotFoundException($"Rule with ID '{command.RuleId}' not found.");

        // Update rule properties
        var updatedRule = rule with
        {
            IsEnabled = command.IsEnabled,
        };

        // Update the rule
        db.Entry(rule).CurrentValues.SetValues(updatedRule);

        // Save changes
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}