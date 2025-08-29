using RebuildSharedData.Data;
using RebuildSharedData.Enum;
using RebuildSharedData.Enum.EntityStats;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.EntityComponents.Util;
using RoRebuildServer.Networking;
using RoRebuildServer.Simulation.Util;
using System.Diagnostics;
using CsvHelper.Configuration.Attributes;
using RoRebuildServer.Data;
using RoRebuildServer.Logging;
using RoRebuildServer.Simulation.Pathfinding;

namespace RoRebuildServer.Simulation.Skills.SkillHandlers.Ninja;

[SkillHandler(CharacterSkill.ShadowLeap, SkillClass.None, SkillTarget.Ground)]
public class ShadowLeapHandler : SkillHandlerBase
{
    public override int GetSkillRange(CombatEntity source, int lvl)  {
        return 5 + lvl;
    }

    public override bool UsableWhileHidden => true;

    // Can only use when in hiding state
    public override bool PreProcessValidation(CombatEntity source, CombatEntity? target, Position position, int lvl,
        bool isIndirect, bool isItemSource) {
        if (!source.HasStatusEffectOfType(CharacterStatusEffect.Hiding)) {
            CommandBuilder.ErrorMessage(source.Player, $"You need to be in hiding to use this skill.");
            return false;
        }
        return true;
    }
    
    public override void Process(CombatEntity source, CombatEntity? target, Position position, int lvl, bool isIndirect,
        bool isItemSource)
    {
        var character = source.Character;
        var map = character.Map;
        lvl = int.Clamp(lvl, 1, 5);

        if (map == null)
            return;
        
        if (map.WalkData.HasDirectPathAccess(character.Position, position)) {

            if (character.Position != position) {
                map.ChangeEntityPosition3(character, character.WorldPosition, position, false);
                character.StopMovingImmediately();
            }
            
            // Collect a list of all visible characters and inform them about the new position of the skill user
            map.AddVisiblePlayersAsPacketRecipients(character);
            CommandBuilder.SendMoveEntityMulti(character);
            
            // Make sure we look to the correct direction after leaping
            character.ChangeLookDirection(position);
            source.RemoveStatusOfTypeIfExists(CharacterStatusEffect.Hiding);
            
            CommandBuilder.SkillExecuteSelfTargetedSkillAutoVis(source.Character, CharacterSkill.ShadowLeap, lvl, isIndirect);
            CommandBuilder.ClearRecipients();
        } 
    }
}