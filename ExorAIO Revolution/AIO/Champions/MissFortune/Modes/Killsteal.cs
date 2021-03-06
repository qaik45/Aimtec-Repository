
using System.Collections.Generic;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class MissFortune
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The Q Killsteal Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["extendedq"]["killsteal"].As<MenuBool>().Enabled)
            {
                var unitsInQRange = Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range);
                var unitsToIterateIfPlayerNormallyKillable =
                    unitsInQRange.Any(m => m.GetRealHealth() < UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q))
                        ? unitsInQRange.Where(m => m.GetRealHealth() < UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q)).ToList()
                        : unitsInQRange.ToList();
                var unitsToIterateIfPlayerEmpoweredKillable =
                        unitsToIterateIfPlayerNormallyKillable.Where(u => u.GetRealHealth() < UtilityClass.Player.GetSpellDamage(u, SpellSlot.Q)).ToList();

                List<Obj_AI_Base> unitsToIterate = null;
                var heroMultiplier = GetHeroLoveTapDamageMultiplier();
                foreach (var hero in Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.Q2.Range))
                {
                    if (UtilityClass.Player.GetSpellDamage(hero, SpellSlot.Q) + heroMultiplier >= hero.GetRealHealth())
                    {
                        unitsToIterate = unitsToIterateIfPlayerNormallyKillable;
                    }
                    else if (UtilityClass.Player.GetSpellDamage(hero, SpellSlot.Q, DamageStage.Empowered) + heroMultiplier >= hero.GetRealHealth())
                    {
                        unitsToIterate = unitsToIterateIfPlayerEmpoweredKillable;
                    }

                    if (unitsToIterate != null)
                    {
                        foreach (var minion in unitsToIterate)
                        {
                            var polygon = QCone(minion);
                            if (polygon.IsInside((Vector2)hero.ServerPosition) &&
                                (LoveTapTargetNetworkId == hero.NetworkId || GameObjects.EnemyMinions.All(m => polygon.IsOutside((Vector2)m.ServerPosition))) &&
                                polygon.IsInside((Vector2)SpellClass.Q.GetPrediction(hero).CastPosition))
                            {
                                if (UtilityClass.Player.GetSpellDamage(hero, SpellSlot.Q, DamageStage.Empowered) >= hero.GetRealHealth())
                                {
                                    SpellClass.Q.CastOnUnit(minion);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}