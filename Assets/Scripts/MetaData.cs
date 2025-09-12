namespace PeraphsPizza.SaveSystem
{
    [System.Serializable]
    public class MetaData
    {
        public string SlotId;
        public string PlayerName;
        public double PlayTimeSeconds;
        public Days Day;
        public int Hours;
        public int Minutes;

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