using Robust.Shared.Configuration;

namespace Content.Server._CleanSector;

[CVarDefs]
public sealed class CleanSectorCVars
{
    public static readonly CVarDef<float> WishSpeed =
        CVarDef.Create("cleansector.wishspeed", 20f, CVar.REPLICATED | CVar.SERVER);
}
