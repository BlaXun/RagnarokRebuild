using RebuildSharedData.Data;
using RebuildSharedData.Enum;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.Networking;
using RoRebuildServer.Simulation.StatusEffects.Setup;

namespace RoRebuildServer.Simulation.Skills.SkillHandlers.Ninja;

[SkillHandler(CharacterSkill.CicadaSkinShed, SkillClass.Physical, SkillTarget.Self)]
public class CicadaSkinShedHandler : SkillHandlerBase  {
    
    public override void Process(CombatEntity source, CombatEntity? target, Position position, int lvl, bool isIndirect,
        bool isItemSource) {
        var ch = source.Character;
        if (source.Character.Type == CharacterType.Player && source.Character.State == CharacterState.Sitting)
            return; // Can not be applied when sitting
            
        var skillDuration = 10f + (10f * lvl);
        source.ApplyCooldownForSupportSkillAction();

        CommandBuilder.SkillExecuteSelfTargetedSkillAutoVis(ch, CharacterSkill.CicadaSkinShed, lvl, isIndirect);
        
        var status = StatusEffectState.NewStatusEffect(CharacterStatusEffect.CicadaSkinShed, skillDuration, lvl);
        source.AddStatusEffect(status);
    }
}