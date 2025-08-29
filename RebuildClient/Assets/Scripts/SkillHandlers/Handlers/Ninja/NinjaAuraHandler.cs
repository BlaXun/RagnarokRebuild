using Assets.Scripts.Network;
using RebuildSharedData.Enum;
using JetBrains.Annotations;
using RebuildSharedData.Enum.EntityStats;
using Assets.Scripts.Effects.EffectHandlers;
    
namespace Assets.Scripts.SkillHandlers.Handlers
{
    [SkillHandler(CharacterSkill.NinjaAura)]
    public class NinjaAuraHandler : SkillHandlerBase
    {
        public override void StartSkillCasting(ServerControllable src, ServerControllable target, int lvl, float castTime) {
            HoldStandbyMotionForCast(src, castTime);
            src.AttachEffect(CastEffect.Create(castTime, src.gameObject, AttackElement.Water));
        }
        
        public override void ExecuteSkillTargeted([CanBeNull] ServerControllable src, ref AttackResultData attack) {
            src?.PerformSkillMotion();
        }
    }
}