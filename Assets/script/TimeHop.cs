using TMPro;
using UnityEngine;
using System;

public class TimeHop : MonoBehaviour
{
    public static TimeHop Instance { get; private set; }

    public enum TimePeriod { Morning, Afternoon, Evening }
    public enum WeekDay { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }

    [Header("Initial State")]
    [SerializeField] private WeekDay startWeekday = WeekDay.Monday;
    [SerializeField] private TimePeriod startPeriod = TimePeriod.Morning;
    [SerializeField] private int startDayNumber = 0;

    [Header("Optional UI")]
    [SerializeField] private TextMeshProUGUI textDayTime;

    // State (เปลี่ยนชื่อให้ชัดว่าเป็น “ปัจจุบัน”)
    private WeekDay currentWeekday;
    private TimePeriod currentPeriod;
    private int currentDayNumber;

    // Event แจ้งเตือนเมื่อเวลาเปลี่ยน (ระบบอื่น subscribe ได้)
    public event Action<int, WeekDay, TimePeriod> OnTimeChanged;

    // Read-only properties (ให้ระบบอื่นอ่านสถานะได้)
    public int DayNumber => currentDayNumber;
    public WeekDay Day => currentWeekday;
    public TimePeriod Period => currentPeriod;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        currentWeekday = startWeekday;
        currentPeriod = startPeriod;
        currentDayNumber = Mathf.Max(0, startDayNumber);
        RefreshUI();
    }

    // เลื่อน 1 ช่วงเวลา
    public void AdvanceOne()
    {
        switch (currentPeriod)
        {
            case TimePeriod.Morning: 
                currentPeriod = TimePeriod.Afternoon; 
                break;

            case TimePeriod.Afternoon: 
                currentPeriod = TimePeriod.Evening; 
                break;

            case TimePeriod.Evening:
                currentPeriod = TimePeriod.Morning;
                currentDayNumber++;
                currentWeekday = (WeekDay)(((int)currentWeekday + 1) % 7);
                break;
        }
        OnTimeChanged?.Invoke(currentDayNumber, currentWeekday, currentPeriod);
        RefreshUI();
    }

    // เผื่ออยากข้ามหลายช่วง
    public void Advance(int hops)
    {
        int n = Mathf.Max(1, hops);
        for (int i = 0; i < n; i++) 
        { 
            AdvanceOne(); 
        }
    }

    private void RefreshUI()
    {
        if (textDayTime != null)
        {
            textDayTime.text = $"{currentPeriod}\nDay {currentDayNumber}\n{currentWeekday}";
        }
    }

    // แยกทริกเกอร์ออก *ได้* แต่ถ้ายังอยากอยู่ที่เดิม ใส่จำกัด tag ชัดเจน
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            AdvanceOne();
        }
    }
}
