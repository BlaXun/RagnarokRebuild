using Assets.Scripts.Effects;
using Assets.Scripts.Effects.EffectHandlers;
using Assets.Scripts.Effects.EffectHandlers.Skills;
using Assets.Scripts.Network;
using JetBrains.Annotations;
using RebuildSharedData.Enum;
using RebuildSharedData.Enum.EntityStats;
using UnityEngine;
using Assets.Scripts.Objects;
namespace Assets.Scripts.SkillHandlers.Handlers
{
    [SkillHandler(CharacterSkill.BlazeShield, true)]
    public class BlazeShieldHandler : SkillHandlerBase
    {
        public override void StartSkillCasting(ServerControllable src, ServerControllable target, int lvl, float castTime) {
            src.SpriteAnimator.ChangeMotion(SpriteMotion.Casting,true);
            
            // Force the "hands together" motion for casting
            src.SpriteAnimator.OverrideCurrentFrame(4);
            src.SpriteAnimator.PauseAnimation();
            
            src.AttachEffect(CastEffect.Create(castTime, src.gameObject, AttackElement.Fire));
            
            if(target != null)
                target.AttachEffect(CastLockOnEffect.Create(castTime, target.gameObject));
        }
        
        public override void ExecuteSkillTargeted([CanBeNull] ServerControllable src, ref AttackResultData attack)
        {
            if (src == null)
                return;
            
            HoldStandbyMotionForCast(src, 0.01f);
            
            // This will make the ninja kneel down while casting ...not quite what we want though.. we want the kneeling on stop
            src.SpriteAnimator.OverrideCurrentFrame(2);
            src.SpriteAnimator.PauseAnimation();
            
            // Playing the soundeffect, but a lil louder... its like in the original
            AudioManager.Instance.OneShotSoundEffect(src.Id, $"ef_firewall.ogg", attack.TargetAoE.ToWorldPosition(),2f);
        }
        
        public override void OnHitEffect(ServerControllable target, ref AttackResultData attack) {
            CameraFollower.Instance.CreateEffectAtLocation("firehit1",target.RealPosition, new Vector3(0.75f,0.75f,0.75f), 0);
        }
    }
}