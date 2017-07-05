namespace AIO.Champions
{
    using Aimtec;

    using AIO.Utilities;

    /// <summary>
    ///     The methods class.
    /// </summary>
    internal partial class Tristana
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public void Methods()
        {
            Game.OnUpdate += this.OnUpdate;
            BuffManager.OnAddBuff += this.OnAddBuff;
            UtilityClass.IOrbwalker.PreAttack += this.OnPreAttack;
            RenderManager.OnPresent += this.OnPresent;

            /*
            Events.OnGapCloser += OnGapCloser;
            Events.OnInterruptableTarget += OnInterruptableTarget;
            */
        }

        #endregion
    }
}