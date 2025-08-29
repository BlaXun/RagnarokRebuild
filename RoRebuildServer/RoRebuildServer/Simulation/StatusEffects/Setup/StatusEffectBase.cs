using RebuildSharedData.Data;
using RebuildSharedData.Enum;
using RoRebuildServer.EntityComponents;
using RoRebuildServer.EntityComponents.Character;
using RoRebuildServer.EntityComponents.Util;

namespace RoRebuildServer.Simulation.StatusEffects.Setup
{
    public class StatusEffectBase
    {
        public virtual float Duration => 5f;
        public virtual StatusUpdateMode UpdateMode { get; set; }
        public virtual bool TestApplication(CombatEntity ch, float testValue) => true;
        public virtual void OnApply(CombatEntity ch, ref StatusEffectState state) { }
        public virtual void OnExpiration(CombatEntity ch, ref StatusEffectState state) { }
        public virtual void OnRestore(CombatEntity ch, ref StatusEffectState state) => OnApply(ch, ref state);

        public virtual StatusUpdateResult OnUpdateTick(CombatEntity ch, ref StatusEffectState state) => StatusUpdateResult.Continue;
        public virtual StatusUpdateResult OnAttack(CombatEntity ch, ref StatusEffectState state, ref DamageInfo info) => StatusUpdateResult.Continue;
        
        /// Occurs when queued damage is applied. Note that this means the client has already has the damage number, so it's not a good place to nullify damage.
        /// Used to break ice, stone, sleep, and other such statuses on hit.
        public virtual StatusUpdateResult OnTakeDamage(CombatEntity ch, ref StatusEffectState state, ref DamageInfo info) => StatusUpdateResult.Continue;
        
        /// A CalculateCombatResult was finished. Lets you modify the results. EnergyCoat uses this to cut damage,
        /// Hiding uses this to let it ignore non-earth property attacks
        public virtual StatusUpdateResult OnCalculateDamage(CombatEntity ch, ref StatusEffectState state, ref AttackRequest req, ref DamageInfo info) => StatusUpdateResult.Continue;
        
        /// A CalculateCombatResult was called, but has not been executed. Used for sleep (guaranteed crit),
        /// magical attack (replaces atk with matk), and power up (increases skill mod)
        public virtual StatusUpdateResult OnPreCalculateDamage(CombatEntity ch, CombatEntity? target, ref StatusEffectState state, ref AttackRequest req) => StatusUpdateResult.Continue;
        public virtual StatusUpdateResult OnChangeEquipment(CombatEntity ch, ref StatusEffectState state) => StatusUpdateResult.Continue;
        public virtual StatusUpdateResult OnMove(CombatEntity ch, ref StatusEffectState state, Position src, Position dest, bool isTeleport) => StatusUpdateResult.Continue;
        public virtual StatusUpdateResult OnChangeMaps(CombatEntity ch, ref StatusEffectState state) => StatusUpdateResult.Continue;
    }
}
