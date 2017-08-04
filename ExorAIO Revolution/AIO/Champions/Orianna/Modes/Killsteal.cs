
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using AIO.Utilities;

    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Orianna
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on tick update.
        /// </summary>
        public void Killsteal()
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                MenuClass.Spells["q"]["killsteal"].As<MenuBool>().Enabled)
            {
                var bestTarget = SpellClass.Q.GetBestKillableHero(DamageType.Magical);
                if (bestTarget != null &&
                    UtilityClass.Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.GetRealHealth())
                {
                    SpellClass.Q.GetPredictionInput(bestTarget).From = this.BallPosition;
                    SpellClass.Q.Cast(SpellClass.Q.GetPrediction(bestTarget).CastPosition);
                }
            }

            /// <summary>
            ///     The KillSteal W Logic.
            /// </summary>
            if (SpellClass.W.Ready &&
                MenuClass.Spells["w"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(t =>
                        t.IsValidTarget(SpellClass.W.Width, false, false, this.BallPosition) &&
                        UtilityClass.Player.GetSpellDamage(t, SpellSlot.W) >= t.GetRealHealth()))
                {
                    SpellClass.W.Cast();
                    return;
                }
            }

            /// <summary>
            ///     The KillSteal R Logic.
            /// </summary>
            if (SpellClass.R.Ready &&
                MenuClass.Spells["r"]["killsteal"].As<MenuBool>().Enabled)
            {
                if (GameObjects.EnemyHeroes.Any(t =>
                        t.IsValidTarget(SpellClass.R.Width, false, false, this.BallPosition) &&
                        UtilityClass.Player.GetSpellDamage(t, SpellSlot.R) >= t.GetRealHealth()))
                {
                    SpellClass.R.Cast();
                }
            }
        }

        #endregion
    }
}