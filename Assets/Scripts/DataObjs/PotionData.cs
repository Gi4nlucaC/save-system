using PeraphsPizza.SaveSystem;

namespace PeraphsPizza.SaveSystem
{
    [DataTypeId(5)]
    public class PotionData : PureRawData
    {
        public float _nextSpawnTimer;
        public int _healAmount;

        public void UpdateData(float spawnTimer)
        {
            this._nextSpawnTimer = spawnTimer;
        }
    }
}