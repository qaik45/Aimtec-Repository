
using System.Linq;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
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
            if (SpellClass.Q.Ready)
            {
                /// <summary>
                ///     Normal.
                /// </summary>
                if (MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
                {
                    var bestTarget = SpellClass.Q.GetBestKillableHero(DamageType.Physical);
                    if (bestTarget != null &&
                        !bestTarget.IsValidTarget(UtilityClass.Player.GetFullAttackRange(bestTarget)) &&
                        UtilityClass.Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.GetRealHealth())
                    {
                        SpellClass.Q.CastOnUnit(bestTarget);
                    }
                }

                /// <summary>
                ///     Extended.
                /// </summary>
                if (MenuClass.Spells["extendedq"]["killsteal"].As<MenuBool>().Enabled &&
                    Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range).Any(m => m.IsValidTarget(SpellClass.Q.Range)))
                {
                    var target = SpellClass.Q2.GetBestKillableHero(DamageType.Physical);
                    foreach (var minion in from minion in Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range).Where(m => m.IsValidTarget(SpellClass.Q.Range))
                                           let polygon = new Geometry.Rectangle(
                                                                (Vector2)UtilityClass.Player.ServerPosition,
                                                                (Vector2)UtilityClass.Player.ServerPosition.Extend(minion.ServerPosition, SpellClass.Q2.Range-150f),
                                                                SpellClass.Q2.Width)
                                           where
                                                target != null &&
                                                target != minion &&
                                                polygon.IsInside((Vector2)SpellClass.Q2.GetPrediction(target).CastPosition) &&
                                                UtilityClass.Player.GetSpellDamage(target, SpellSlot.Q) >= target.GetRealHealth()
                        select minion)
                    {
                        SpellClass.Q.CastOnUnit(minion);
                    }
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
            {
                var bestTarget = SpellClass.W.GetBestKillableHero(DamageType.Magical);
                if (bestTarget != null &&
                    !bestTarget.IsValidTarget(UtilityClass.Player.GetFullAttackRange(bestTarget)) &&
                    UtilityClass.Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.GetRealHealth())
                {
                    SpellClass.W.Cast(bestTarget);
                }
            }
        }

        #endregion
    }
}