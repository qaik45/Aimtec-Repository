
#pragma warning disable 1587

namespace AIO.Champions
{
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using Utilities;

    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public static void Killsteal()
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Value)
            {
                var bestTarget = SpellClass.Q.GetBestKillableHero(DamageType.Magical);
                if (bestTarget != null &&
                    UtilityClass.Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.GetRealHealth())
                {
                    SpellClass.Q.Cast(bestTarget);
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Value)
            {
                foreach (var target in Extensions.GetBestEnemyHeroesTargetsInRange(SpellClass.W.Range))
                {
                    if (target.IsValidTarget(SpellClass.W.Width, false, BallPosition) &&
                        UtilityClass.Player.GetSpellDamage(target, SpellSlot.W) >= target.GetRealHealth())
                    {
                        SpellClass.W.Cast();
                    }
                }
            }
        }

        #endregion
    }
}