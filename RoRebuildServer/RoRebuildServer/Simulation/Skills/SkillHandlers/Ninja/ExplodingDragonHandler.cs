using RebuildSharedData.Enum.EntityStats;
using RebuildSharedData.Enum;
using RebuildSharedData.Data;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.Networking;
using RoRebuildServer.Simulation.Util;
using RoRebuildServer.Data;
using RoRebuildServer.EntityComponents.Util;

namespace RoRebuildServer.Simulation.Skills.SkillHandlers.Ninja
{
    [SkillHandler(CharacterSkill.ExplodingDragon, SkillClass.Magic, SkillTarget.Enemy)]
    public class ExplodingDragonHandler : SkillHandlerBase  {

        private readonly int requiredCatalystItemId = 7521; // Flame Stone Id
        
        public override bool PreProcessValidation(CombatEntity source, CombatEntity? target, Position position, int lvl,
            bool isIndirect, bool isItemSource) {
            // Exploding Dragon requires a Flame Stone
            // Make sure to consume the catalyst item, if it can't be consumed we end here
            if (!isIndirect && !isItemSource) {
                if (ConsumeGemstoneForSkillWithFailMessage(source, requiredCatalystItemId) == false)  {
                    return false;
                }
            }

            return true;
        }
        
        public override int GetAreaOfEffect(CombatEntity source, Position position, int lvl) => 2; //range 2 = 5x5
        
        private float GetAfterCastDelay()  {
            return 2f;
        }
        
        public override float GetCastTime(CombatEntity source, CombatEntity? target, Position position, int lvl)  {
            return 3f;
        }

        
        public override void Process(CombatEntity source, CombatEntity? target, Position position, int lvl,
            bool isIndirect, bool isItemSource)  {
            if (lvl < 0 || lvl > 5)
                lvl = 5;

            if (target == null || !target.IsValidTarget(source))
                return;
            
            var map = source.Character.Map;
            //gather all players who can see either the caster or target as recipients of the following packets
            map.AddVisiblePlayersAsPacketRecipients(source.Character, target.Character);
            
            var flags = AttackFlags.Magical;
            var amountOfHits = 3;
            var damageMultiplier = (1.5f + (1.5f * lvl) / amountOfHits);
            var range = 2 ;

            //first, hit the target with Napalm Beat fair and square
            using var targetList = EntityListPool.Get();
            var req = new AttackRequest(CharacterSkill.ExplodingDragon, damageMultiplier, amountOfHits, flags, AttackElement.Fire);
            (req.MinAtk, req.MaxAtk) = source.CalculateAttackPowerRange(true);
            var res = source.CalculateCombatResultUsingSetAttackPower(target, req);
            res.Time = Time.ElapsedTimeFloat + 0.15f; // Make the attack hit shortly after
                
            source.ApplyAfterCastDelay(GetAfterCastDelay(), ref res);
            source.ExecuteCombatResult(res, false);
            
            CommandBuilder.SkillExecuteTargetedSkill(source.Character, target.Character, CharacterSkill.ExplodingDragon, lvl, res, isIndirect); //send cast packet
            
            //now gather all players getting hit
            map.GatherEnemiesInArea(source.Character, target.Character.Position, range, targetList, !isIndirect, true);
            // Make sure the initial enemy is also targeted
            
            foreach (var e in targetList)
            {
                if (e == target.Entity)
                    continue; //we've already hit them

                if (!e.TryGet<CombatEntity>(out var blastTarget))
                    continue;

                if (!blastTarget.IsValidTarget(source))
                    continue;

                res.Target = e;
                blastTarget.ExecuteCombatResult(res, false);  //apply damage to target
                CommandBuilder.AttackMulti(source.Character, blastTarget.Character, res, false);
            }
            
            CommandBuilder.ClearRecipients();

            if (!isIndirect)  {
                source.ApplyCooldownForSupportSkillAction();
            }
                
        }
    }
}