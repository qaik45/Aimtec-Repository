﻿using Aimtec.SDK.Menu;

namespace NabbTracker
{
    /// <summary>
    ///     The Utility class.
    /// </summary>
    internal class MenuClass
    {
        #region Public Properties

        /// <summary>
        ///     The Colorblind Menu.
        /// </summary>
        public static Menu ColorblindMenu { get; set; }

        /// <summary>
        ///     The ExpTracker Menu.
        /// </summary>
        public static Menu ExpTracker { get; set; }

        /// <summary>
        ///     The Main Menu.
        /// </summary>
        public static Menu Menu { get; set; }

        /// <summary>
        ///     The Miscellaneous Menu.
        /// </summary>
        public static Menu Miscellaneous { get; set; }

        /// <summary>
        ///     The SpellTracker Menu.
        /// </summary>
        public static Menu SpellTracker { get; set; }

        /// <summary>
        ///     The TowerRangeTracker Menu.
        /// </summary>
        public static Menu TowerRangeTracker { get; set; }

        /// <summary>
        ///     The AttackRangeTracker Menu.
        /// </summary>
        public static Menu AttackRangeTracker { get; set; }

        #endregion
    }
}