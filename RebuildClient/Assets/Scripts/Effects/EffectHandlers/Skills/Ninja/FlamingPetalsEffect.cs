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
        public static Ragnarok3dEffect Spawn(ServerControllable source, ServerControllable target, float delayTime, int level)
        {
            var effect = RagnarokEffectPool.Get3dEffect(EffectType.FlamingPetals);
            effect.FollowTarget = target.gameObject;
            effect.SourceEntity = target;
            effect.SetDurationByFrames(9*level);
            effect.AimTarget = target.gameObject;
            effect.UpdateOnlyOnFrameChange = true;
            
            return effect;
        }
        
        public bool Update(Ragnarok3dEffect effect, float pos, int step) {

            if (step % 11 == 0) {
                System.Random rnd = new System.Random();
                ServerControllable target = effect.FollowTarget.GetComponent<ServerControllable>();
                CameraFollower.Instance.CreateEffectAtLocation($"firehit{rnd.Next(1,3)}", target.RealPosition + target.PositionOffset.normalized,
                    new Vector3(0.75f, 0.75f, 0.75f), 0);
                
                AudioManager.Instance.AttachSoundToEntity(effect.SourceEntityId, "ef_firehit.ogg", effect.AimTarget.gameObject);
            }
            
            
            
            return effect.IsTimerActive;
        }
    }
}