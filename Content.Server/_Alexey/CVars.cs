using Robust.Shared.Configuration;

namespace Content.Server._Alexey;

[CVarDefs]
public sealed class AlexeyCVars
{
    public static readonly CVarDef<float> WishSpeed =
        CVarDef.Create("alexey.wishspeed", 20f, CVar.REPLICATED | CVar.SERVER);
}
