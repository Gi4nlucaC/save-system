public class PotionData : PureRawData
{
    public float _newxSpawnTimer;

    public void UpdateData(float spawnTimer)
    {
        this._newxSpawnTimer = spawnTimer;
    }
}