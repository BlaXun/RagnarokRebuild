using RebuildSharedData.Enum;
using RebuildSharedData.Enum.EntityStats;
using RoRebuildServer.EntityComponents.Character;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.Simulation.StatusEffects.Setup;
using RoRebuildServer.EntityComponents.Util;
using RoRebuildServer.Networking;
using RoRebuildServer.EntitySystem;
using RoRebuildServer.Logging;

namespace RoRebuildServer.Simulation.StatusEffects.ExpandedJobs;

[StatusEffectHandler(CharacterStatusEffect.NinjaAura, StatusClientVisibility.Everyone, StatusEffectFlags.NoSave, "NinjaAura")]
public class StatusNinjaAura : StatusEffectBase
{
    public override StatusUpdateMode UpdateMode => StatusUpdateMode.OnUpdate;
    
    //Value1: Skill level
    public override void OnApply(CombatEntity ch, ref StatusEffectState state) {
        
        ServerLogger.LogWarning("OnApply");
        ch.AddStat(CharacterStat.AddStr, state.Value1);
        ch.AddStat(CharacterStat.AddInt, state.Value1);

        if (ch.Character.State != CharacterState.Dead)
            ch.Character.State = CharacterState.Idle;
    }

    public override void OnExpiration(CombatEntity ch, ref StatusEffectState state)  {
        ServerLogger.LogWarning("OnExpiration");
        ServerLogger.LogWarning($"{ch}");
        ch.SubStat(CharacterStat.AddStr, state.Value1);
        ch.SubStat(CharacterStat.AddInt, state.Value1);
    }
}