using Content.Server._Afterlight.Research.Components;
using Content.Server.Station.Systems;
using Content.Shared.Research.Components;

namespace Content.Server._Afterlight.Research.Systems;

/// <summary>
/// This handles renaming the R&D server automatically.
/// </summary>
public sealed class AutoRenameResearchServerSystem : EntitySystem
{
    [Dependency] private readonly StationSystem _station = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<AutoRenameResearchServerComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<TryRenameEvent>(OnTryRename);
    }

    private void OnTryRename(TryRenameEvent ev)
    {
        if (Deleted(ev.Target) || Terminating(ev.Target))
            return;

        var station = _station.GetOwningStation(ev.Target);
        if (station is null)
            return;

        var server = Comp<ResearchServerComponent>(ev.Target);
        server.ServerName = $"RDSERVER ({Name(station.Value)})";
    }

    private void OnMapInit(EntityUid uid, AutoRenameResearchServerComponent component, MapInitEvent args)
    {
        QueueLocalEvent(new TryRenameEvent() { Target = uid });
    }

    private sealed class TryRenameEvent : EntityEventArgs
    {
        public required EntityUid Target;
    }
}
