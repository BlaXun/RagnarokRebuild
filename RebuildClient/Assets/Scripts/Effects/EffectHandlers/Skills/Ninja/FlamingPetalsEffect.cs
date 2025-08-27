using System;
using System.Runtime.CompilerServices;
using Assets.Scripts.Effects.PrimitiveData;
using Assets.Scripts.Network;
using Assets.Scripts.Objects;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Effects.EffectHandlers.General;

namespace Assets.Scripts.Effects.EffectHandlers.Skills
{
    [RoEffect("FlamingPetals")]
    public class FlamingPetalsEffect : IEffectHandler
    {
        public static Ragnarok3dEffect Create(ServerControllable source, GameObject target, float delayTime, int level)
        {
            var effect = RagnarokEffectPool.Get3dEffect(EffectType.Fireball);
            effect.SourceEntity = source;
            effect.AimTarget = target;
            effect.SetDurationByTime(60);
            effect.UpdateOnlyOnFrameChange = true;
            effect.ActiveDelay = delayTime;
            effect.PositionOffset = target.transform.position;
            
            EffectSharedMaterialManager.PrepareEffectSprite("Assets/Sprites/Effects/fireball.spr");

            return effect;
        }
        
        public bool Update(Ragnarok3dEffect effect, float pos, int step)
        {
            Debug.Log($"UPDATE");
            if (!EffectSharedMaterialManager.TryGetEffectSprite("Assets/Sprites/Effects/fireball.spr", out _))
            {
                effect.ResetStep();
                Debug.Log($"true");
                return true; //wait until the sprite loads
            }
            
            AudioManager.Instance.OneShotSoundEffect(effect.SourceEntity.Id, "ef_fireball.ogg", effect.SourceEntity.transform.position, 0.5f);
            //RoSpriteProjectileEffect.CreateProjectile(effect.SourceEntity, effect.AimTarget, "Assets/Sprites/Effects/fireball.spr", c, 0);
            var sc = effect.FollowTarget.GetComponent<ServerControllable>();
            //RoSpriteProjectileEffect.CreateProjectile(effect.SourceEntity, effect.AimTarget, "Assets/Sprites/Effects/fireball.spr", c, 0);
            RoSpriteEffect.AttachSprite(effect.SourceEntity, "Assets/Sprites/Effects/msg.spr", 1f, 1f, RoSpriteEffectFlags.EndWithAnimation);

            return step < effect.DurationFrames;
        }
        /*public bool Update(Ragnarok3dEffect effect, float pos, int step) {

            if (step % 10 == 0) {
                Debug.Log($"FollowTarget: {effect.FollowTarget}");
                var sc = effect.FollowTarget.GetComponent<ServerControllable>();
                Debug.Log($"sc: {sc}");
                //RoSpriteEffect.AttachSprite(sc, "Assets/Sprites/Effects/msg.spr", 1f, 1f, RoSpriteEffectFlags.EndWithAnimation);
            }
            
            return effect.IsTimerActive;
        }*/
    }
}