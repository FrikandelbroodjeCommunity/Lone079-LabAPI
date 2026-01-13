using System.Collections.Generic;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp079Events;
using LabApi.Events.Arguments.WarheadEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using Random = System.Random;

namespace Lone079;

public static class EventHandlers
{
    private static readonly Random Random = new();
    private static bool _isTransformationAllowed;
    private static CoroutineHandle _checkCoroutine;

    private static bool Debug => Lone079.Instance.Config.Debug;

    private static IEnumerator<float> CheckForScp079Transformation(float delay = 0.5f)
    {
        yield return Timing.WaitForSeconds(delay);

        if (!_isTransformationAllowed)
        {
            Logger.Debug("[SCP-079] Transformation conditions not met.", Debug);
            yield break;
        }

        var scpTeam = Player.List
            .Where(x => x.Team == Team.SCPs)
            .Where(p => Lone079.Instance.Config.CountZombies || p.Role != RoleTypeId.Scp0492)
            .Where(p => p.Role != RoleTypeId.Scp079)
            .ToList();

        if (scpTeam.Count > 0)
        {
            Logger.Debug("[SCP-079] Other SCPs still alive.", Debug);
            yield break;
        }

        var scp079 = Player.List.FirstOrDefault(p => p.IsScp079());
        if (scp079 == null)
        {
            Logger.Debug("[SCP-079] SCP-079 not found.", Debug);
            yield break;
        }

        if (!TryGetRandomScpRole(out var newRole))
        {
            Logger.Debug("[SCP-079] No available SCP roles configured.", Debug);
            yield break;
        }

        PerformScpTransformation(scp079, newRole);
    }

    private static void PerformScpTransformation(Player player, RoleTypeId newRole)
    {
        if (!player.IsScp079())
        {
            Logger.Debug($"[SCP-079] Player {player.Nickname} is not SCP-079.", Debug);
            return;
        }

        var scp079Role = (Scp079Role)player.RoleBase;
        var accessTier = 5;
        if (scp079Role.SubroutineModule.TryGetSubroutine(out Scp079TierManager manager))
        {
            accessTier = manager.AccessTierLevel;
        }
        else
        {
            Logger.Warn("Failed to check SCP-079 level, spawning in with full health");
        }

        Logger.Debug($"[SCP-079] Transforming {player.Nickname} to {newRole}", Debug);
        player.SetRole(newRole);

        ApplyHealthModifications(player, accessTier);
        player.ShowTransformationBroadcast();
    }

    private static void ApplyHealthModifications(Player player, int scp079Level)
    {
        var config = Lone079.Instance.Config;
        var healthMultiplier = config.ScaleWithLevel
            ? (config.HealthPercent + (scp079Level - 1) * 5) / 100f
            : config.HealthPercent / 100f;

        player.Health = player.MaxHealth * healthMultiplier;
    }

    public static void OnPlayerDeath(PlayerDyingEventArgs ev)
    {
        if (ev.Player.Team != Team.SCPs) return;

        Logger.Debug($"[SCP-079] SCP death detected: {ev.Player.Nickname}", Debug);

        if (_checkCoroutine.IsRunning)
            Timing.KillCoroutines(_checkCoroutine);

        _checkCoroutine = Timing.RunCoroutine(
            CheckForScp079Transformation(Lone079.Instance.Config.RespawnDelay)
        );
    }

    public static void OnWarheadDetonated(WarheadDetonatedEventArgs _)
    {
        Logger.Debug("[SCP-079] Warhead detonated - disabling transformations");
        _isTransformationAllowed = false;
    }

    public static void OnRoundStarted()
    {
        Logger.Debug("[SCP-079] New round started - resetting settings");
        _isTransformationAllowed = true;
    }

    public static void HandleRecontainment(Scp079RecontainingEventArgs ev)
    {
        var activeScps = Player.List
            .Where(x => x.Team == Team.SCPs)
            .Where(p => p != ev.Player)
            .Where(p => Lone079.Instance.Config.CountZombies || p.Role != RoleTypeId.Scp0492)
            .ToList();

        var shouldTransform = activeScps.Count == 0 || Lone079.Instance.Config.TransformOnRecontain;

        ev.IsAllowed = !shouldTransform;

        if (!shouldTransform) return;

        if (!TryGetRandomScpRole(out var newRole))
        {
            Logger.Debug("[SCP-079] No valid SCP roles available");
            return;
        }

        PerformScpTransformation(ev.Player, newRole);
    }

    private static bool TryGetRandomScpRole(out RoleTypeId role)
    {
        role = RoleTypeId.None;
        var availableRoles = Lone079.Instance.Config.Scp079AvailableRoles;

        if (availableRoles == null || availableRoles.Count == 0)
            return false;

        role = availableRoles[Random.Next(availableRoles.Count)];
        return true;
    }
}

public static class PlayerExtensions
{
    public static bool IsScp079(this Player player)
    {
        return player != null &&
               player.Role == RoleTypeId.Scp079;
    }

    public static void ShowTransformationBroadcast(this Player player)
    {
        player.SendHint(
            Lone079.Instance.Config.BroadcastMessage,
            Lone079.Instance.Config.BroadcastDuration
        );

        foreach (var ply in Player.List.Where(x => x != player))
        {
            ply.SendBroadcast(
                Lone079.Instance.Config.PublicBroadMessage,
                Lone079.Instance.Config.PublicBroadcastDuration
            );
        }
    }
}