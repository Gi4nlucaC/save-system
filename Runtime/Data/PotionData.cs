namespace SaveSystem
{
    /// <summary>
    /// Data container for potion information
    /// </summary>
    [System.Serializable]
    [DataTypeId(5)]
    public class PotionData : PureRawData
    {
        /// <summary>Time until next spawn</summary>
        public float _nextSpawnTimer;
        
        /// <summary>Amount of health this potion restores</summary>
        public int _healAmount;

        /// <summary>
        /// Update potion data with new spawn timer
        /// </summary>
        /// <param name="spawnTimer">New spawn timer value</param>
        public void UpdateData(float spawnTimer)
        {
            this._nextSpawnTimer = spawnTimer;
        }
    }
}