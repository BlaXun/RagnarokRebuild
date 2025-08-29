using RebuildSharedData.Data;
using RebuildSharedData.Enum;
using RebuildSharedData.Enum.EntityStats;
using RoRebuildServer.EntityComponents;

namespace RoRebuildServer.Simulation.Skills.SkillHandlers.Ninja  {
    
    [SkillHandler(CharacterSkill.NinjaMastery, SkillClass.None, SkillTarget.Passive)]
    public class NinjaMasteryHandler : SkillHandlerBase
    {
        // Would have loved to implement the logic right here
        // But the way skills like this (For example mages' "Increase SP Recovery"
        // are implemented we do all this in the Player.cs :/
        public override void ApplyPassiveEffects(CombatEntity owner, int lvl)  {
            //owner.AddStat(CharacterStat.AddSpRecoveryAbsolute, 3 * lvl);
        }

        public override void RemovePassiveEffects(CombatEntity owner, int lvl) {
            //owner.SubStat(CharacterStat.AddSpRecoveryAbsolute, 3 * lvl);
        }

        public override void Process(CombatEntity source, CombatEntity? target, Position position, int lvl,
            bool isIndirect, bool isItemSource) {
            throw new NotImplementedException();
        }
    }
}