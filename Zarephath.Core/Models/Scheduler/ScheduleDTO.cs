using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.Scheduler
{
    public class ScheduleDTO
    {
        private int _frequencyChoice;
        private DateTime? _endDate;

        public int ID { get; set; }
        public string Title { get; set; }

        public long PayorId { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public bool IsVirtualVisit { get; set; }

        public string PayorName { get; set; }
        public string CareType { get; set; }

        public string EmployeeFullName { get; set; }

        public string PatientFullName { get; set; }

        public RecurrencePattern ScheduleRecurrence { get; set; }
        public string TempScheduleRecurrence { get; set; }

        public int AnniversaryDay { get; set; }
        public int AnniversaryMonth { get; set; }
        public long NurseScheduleID { get; set; }

        public int FrequencyChoice
        {
            get { return _frequencyChoice; }
            set
            {
                _frequencyChoice = value;
                CalculateFrequency();
                CalculateRecurrencePattern();
            }
        }

        public int Frequency { get; set; }
        public int DaysOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public bool IsMonthlyDaySelection { get; set; }
        public int DailyInterval { get; set; }
        public int WeeklyInterval { get; set; }
        public int MonthlyInterval { get; set; }
        public int? NumberOfOccurrences { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ScheduleStartDate { get; set; }
        public DateTime? ScheduleEndDate { get; set; }
        public string Notes { get; set; }

        public bool IsAnyDay { get; set; }
        public bool AnyTimeClockIn { get; set; }
        public string ClockInStartTime { get; set; }
        public string ClockInEndTime { get; set; }
        public DateTime StartDateTime
        {
            get
            {
                return StartDate + StartTime;
            }
            set
            {
                StartDate = value.Date;
                StartTime = value.TimeOfDay;
            }
        }

        public DateTime? EndDateTime
        {
            get
            {
                if (Frequency == 0) // one-time only 
                    return (StartDate + EndTime);

                return (_endDate.HasValue) ? _endDate : null;
            }
            set
            {
                _endDate = value;

                var ts = (EndDateTime - StartDate);
                if (!ts.HasValue)
                {
                    return;
                }

                if (ts.Value.Days == 0)
                    Frequency = 0;
            }
        }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime? EndDate { get; set; }
        public DateTime? OriginalEndDate { get; set; }
        public DateTime? OriginalStartDate { get; set; }
        public bool IsEndDateNull { get; set; }

        public bool IsSundaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sun);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sun))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Sun;
                }
            }
        }

        public bool IsMondaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Mon);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Mon))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Mon;
                }
            }
        }

        public bool IsTuesdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Tue);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Tue))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Tue;
                }
            }
        }

        public bool IsWednesdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Wed);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Wed))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Wed;
                }
            }
        }

        public bool IsThursdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Thu);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Thu))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Thu;
                }
            }
        }

        public bool IsFridaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Fri);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Fri))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Fri;
                }
            }
        }

        public bool IsSaturdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sat);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sat))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Sat;
                }
            }
        }

        public bool IsFirstWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.First);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.First))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.First;
                }
            }
        }

        public bool IsSecondWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Second);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Second))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Second;
                }
            }
        }

        public long ScheduleID { get; set; }
        public long EmployeeId { get; set; }
        public long ReferralId { get; set; }
        public string CareTypeId { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public bool IsThirdWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Third);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Third))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Third;
                }
            }
        }

        public bool IsFourthWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Fourth);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Fourth))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Fourth;
                }
            }
        }

        public bool IsLastWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Last);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Last))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Last;
                }
            }
        }

        /// <summary>
        /// The frequency expressed as enumeration.
        /// </summary>
        public FrequencyTypeEnum FrequencyTypeOptions
        {
            get
            {
                return (FrequencyTypeEnum)Frequency;
            }
            set
            {
                Frequency = (int)value;
            }
        }

        public string TempFrequencyTypeOptions
        {
            get; set;
        }

        /// <summary>
        /// The monthly interval expressed as enumeration
        /// </summary>
        public MonthlyIntervalEnum MonthlyIntervalOptions
        {
            get
            {
                return (MonthlyIntervalEnum)MonthlyInterval;
            }
            set
            {
                MonthlyInterval = (int)value;
            }
        }

        public string TempMonthlyIntervalOptions
        { get; set; }

        /// <summary>
        /// The days of the week expressed as enumeration.
        /// </summary>
        public DayOfWeekEnum DaysOfWeekOptions
        {
            get
            {
                return (DayOfWeekEnum)DaysOfWeek;
            }
            set
            {
                DaysOfWeek = (int)value;
            }
        }

        public string TempDaysOfWeekOptions
        { get; set; }

        public Schedule Schedule
        {
            get
            {
                return BuildSchedule();
            }
        }

        /// <summary>
        /// Returns a schedule from the ScheduleWidget engine based on the 
        /// properties of this recurring schedule.
        /// </summary>
        /// <returns></returns>
        private Schedule BuildSchedule()
        {
            // create a new instance of each recurring event
            var recurringEvent = new Event()
            {
                ID = (int)ScheduleID,
                Title = Title,
                Frequency = Frequency,
                RepeatInterval = (Frequency == 2 ? WeeklyInterval : (Frequency == 1 ? DailyInterval : 1)),
                MonthlyInterval = MonthlyInterval,
                StartDateTime = StartDate,
                EndDateTime = EndDate,
                DaysOfWeek = DaysOfWeek,
                DayOfMonth = DayOfMonth,
                Anniversary = Frequency == 16 ? new Anniversary() { Day = AnniversaryDay, Month = AnniversaryMonth } : null //yearly frequency - need to add anniversary for that
            };

            if (IsOneTimeEvent())
            {
                recurringEvent.OneTimeOnlyEventDate = StartDate;
            }

            return new Schedule(recurringEvent);
        }

        private bool IsOneTimeEvent()
        {
            if (Frequency == 0 && DaysOfWeek == 0 && MonthlyInterval == 0)
                return true;

            return false;
        }

        private void CalculateFrequency()
        {
            switch (_frequencyChoice)
            {
                case 1:
                    Frequency = 1; // daily
                    break;

                case 2:
                    Frequency = 2; // weekly
                    break;

                case 4:
                    Frequency = 4; // monthly
                    break;

                case 16:
                    Frequency = 16; // yearly
                    break;

                default:
                    Frequency = 0; // one-time only
                    break;
            }
        }

        public Anniversary Anniversary { get; set; }

        private void CalculateRecurrencePattern()
        {
            // determine frequency from recurrence pattern
            if (ScheduleRecurrence == RecurrencePattern.OneTime)
            {
                Frequency = 0;
                EndDate = null;
            }
            else // repeat pattern
            {
                if (FrequencyTypeOptions == FrequencyTypeEnum.Daily)
                {
                    DaysOfWeekOptions =
                        DayOfWeekEnum.Sun |
                        DayOfWeekEnum.Mon |
                        DayOfWeekEnum.Tue |
                        DayOfWeekEnum.Wed |
                        DayOfWeekEnum.Thu |
                        DayOfWeekEnum.Fri |
                        DayOfWeekEnum.Sat;
                }
            }
        }

        public enum RecurrencePattern
        {
            OneTime,
            Repeat
        };
    }

    public class CalenderObject
    {
        public long ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ScheduleViewModel
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public long PayorId { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public bool IsVirtualVisit { get; set; }

        public string PayorName { get; set; }
        public string CareType { get; set; }
        public string EmployeeFullName { get; set; }
        public string ScheduleRecurrence { get; set; }
        public string PatientFullName { get; set; }
        public DateTime ScheduleStartDate { get; set; }
        public DateTime? ScheduleEndDate { get; set; }
        public int FrequencyChoice { get; set; }

        public int Frequency { get; set; }
        public int DaysOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public bool IsMonthlyDaySelection { get; set; }
        public string Notes { get; set; }
        public bool IsAnyDay { get; set; }

        public bool AnyTimeClockIn { get; set; }
        public TimeSpan ClockInStartTime { get; set; }
        public TimeSpan ClockInEndTime { get; set; }

        public int DailyInterval { get; set; }
        public int WeeklyInterval { get; set; }
        public int MonthlyInterval { get; set; }
        public int? NumberOfOccurrences { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? EndDateTime { get; set; }

        public int AnniversaryDay { get; set; }
        public int AnniversaryMonth { get; set; }

        public long NurseScheduleID { get; set; }

        public bool IsSundaySelected
        {
            get; set;
        }

        public bool IsMondaySelected
        {
            get; set;
        }

        public bool IsTuesdaySelected
        {
            get; set;
        }

        public bool IsWednesdaySelected
        {
            get; set;
        }

        public bool IsThursdaySelected
        {
            get; set;
        }

        public bool IsFridaySelected
        {
            get; set;
        }

        public bool IsSaturdaySelected
        {
            get; set;
        }

        public bool IsFirstWeekOfMonthSelected
        {
            get; set;
        }

        public bool IsSecondWeekOfMonthSelected
        {
            get; set;
        }

        public long ScheduleID { get; set; }
        public long EmployeeId { get; set; }
        public long ReferralId { get; set; }
        public string CareTypeId { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public bool IsThirdWeekOfMonthSelected
        {
            get; set;
        }

        public bool IsFourthWeekOfMonthSelected
        {
            get; set;
        }

        public bool IsLastWeekOfMonthSelected
        {
            get; set;
        }

        /// <summary>
        /// The frequency expressed as enumeration.
        /// </summary>
        public FrequencyTypeEnum FrequencyTypeOptions
        {
            get; set;
        }

        /// <summary>
        /// The monthly interval expressed as enumeration
        /// </summary>
        public string MonthlyIntervalOptions
        {
            get; set;
        }

        /// <summary>
        /// The days of the week expressed as enumeration.
        /// </summary>
        public string DaysOfWeekOptions
        {
            get; set;
        }
    }
}
