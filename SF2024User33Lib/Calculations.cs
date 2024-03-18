using System;
using System.Collections.Generic;
using System.Linq;

namespace SF2024User33Lib
{
	public class Calculations
	{
		public string[] AvailablePeriods(
			TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime,
			TimeSpan endWorkingTime, int consultationTime
		)
		{
			List<TimeSlot> allTimeSlots = GenerateAllTimeSlots(beginWorkingTime, endWorkingTime, consultationTime);
			RemoveBusyTimeSlots(allTimeSlots, startTimes, durations);
			List<TimeSlot> freeTimeSlots = FilterFreeTimeSlots(allTimeSlots, consultationTime);
			return ConvertTimeSlotsToStrings(freeTimeSlots);
		}

		private List<TimeSlot> GenerateAllTimeSlots(TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
		{
			List<TimeSlot> allTimeSlots = new List<TimeSlot>();
			if (consultationTime <= 0) return allTimeSlots;

			TimeSpan currentTime = beginWorkingTime;

			while (currentTime < endWorkingTime)
			{
				allTimeSlots.Add(new TimeSlot { StartTime = currentTime, EndTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime)) });
				currentTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime));
			}
			return allTimeSlots;
		}

		private void RemoveBusyTimeSlots(List<TimeSlot> allTimeSlots, TimeSpan[] startTimes, int[] durations)
		{
			for (int i = 0; i < startTimes.Length; i++)
			{
				TimeSpan startTime = startTimes[i];
				TimeSpan endTime = startTime.Add(TimeSpan.FromMinutes(durations[i]));

				TimeSlot busyTimeSlot = new TimeSlot { StartTime = startTime, EndTime = endTime };

				allTimeSlots.RemoveAll(slot => (slot.StartTime >= busyTimeSlot.StartTime && slot.StartTime < busyTimeSlot.EndTime) ||
												(slot.EndTime > busyTimeSlot.StartTime && slot.EndTime <= busyTimeSlot.EndTime));
			}
		}

		private List<TimeSlot> FilterFreeTimeSlots(List<TimeSlot> allTimeSlots, int consultationTime)
		{
			TimeSpan minConsultationTime = TimeSpan.FromMinutes(consultationTime);
			return allTimeSlots.Where(slot => (slot.EndTime - slot.StartTime) >= minConsultationTime).ToList();
		}

		private string[] ConvertTimeSlotsToStrings(List<TimeSlot> timeSlots)
		{
			List<string> timeSlotStrings = new List<string>();
			foreach (var slot in timeSlots)
			{
				timeSlotStrings.Add($"{slot.StartTime:hh\\:mm}-{slot.EndTime:hh\\:mm}");
			}
			return timeSlotStrings.ToArray();
		}
	}
}
