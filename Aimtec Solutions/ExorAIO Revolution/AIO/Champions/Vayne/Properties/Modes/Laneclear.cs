﻿
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Orbwalking;

    using AIO.Utilities;

    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Vayne
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public static void Laneclear(object sender, PostAttackEventArgs args)
        {
            /// <summary>
            ///     The Q FarmHelper Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent() >
                    ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["q"]["farmhelper"]) &&
                MenuClass.Spells["q"]["farmhelper"].As<MenuSliderBool>().Enabled)
            {
                var posAfterQ = UtilityClass.Player.Position.Extend(Game.CursorPos, 300f);
                if (UtilityClass.GetEnemyLaneMinionsTargetsInRange(SpellClass.Q.Range).Any(m =>
                        m.Distance(posAfterQ) < UtilityClass.Player.AttackRange &&
                        posAfterQ.CountEnemyHeroesInRange(UtilityClass.Player.AttackRange) <= 2 &&
                        m.GetRealHealth() < UtilityClass.Player.GetAutoAttackDamage(m) + UtilityClass.Player.GetSpellDamage(m, SpellSlot.Q)))
                {
                    SpellClass.Q.Cast(Game.CursorPos);
                }
            }
        }

    }

    #endregion
}