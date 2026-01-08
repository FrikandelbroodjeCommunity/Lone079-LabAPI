using System;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;

namespace Lone079;

public class Lone079 : Plugin<Config>
{
    public override string Name => "Lone079";

    public override string Description =>
        " A LabAPI plugin for SCP:Secret Laboratory that transforms SCP-079 into a random SCP if it becomes the last SCP alive.";

    public override string Author => "Naxefir";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    public static Lone079 Instance;

    public override void Enable()
    {
        if (Config.Scp079AvailableRoles == null || Config.Scp079AvailableRoles.Count == 0)
        {
            Logger.Error("Scp079AvailableRoles is empty or null. Plugin will not work correctly.");
        }

        Instance = this;

        ServerEvents.RoundStarted += EventHandlers.OnRoundStarted;
        PlayerEvents.Dying += EventHandlers.OnPlayerDeath;
        WarheadEvents.Detonated += EventHandlers.OnWarheadDetonated;
        Scp079Events.Recontaining += EventHandlers.HandleRecontainment;
    }

    public override void Disable()
    {
        ServerEvents.RoundStarted -= EventHandlers.OnRoundStarted;
        PlayerEvents.Dying -= EventHandlers.OnPlayerDeath;
        WarheadEvents.Detonated -= EventHandlers.OnWarheadDetonated;
        Scp079Events.Recontaining -= EventHandlers.HandleRecontainment;
    }
}