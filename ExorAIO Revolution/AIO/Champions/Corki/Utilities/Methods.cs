namespace AIO.Champions
{
    using Aimtec;

    using AIO.Utilities;

    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Corki
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += this.OnUpdate;
            Render.OnPresent += this.OnPresent;
            ImplementationClass.IOrbwalker.PostAttack += this.OnPostAttack;
            ImplementationClass.IOrbwalker.PreAttack += this.OnPreAttack;
            ImplementationClass.IOrbwalker.OnNonKillableMinion += this.OnNonKillableMinion;
        }

        #endregion
    }
}