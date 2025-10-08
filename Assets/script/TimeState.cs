using UnityEngine;

[System.Serializable]
public sealed class TimeState
{
    public int DayNumber { get; private set; }     // 0..∞
    public WeekDay Weekday { get; private set; }   // Mon..Sun
    public TimePeriod Period { get; private set; } // Morning..Evening

    public TimeState(int startDay = 0, WeekDay startWeekday = WeekDay.Mon, TimePeriod startPeriod = TimePeriod.Morning)
    {
        DayNumber = Mathf.Max(0, startDay);
        Weekday = startWeekday;
        Period = startPeriod;
    }

    /// <summary>เลื่อนไป 1 ช่วงเวลา (เช้า→บ่าย→เย็น→+วันใหม่เช้า)</summary>
    public void AdvanceOne()
    {
        switch (Period)
        {
            case TimePeriod.Morning:
                Period = TimePeriod.Afternoon;
                break;
            case TimePeriod.Afternoon:
                Period = TimePeriod.Evening;
                break;
            case TimePeriod.Evening:
                Period = TimePeriod.Morning;
                DayNumber++;
                Weekday = (WeekDay)(((int)Weekday + 1) % 7);
                break;
        }
    }

    public override string ToString() => $"Day {DayNumber} | {Weekday} | {Period}";
}