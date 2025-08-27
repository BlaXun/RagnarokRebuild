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
        public override bool DoesAttackTakeWeaponSound => false;
        
        public override void StartSkillCasting(ServerControllable src, ServerControllable target, int lvl, float castTime)  {
            HoldStandbyMotionForCast(src, castTime);
            src.AttachEffect(CastEffect.Create(castTime, src.gameObject, AttackElement.Fire));
            target?.AttachEffect(CastLockOnEffect.Create(castTime, target.gameObject));
        }
        
        public override void OnHitEffect(ServerControllable target, ref AttackResultData attack)  {
            attack.Target?.Messages.SendHitEffect(attack.Src, attack.DamageTiming, 1, attack.HitCount);
        }
        
        public override void ExecuteSkillTargeted([CanBeNull] ServerControllable source, ref AttackResultData attack) {
            
            source?.PerformSkillMotion();

            CameraFollower.Instance.CreateEffectAtLocation("FlametrapHit", attack.Target.CellPosition.ToWorldPosition() ,
                new Vector3(2, 2, 2), 0);
            
            //CameraFollower.Instance.AttachEffectToEntity("FlametrapHit", attack.Target.gameObject, source.Id);
            source?.LookAtOrDefault(attack.Target);
            if (source != null && attack.Target != null && attack.Result != AttackResult.Invisible)  {
                FlamingPetalsEffect.Spawn(source, attack.Target, 0f, attack.SkillLevel);
            }
        }
    }
}