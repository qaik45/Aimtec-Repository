
#pragma warning disable 1587

namespace AIO.Champions
{
    using System.Linq;

    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;

    using AIO.Utilities;

    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Lucian
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void Harass()
        {
            /// <summary>
            ///     The Extended Q Mixed Harass Logic.
            /// </summary>
            if (SpellClass.Q.Ready &&
                UtilityClass.Player.ManaPercent()
                > ManaManager.GetNeededMana(SpellClass.Q.Slot, MenuClass.Spells["extendedq"]["mixed"]) &&
                MenuClass.Spells["extendedq"]["mixed"].As<MenuSliderBool>().Enabled)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(SpellClass.Q.Range);
                foreach (var minion in from minion in Extensions.GetAllGenericUnitTargetsInRange(SpellClass.Q.Range)
                                       let polygon = new Geometry.Rectangle((Vector2)UtilityClass.Player.Position, (Vector2)UtilityClass.Player.Position.Extend(minion.Position, SpellClass.Q2.Range), SpellClass.Q2.Width)
                                       where polygon.IsInside((Vector2)SpellClass.Q2.GetPrediction(target).PredictedPosition)
                                       select minion)
                {
                    if (MenuClass.Spells["extendedq"]["whitelist"][target.ChampionName.ToLower()].Enabled)
                    {
                        SpellClass.Q.CastOnUnit(minion);
                    }
                }
            }
        }

        #endregion
    }
}