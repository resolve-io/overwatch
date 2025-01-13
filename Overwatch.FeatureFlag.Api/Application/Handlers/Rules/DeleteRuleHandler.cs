namespace Overwatch.FeatureFlag.Api.Application.Handlers.Rules;

public record DeleteRuleRequest([Required] Guid RuleId) : IRequest<bool>
{
    public static DeleteRuleRequest CreateDeleteRuleRequest(Guid ruleId) => new(ruleId);
}

public class DeleteRuleHandler(ComponentDbContext db) : IRequestHandler<DeleteRuleRequest, bool>
{
    public async Task<bool> Handle(DeleteRuleRequest command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        // Fetch the rule by ID
        var rule = await db.Rules
            .FirstOrDefaultAsync(r => r.Id == command.RuleId, cancellationToken);

        // Validate that the rule exists
        if (rule == null)
            throw new KeyNotFoundException($"Rule with ID '{command.RuleId}' not found.");

        // Remove the rule
        db.Rules.Remove(rule);

        // Save changes
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}