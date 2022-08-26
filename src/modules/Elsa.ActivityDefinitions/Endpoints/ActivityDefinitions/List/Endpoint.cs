using Elsa.ActivityDefinitions.Models;
using Elsa.ActivityDefinitions.Services;
using Elsa.Models;
using FastEndpoints;

namespace Elsa.ActivityDefinitions.Endpoints.ActivityDefinitions.List;

/// <summary>
/// An endpoint that returns a page of <see cref="ActivityDefinitionSummary"/> objects. 
/// </summary>
public class List : Endpoint<Request, PagedListResponse<ActivityDefinitionSummary>>
{
    private readonly IActivityDefinitionStore _store;

    /// <inheritdoc />
    public List(IActivityDefinitionStore store)
    {
        _store = store;
    }

    /// <inheritdoc />
    public override void Configure()
    {
        Get("/activity-definitions");
        Policies(Constants.PolicyName);
    }

    /// <inheritdoc />
    public override async Task<PagedListResponse<ActivityDefinitionSummary>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var parsedVersionOptions = req.VersionOptions != null ? VersionOptions.FromString(req.VersionOptions) : VersionOptions.Published;
        var pageArgs = new PageArgs(req.Page, req.PageSize);
        var pageOfSummaries = await _store.ListSummariesAsync(parsedVersionOptions, pageArgs, ct);
        return new PagedListResponse<ActivityDefinitionSummary>(pageOfSummaries);
    }
}