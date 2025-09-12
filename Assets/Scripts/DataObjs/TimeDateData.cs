using Newtonsoft.Json;
using UnityEngine;
using PeraphsPizza.SaveSystem;

namespace PeraphsPizza.SaveSystem
{
    [DataTypeId(4)]
    public class TimeDateData : PureRawData
    {
        public Days _day;
        public int _minutes;
        public int _hours;

        public TimeDateData() { }

        [JsonConstructor]
        public TimeDateData(string id, Days day, int hours, int minutes)
        {
            this._id = id;
            this._day = day;
            this._hours = hours;
            this._minutes = minutes;
        }

        public void UpdateData(Days day, int hours, int minutes)
        {
            this._day = day;
            this._hours = hours;
            this._minutes = minutes;
        }
    }
}

