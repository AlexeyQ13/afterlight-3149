using Content.Server._Afterlight.Worldgen.Components;
using Content.Server._Citadel.Worldgen.Systems;
using Content.Server.Radio;
using Content.Server.Radio.EntitySystems;

namespace Content.Server._Afterlight.Worldgen.Systems;

/// <summary>
/// This handles worldgen chunk driven radio interference.
/// </summary>
public sealed class RadioInterferingChunkSystem : BaseWorldSystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<RadioReceiveAttemptEvent>(OnTryHeadsetTransmit);
    }

    private void OnTryHeadsetTransmit(RadioReceiveAttemptEvent ev)
    {
        if (ev.RadioSource is null)
            return;
        var xform = Transform(ev.RadioSource.Value);
        var chunk = GetOrCreateChunk(GetChunkCoords(ev.RadioSource.Value, xform), xform.MapUid!.Value);
        if (HasComp<RadioInterferingChunkComponent>(chunk))
            ev.Cancel();
    }
}
