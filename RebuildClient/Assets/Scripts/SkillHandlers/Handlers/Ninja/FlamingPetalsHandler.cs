using Assets.Scripts.Effects.EffectHandlers;
using Assets.Scripts.Effects.EffectHandlers.Skills;
using Assets.Scripts.Network;
using JetBrains.Annotations;
using RebuildSharedData.Enum;
using RebuildSharedData.Enum.EntityStats;
using UnityEngine;

using Assets.Scripts.Effects;
namespace Assets.Scripts.SkillHandlers
{
    [SkillHandler(CharacterSkill.FlamingPetals)]
    public class FlamingPetalsHandler : SkillHandlerBase
    {
        public override void OnHitEffect(ServerControllable target, ref AttackResultData attack)  {
            attack.Target?.Messages.SendHitEffect(attack.Src, attack.DamageTiming, 1, attack.HitCount);
        }
        
        public override void StartSkillCasting(ServerControllable src, ServerControllable target, int lvl, float castTime) {
            Debug.Log($"StartSkillCasting");
            HoldStandbyMotionForCast(src, castTime);
            src.AttachEffect(CastEffect.Create(castTime, src.gameObject, AttackElement.Fire));
            target?.AttachEffect(CastLockOnEffect.Create(castTime, target.gameObject));
            Debug.Log($"StartSkillCasting ENDE");
        }

        public override void ExecuteSkillTargeted([CanBeNull] ServerControllable src, ref AttackResultData attack) {
            
            src?.LookAtOrDefault(attack.Target);
            if (src != null && attack.Target != null && attack.Result != AttackResult.Invisible)  {
                FlamingPetalsEffect.Create(src, attack.Target.gameObject, 0f, attack.SkillLevel);
                src?.PerformSkillMotion(true);
            }
        }
    }
}