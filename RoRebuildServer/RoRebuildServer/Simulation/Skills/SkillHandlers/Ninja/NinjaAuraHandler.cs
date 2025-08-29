using RebuildSharedData.Data;
using RebuildSharedData.Enum;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.Networking;
using RoRebuildServer.Simulation.StatusEffects.Setup;

namespace RoRebuildServer.Simulation.Skills.SkillHandlers.Ninja;

[SkillHandler(CharacterSkill.NinjaAura, SkillClass.None, SkillTarget.Self)]
public class NinjaAuraHandler : SkillHandlerBase  {

    public override float GetCastTime(CombatEntity source, CombatEntity? target, Position position, int lvl) {
        return 6f - lvl;
    }

    public override void Process(CombatEntity source, CombatEntity? target, Position position, int lvl, bool isIndirect,
        bool isItemSource) {
        var ch = source.Character;
        if (source.Character.Type == CharacterType.Player && source.Character.State == CharacterState.Sitting)
            return; // Can not be applied when sitting

        var skillDuration = 15f + (15f * lvl);
        source.ApplyCooldownForSupportSkillAction();

        CommandBuilder.SkillExecuteSelfTargetedSkillAutoVis(ch, CharacterSkill.NinjaAura, lvl, isIndirect);

        var status = StatusEffectState.NewStatusEffect(CharacterStatusEffect.NinjaAura, skillDuration, lvl);
        source.AddStatusEffect(status);
    }
}