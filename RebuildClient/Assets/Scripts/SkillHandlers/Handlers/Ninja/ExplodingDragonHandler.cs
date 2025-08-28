using Assets.Scripts.Network;
using RebuildSharedData.Enum;
using UnityEngine;
using RebuildSharedData.Enum.EntityStats;
using Assets.Scripts.Effects.EffectHandlers;

namespace Assets.Scripts.SkillHandlers.Handlers
{
    [SkillHandler(CharacterSkill.ExplodingDragon)]
    public class ExplodingDragonHandler : SkillHandlerBase {
        public override bool DoesAttackTakeWeaponSound => false;
        
        public override void StartSkillCasting(ServerControllable src, ServerControllable target, int lvl, float castTime) {
            HoldStandbyMotionForCast(src, castTime);
            src.AttachEffect(CastEffect.Create(castTime, src.gameObject, AttackElement.Fire));
            target?.AttachEffect(CastLockOnEffect.Create(castTime, target.gameObject));
        }
        
        public override void ExecuteSkillTargeted(ServerControllable src, ref AttackResultData attack)  {
            if (src != null)  {
                CameraFollower.Instance.AttachEffectToEntity("ExplodingDragon", attack.Target.gameObject, src.Id);
                src.PerformSkillMotion();
            }
        }
    }
}