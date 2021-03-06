
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
    internal partial class Jinx
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                !UtilityClass.Player.IsUnderEnemyTurret() &&
                UtilityClass.Player.CountEnemyHeroesInRange(SpellClass.Q.Range) < 3 &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
            {
                var bestTarget = SpellClass.W.GetBestKillableHero(DamageType.Physical);
                if (bestTarget != null &&
                    !bestTarget.IsValidTarget(SpellClass.Q2.Range))
                {
                    if (UtilityClass.Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.GetRealHealth())
                    {
                        SpellClass.W.Cast(bestTarget);
                    }
                }
            }
        }

        #endregion
    }
}