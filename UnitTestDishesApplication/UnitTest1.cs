using Microsoft.VisualStudio.TestTools.UnitTesting;
using SF2024User33Lib;
using System;
using System.Linq;

namespace UnitTestDishesApplication
{
	[TestClass]
	public class UnitTest1
	{
		private readonly Calculations _calculations;

		public UnitTest1()
		{
			_calculations = new Calculations();
		}

		[TestMethod]
		public void AvailablePeriods_ConsultationTimeGreaterThanWorkingHours_ReturnsEmptyArray()
		{
			// Arrange
			var startTimes = new TimeSpan[0];
			var durations = new int[0];
			var beginWorkingTime = new TimeSpan(8, 0, 0);
			var endWorkingTime = new TimeSpan(18, 0, 0);
			var consultationTime = 540; // 9 hours

			// Act
			var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			// Assert
			Assert.AreEqual(result.First(), "08:00-17:00");
		}

		[TestMethod]
		public void AvailablePeriods_NoBusySlots_ReturnsAllSlots()
		{
			// Arrange
			TimeSpan[] startTimes = Array.Empty<TimeSpan>();
			int[] durations = Array.Empty<int>();
			TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
			TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
			int consultationTime = 30;

			// Act
			string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			// Assert
			Assert.AreEqual(18, result.Length);
			Assert.AreEqual("09:00-09:30", result[0]);
			Assert.AreEqual("17:30-18:00", result[17]);
		}

		[TestMethod]
		public void AvailablePeriods_ConsultationTimeLessThanMinute_ReturnsEmptyArray()
		{
			// Arrange
			var startTimes = new TimeSpan[0];
			var durations = new int[0];
			TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
			TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
			var consultationTime = 0; // less than a minute

			// Act
			var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			// Assert
			Assert.AreEqual(0, result.Length);
		}

		[TestMethod]
		public void AvailablePeriods_OneBusySlot_ReturnsRemainingSlots()
		{
			// Arrange
			TimeSpan[] startTimes = Array.Empty<TimeSpan>();
			int[] durations = Array.Empty<int>();
			TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
			TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
			int consultationTime = 30;

			// Act
			string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			// Assert
			Assert.AreEqual(18, result.Length);
			Assert.AreEqual("09:00-09:30", result[0]);
			Assert.AreEqual("17:00-17:30", result[16]);
		}

		[TestMethod]
		public void AvailablePeriods_BusySlotAtStart_ReturnsRemainingSlots()
		{
			// Arrange
			TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(9, 0, 0) };
			int[] durations = new int[] { 60 };
			TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
			TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
			int consultationTime = 30;

			// Act
			string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

			// Assert
			Assert.AreEqual(16, result.Length);
			Assert.AreEqual(result.First(), "10:00-10:30");
		}

	}
}
