using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Data container for time and date information
    /// </summary>
    [System.Serializable]
    [DataTypeId(4)]
    public class TimeDateData : PureRawData
    {
        /// <summary>Current day of the week</summary>
        public Days _day;
        
        /// <summary>Current minutes</summary>
        public int _minutes;
        
        /// <summary>Current hours</summary>
        public int _hours;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TimeDateData() { }

        /// <summary>
        /// Full constructor for creating time/date data
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <param name="day">Day of the week</param>
        /// <param name="hours">Hour of the day</param>
        /// <param name="minutes">Minutes</param>
        [JsonConstructor]
        public TimeDateData(string id, Days day, int hours, int minutes)
        {
            this._id = id;
            this._day = day;
            this._hours = hours;
            this._minutes = minutes;
        }

        /// <summary>
        /// Update time/date data with new values
        /// </summary>
        /// <param name="day">New day</param>
        /// <param name="hours">New hours</param>
        /// <param name="minutes">New minutes</param>
        public void UpdateData(Days day, int hours, int minutes)
        {
            this._day = day;
            this._hours = hours;
            this._minutes = minutes;
        }
    }
}