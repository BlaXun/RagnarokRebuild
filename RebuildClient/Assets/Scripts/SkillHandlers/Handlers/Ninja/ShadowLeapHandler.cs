using Assets.Scripts.Effects.EffectHandlers;
using Assets.Scripts.Effects.EffectHandlers.StatusEffects;
using Assets.Scripts.Network;
using JetBrains.Annotations;
using RebuildSharedData.Enum;
using UnityEngine;

namespace Assets.Scripts.SkillHandlers.Handlers
{
    [SkillHandler(CharacterSkill.ShadowLeap)]
    public class ShadowLeapHandler : SkillHandlerBase
    {
        public override void ExecuteSkillTargeted([CanBeNull] ServerControllable src, ref AttackResultData attack) {
            
            if (!src)
                return;
            
            // Force the "hands on ground" sprite
            src.SpriteAnimator.ChangeMotion(SpriteMotion.Casting,true);
            src.SpriteAnimator.OverrideCurrentFrame(3);
            src.SpriteAnimator.PauseAnimation();
        }
    }
}