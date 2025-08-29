using RebuildSharedData.Enum;
using RoRebuildServer.EntityComponents.Character;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.Simulation.StatusEffects.Setup;
using RoRebuildServer.EntityComponents.Util;
using RoRebuildServer.Networking;
using RoRebuildServer.EntitySystem;

namespace RoRebuildServer.Simulation.StatusEffects.ExpandedJobs;

/**
 * ToDo: Cicada is ignored by some physical skills
 * Will need to implement that once we got those skills
 * Bomb, Acide Terror, Metero Assault, Cart Termination, Bull's Eye, Throw Venom Knife
 */
[StatusEffectHandler(CharacterStatusEffect.CicadaSkinShed, StatusClientVisibility.Everyone, StatusEffectFlags.NoSave, "CicadaSkinShed")]
public class StatusCicadaSkinShed : StatusEffectBase
{
    private float castDelay = 1f;
    private int remainingEvasions;
    private readonly int maxCicadaEvasionDistance = 7;
    
    public override StatusUpdateMode UpdateMode => StatusUpdateMode.OnCalculateDamageTaken | StatusUpdateMode.OnUpdate;

    public override void OnApply(CombatEntity ch, ref StatusEffectState state) {
        switch (state.Value1) {
            case 1:
            case 2:
                remainingEvasions = 1;
                break;
            
            case 3:
            case 4:
                remainingEvasions = 2;
                break;
            case 5:
                remainingEvasions = 3;
                break;
        }
        
        if (ch.Character.State != CharacterState.Dead)
            ch.Character.State = CharacterState.Idle;
    }
    
    //Value1: Skill level

    public override StatusUpdateResult OnCalculateDamage(CombatEntity ch, ref StatusEffectState state,
        ref AttackRequest req, ref DamageInfo info) {
        // Cancel out any physical attack and perform backslide
        if (req.Flags.HasFlag(AttackFlags.Physical)) {
            PerformEvasion(ch, info.Source, info.IsIndirect);
            info.SetAttackToMiss();
            if (remainingEvasions <= 0 || ch.Character.State == CharacterState.Dead)
                return StatusUpdateResult.EndStatus;
        }
        
        return StatusUpdateResult.Continue;
    }

    private void PerformEvasion(CombatEntity ch, Entity attacker, bool isIndirect) {
        var skillUser = ch.Character;
        if (skillUser.Map == null)
            return;

        remainingEvasions--;
        
        if(skillUser.Type == CharacterType.Player)
            ch.ApplyCooldownForSupportSkillAction(castDelay);
        else
            ch.ApplyCooldownForAttackAction(castDelay);

        // Determine facing direction from player to monster to use that for the backslide
        // attacker.Get<WorldObject>().FacingDirection
        var facingDirection = (attacker.Get<WorldObject>().Position - ch.Character.Position).Normalize().GetDirectionForOffset();
        
        var pos = skillUser.Map.WalkData.CalcKnockbackFromPosition(skillUser.Position, skillUser.Position.AddDirectionToPosition(facingDirection), maxCicadaEvasionDistance);
        if (skillUser.Position != pos)
            skillUser.Map.ChangeEntityPosition3(skillUser, skillUser.WorldPosition, pos, false);

        skillUser.StopMovingImmediately();
        skillUser.Map?.AddVisiblePlayersAsPacketRecipients(skillUser);

        // Inform all users near to the caster about the movement 
        CommandBuilder.SendMoveEntityMulti(skillUser);
        CommandBuilder.ClearRecipients();
    }

    public override StatusUpdateResult OnTakeDamage(CombatEntity ch, ref StatusEffectState state, ref DamageInfo info) {
        
        // Cancel status if no evasions remain or we are... dead ;(
        if (remainingEvasions <= 0 || ch.Character.State == CharacterState.Dead)
            return StatusUpdateResult.EndStatus;

        return StatusUpdateResult.Continue;
    }
}