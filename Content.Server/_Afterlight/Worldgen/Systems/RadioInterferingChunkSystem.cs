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
        SubscribeLocalEvent<LowPowerRadioComponent, RadioSendAttemptEvent>(OnTryHeadsetTransmit);
    }

    private void OnTryHeadsetTransmit(EntityUid uid, LowPowerRadioComponent comp, RadioSendAttemptEvent ev)
    {
        var xform = Transform(uid);
        var chunk = GetOrCreateChunk(GetChunkCoords(uid, xform), xform.MapUid!.Value);
        if (HasComp<RadioInterferingChunkComponent>(chunk))
            ev.Cancel();
    }
}
