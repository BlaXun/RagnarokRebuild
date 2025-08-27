using RebuildSharedData.Data;
using RebuildSharedData.Enum;
using RebuildSharedData.Enum.EntityStats;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.Networking;
using RoRebuildServer.Simulation.Util;
using System.Diagnostics;
using RoRebuildServer.EntityComponents.Character;
using RoRebuildServer.EntityComponents.Util;

namespace RoRebuildServer.Simulation.Skills.SkillHandlers.Mage;

[SkillHandler(CharacterSkill.FlamingPetals, SkillClass.Magic, SkillTarget.Enemy)]
public class FlamingPetalsHandler : SkillHandlerBase
{
    public override float GetCastTime(CombatEntity source, CombatEntity? target, Position position, int lvl) {
        return 0.7f * lvl;
    }

    public override void Process(CombatEntity source, CombatEntity? target, Position position, int lvl, bool isIndirect,
        bool isItemSource) {
        if (lvl < 0 || lvl > 10)
            lvl = 10;

        if (target == null || !target.IsValidTarget(source))
            return;

        var res = source.CalculateCombatResult(target, 0.9f, lvl, AttackFlags.Magical, CharacterSkill.FlamingPetals, AttackElement.Fire);
        // Display damage shortly after the cast ended
        res.Time = Time.ElapsedTimeFloat + 0.3f;

        //var baseTime = 1.0f - ((lvl + 1) % 2) * 0.2f;
        
        //source.ApplyAfterCastDelay(baseTime + hits * 0.2f, ref res);
        source.ApplyCooldownForAttackAction(target);
        source.ExecuteCombatResult(res, false);

        CommandBuilder.SkillExecuteTargetedSkillAutoVis(source.Character, target.Character, CharacterSkill.FlamingPetals, lvl, res, isIndirect);
    }
}
