﻿// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable InconsistentNaming


#pragma warning disable 1587

namespace AIO.Utilities
{
    using System;
    using System.Collections.Generic;

    using Aimtec;
    using Aimtec.SDK.Extensions;

    /// <summary>
    ///     The Utility class.
    /// </summary>
    internal static class UtilityClass
    {
        #region Static Fields

        /// <summary>
        ///     The last tick.
        /// </summary>
        public static int LastTick = 0;

        /// <summary>
        ///     Gets the Player.
        /// </summary>
        public static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();

        /// <summary>
        ///     The jungle HP bar offset list.
        /// </summary>
        internal static readonly string[] JungleList =
            {
                "SRU_Dragon_Air", "SRU_Dragon_Fire", "SRU_Dragon_Water",
                "SRU_Dragon_Earth", "SRU_Dragon_Elder", "SRU_Baron",
                "SRU_RiftHerald", "SRU_Red", "SRU_Blue", "SRU_Gromp",
                "Sru_Crab", "SRU_Krug", "SRU_Razorbeak", "SRU_Murkwolf"
            };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The Traps in the game and their position.
        /// </summary>
        public static Dictionary<int, Tuple<Vector3, float>> ActualTraps = new Dictionary<int, Tuple<Vector3, float>>();

        /// <summary>
        ///     The Trap names and their collision radius.
        /// </summary>
        public static Dictionary<string, float> Traps = new Dictionary<string, float>
            {
                { "Jinx_Base_E_Mine_Ready_Green.troy",      50f   },
                { "Caitlyn_Base_W_Indicator_SizeRing.troy", 67.5f },
                { "ZiggsTrap",                              30f   }, // TODO: Find the real name of the ziggs traps.
                { "Nidalee_Base_W_TC_Green.troy",           75f   },
                { "Taliyah_Base_E_Mines.troy",              30f   }
            };

        /// <summary>
        ///     Gets the angle by 'degrees' degrees.
        /// </summary>
        /// <param name="degrees">
        ///     The angle degrees.
        /// </param>
        /// <returns>
        ///     The angle by 'degrees' degrees.
        /// </returns>
        public static float GetAngleByDegrees(float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        /// <summary>
        ///     Gets the remaining buff time of the 'buff' Buff.
        /// </summary>
        /// <param name="buff">
        ///     The buff.
        /// </param>
        /// <returns>
        ///     The remaining buff time.
        /// </returns>
        public static double GetRemainingBuffTime(this Buff buff)
        {
            return buff.EndTime - Game.ClockTime;
        }

        /// <summary>
        ///     Gets the health with Blitzcrank's Shield support.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The target Health with Blitzcrank's Shield support.
        /// </returns>
        public static float GetRealHealth(this Obj_AI_Base unit)
        {
            var debuffer = 0f;

            /// <summary>
            ///     Gets the predicted reduction from Blitzcrank Shield.
            /// </summary>
            var hero = unit as Obj_AI_Hero;
            if (hero != null)
            {
                if (hero.ChampionName.Equals("Blitzcrank") && !hero.HasBuff("BlitzcrankManaBarrierCD"))
                {
                    debuffer += hero.Mana / 2;
                }
            }
            return unit.Health + unit.PhysicalShield + unit.HPRegenRate + debuffer;
        }

        #endregion
    }
}