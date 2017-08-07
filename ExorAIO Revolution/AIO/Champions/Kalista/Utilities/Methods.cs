namespace AIO.Champions
{
    using Aimtec;

    using AIO.Utilities;

    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Kalista
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += this.OnUpdate;
            ImplementationClass.IOrbwalker.PreAttack += this.OnPreAttack;
            Render.OnPresent += this.OnPresent;
            ImplementationClass.IOrbwalker.OnNonKillableMinion += this.OnNonKillableMinion;
        }

        #endregion
    }
}