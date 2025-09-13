namespace SaveSystem
{
    /// <summary>
    /// Metadata container for save file headers
    /// </summary>
    [System.Serializable]
    public class MetaData
    {
        /// <summary>Unique identifier for the save slot</summary>
        public string SlotId;
        
        /// <summary>Player character name</summary>
        public string PlayerName;
        
        /// <summary>Total play time in seconds</summary>
        public double PlayTimeSeconds;
        
        /// <summary>Current in-game day</summary>
        public Days Day;
        
        /// <summary>Current in-game hours</summary>
        public int Hours;
        
        /// <summary>Current in-game minutes</summary>
        public int Minutes;

        /// <summary>
        /// Merge data from another MetaData instance
        /// </summary>
        /// <param name="other">Other MetaData to merge from</param>
        public void Merge(MetaData other)
        {
            if (!string.IsNullOrEmpty(other.PlayerName)) PlayerName = other.PlayerName;
            if (other.Day != default) Day = other.Day;
            if (other.Hours != 0) Hours = other.Hours;
            if (other.Minutes != 0) Minutes = other.Minutes;
            if (other.PlayTimeSeconds != 0) PlayTimeSeconds = other.PlayTimeSeconds;
        }
    }
}