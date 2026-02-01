namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileşim türleri.
    /// Types of interaction.
    /// </summary>
    public enum InteractionType
    {
        /// <summary>
        /// Tek tuş basımı ile anında etkileşim.
        /// Instant interaction with a single button press.
        /// </summary>
        Instant,

        /// <summary>
        /// Basılı tutma gerektiren etkileşim.
        /// Interaction requiring a press and hold.
        /// </summary>
        Hold,

        /// <summary>
        /// Açık/kapalı toggle etkileşimi.
        /// Open/closed toggle interaction.
        /// </summary>
        Toggle
    }
}