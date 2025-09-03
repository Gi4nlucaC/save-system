using TMPro;
using UnityEngine;

public class TimeOfTheDayComponent : SavableMonoBehaviour, ISavable
{
    [SerializeField] TMP_Text _dayText;
    [SerializeField] TMP_Text _timeText;

    public Days currentDay = Days.Monday;
    public int hour = 7;
    public int minute = 0;

    public float realSecondsPerGameMinute = 1f;

    private float timer = 0f;

    TimeDateData _timeDateData;

    private void Start()
    {
        Initialize();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= realSecondsPerGameMinute)
        {
            timer = 0f;
            AdvanceTime();
        }
    }

    void AdvanceTime()
    {
        minute++;

        if (minute >= 60)
        {
            minute = 0;
            hour++;

            if (hour >= 24)
            {
                hour = 0;
                AdvanceDay();
            }
        }

        Debug.Log($"{currentDay} {hour:D2}:{minute:D2}");
        _dayText.text = currentDay.ToString();
        _timeText.text = $"{hour:D2}:{minute:D2}";
    }

    void AdvanceDay()
    {
        if (currentDay == Days.Sunday)
            currentDay = Days.Monday;
        else
            currentDay++;
    }

    public override PureRawData SaveData()
    {
        SnapshotData();

        return _timeDateData;
    }

    public override void LoadData()
    {
        if (SaveSystemManager.ExistData(_persistentId.Value) >= 0)
        {
            _timeDateData = SaveSystemManager.GetData(_persistentId.Value) as TimeDateData;

            currentDay = _timeDateData._day;
            hour = _timeDateData._hours;
            minute = _timeDateData._minutes;
        }
        else
            _timeDateData = new(_persistentId.Value, Days.Monday, 7, 0);
    }

    public override void DeleteData()
    {
        throw new System.NotImplementedException();
    }

    public override void SnapshotData()
    {
        _timeDateData.UpdateData(currentDay, hour, minute);
    }
}
