
using System.Linq;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using AIO.Utilities;

#pragma warning disable 1587

namespace AIO.Champions
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal partial class Caitlyn
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Loads Caitlyn.
        /// </summary>
        public Caitlyn()
        {
            /// <summary>
            ///     Initializes the menus.
            /// </summary>
            Menus();

            /// <summary>
            ///     Initializes the methods.
            /// </summary>
            Methods();

            /// <summary>
            ///     Initializes the spells.
            /// </summary>
            Spells();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Fired on spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SpellBookCastSpellEventArgs" /> instance containing the event data.</param>
        public void OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.Slot)
                {
                    case SpellSlot.Q:
                        var safeQ = MenuClass.Spells["q"]["customization"]["safeq"];
                        if (safeQ != null &&
                            UtilityClass.Player.CountEnemyHeroesInRange(UtilityClass.Player.AttackRange) > safeQ.As<MenuSlider>().Value)
                        {
                            args.Process = false;
                        }
                        break;

                    case SpellSlot.W:
                        if (ObjectManager.Get<GameObject>().Any(m => m.Distance(args.End) <= SpellClass.W.Width && m.Name.Equals("caitlyn_Base_yordleTrap_idle_green.troy")))
                        {
                            args.Process = false;
                        }

                        // Logic to not cast Trap if enemy has been afflicted by it less than 4 seconds ago and he's still in the area.
                        // Note: The time the minion takes to actually be cancelled by the ObjectManager is exactly 4 seconds, that's why i check for its presence.
                        //       maybe a tiny bit more, but the check is solid.
                        var nearestEnemy = GameObjects.EnemyHeroes.MinBy(t => t.Distance(args.End));
                        if (ObjectManager.Get<Obj_AI_Minion>().Any(m => m.IsAlly && m.UnitSkinName == "CaitlynTrap" && m.Distance(nearestEnemy) <= SpellClass.W.Width && nearestEnemy.HasBuff("caitlynyordletrapsight")))
                        {
                            args.Process = false;
                        }
                        break;

                    case SpellSlot.E:
                        var safeE = MenuClass.Spells["e"]["customization"]["safee"];
                        if (safeE != null &&
                            UtilityClass.Player.ServerPosition.Extend(args.End, -400f).CountEnemyHeroesInRange(UtilityClass.Player.AttackRange) > safeE.As<MenuSlider>().Value)
                        {
                            args.Process = false;
                        }

                        if (Game.TickCount - UtilityClass.LastTick >= 1000 &&
                            ImplementationClass.IOrbwalker.Mode == OrbwalkingMode.None &&
                            MenuClass.Miscellaneous["reversede"].As<MenuBool>().Enabled)
                        {
                            UtilityClass.LastTick = Game.TickCount;
                            SpellClass.E.Cast(UtilityClass.Player.ServerPosition.Extend(Game.CursorPos, -SpellClass.E.Range));
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PostAttackEventArgs" /> instance containing the event data.</param>
        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Weaving(sender, args);
                    break;
                case OrbwalkingMode.Laneclear:
                    Jungleclear(sender, args);
                    break;
            }
        }

        /// <summary>
        ///     Fired on present.
        /// </summary>
        public void OnPresent()
        {
            /// <summary>
            ///     Initializes the drawings.
            /// </summary>
            Drawings();
        }

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Obj_AI_BaseMissileClientDataEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender.IsMe)
            {
                /// <summary>
                ///     Initializes the orbwalkingmodes.
                /// </summary>
                switch (ImplementationClass.IOrbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        switch (args.SpellData.Name)
                        {
                            case "CaitlynEntrapment":
                            case "CaitlynEntrapmentMissile":
                                if (SpellClass.W.Ready &&
                                    MenuClass.Spells["w"]["triplecombo"].As<MenuBool>().Enabled)
                                {
                                    var bestTarget = GameObjects.EnemyHeroes
                                        .Where(t => t.IsValidTarget(SpellClass.E.Range))
                                        .MinBy(o => o.Distance(args.End));
                                    if (bestTarget != null)
                                    {
                                        SpellClass.W.Cast(bestTarget.ServerPosition);
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Fired on an incoming gapcloser.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="Dash.DashArgs" /> instance containing the event data.</param>
        public void OnGapcloser(object sender, Dash.DashArgs args)
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }

            var gapSender = (Obj_AI_Hero)args.Unit;
            if (gapSender == null || !gapSender.IsEnemy || !gapSender.IsMelee)
            {
                return;
            }

            /// <summary>
            ///     The Anti-Gapcloser E.
            /// </summary>
            if (SpellClass.E.Ready &&
                args.EndPos.Distance(UtilityClass.Player.ServerPosition) < SpellClass.E.Range &&
                MenuClass.Spells["e"]["gapcloser"].As<MenuBool>().Enabled)
            {
                var playerPos = UtilityClass.Player.ServerPosition;
                if (args.EndPos.Distance(playerPos) >= 200)
                {
                    SpellClass.E.Cast(args.EndPos);
                }
                else
                {
                    SpellClass.E.Cast(playerPos.Extend(args.StartPos, -SpellClass.E.Range));
                }
            }

            /// <summary>
            ///     The Anti-Gapcloser W.
            /// </summary>
            if (SpellClass.W.Ready &&
                !Invulnerable.Check(gapSender, DamageType.Magical, false) &&
                args.EndPos.Distance(UtilityClass.Player.ServerPosition) < SpellClass.W.Range &&
                MenuClass.Spells["w"]["gapcloser"].As<MenuBool>().Enabled)
            {
                SpellClass.W.Cast(args.EndPos);
            }
        }

        /*
        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public void OnInterruptableTarget(object sender, Events.InterruptableTargetEventArgs args)
        {
            if (UtilityClass.Player.IsDead || Invulnerable.Check(args.Sender, DamageType.Magical, false))
            {
                return;
            }

            if (SpellClass.E.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.E.SpellData.Range)
                && MenuClass.Spells["e"]["interrupter"].As<MenuBool>().Enabled)
            {
                if (!SpellClass.E.GetPrediction(args.Sender).CollisionObjects.Any())
                {
                    SpellClass.E.Cast(SpellClass.E.GetPrediction(args.Sender).UnitPosition);
                }
            }

            if (SpellClass.W.State == SpellState.Ready && args.Sender.IsValidTarget(SpellClass.W.SpellData.Range)
                && MenuClass.Spells["w"]["interrupter"].As<MenuBool>().Enabled)
            {
                SpellClass.W.Cast(SpellClass.W.GetPrediction(args.Sender).CastPosition);
            }
        }
        */

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        public void OnUpdate()
        {
            if (UtilityClass.Player.IsDead)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            Killsteal();

            if (ImplementationClass.IOrbwalker.IsWindingUp)
            {
                return;
            }

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Automatic();

            /// <summary>
            ///     Initializes the orbwalkingmodes.
            /// </summary>
            switch (ImplementationClass.IOrbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo();
                    break;

                case OrbwalkingMode.Mixed:
                    Harass();
                    break;

                case OrbwalkingMode.Laneclear:
                    Laneclear();
                    break;
            }
        }

        #endregion
    }
}