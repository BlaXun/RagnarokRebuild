using Assets.Scripts.Network;
using RebuildSharedData.Enum;
using JetBrains.Annotations;

namespace Assets.Scripts.SkillHandlers.Handlers
{
    [SkillHandler(CharacterSkill.CicadaSkinShed)]
    public class CicadaSkinShedHandler : SkillHandlerBase
    {
        public override void ExecuteSkillTargeted([CanBeNull] ServerControllable src, ref AttackResultData attack) {
            src?.PerformSkillMotion();
        }
    }
}