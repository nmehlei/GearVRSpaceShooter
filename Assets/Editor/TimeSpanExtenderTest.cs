using System;
using NUnit.Framework;
using Assets.Scripts;

public class TimeSpanExtenderTest
{
    [Test]
	public void ToCounterTimeString_Returns_Correct_String_For_0_Seconds()
	{
	    var timeSpan = TimeSpan.FromSeconds(0);
	    var result = timeSpan.ToCounterTimeString();
	    var expectedResult = "00:00";

        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void ToCounterTimeString_Returns_Correct_String_For_43_Seconds()
    {
        var timeSpan = TimeSpan.FromSeconds(43);
        var result = timeSpan.ToCounterTimeString();
        var expectedResult = "00:43";

        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void ToCounterTimeString_Returns_Correct_String_For_2_Minutes_11_Seconds()
    {
        var timeSpan = new TimeSpan(0, 2, 11);
        var result = timeSpan.ToCounterTimeString();
        var expectedResult = "02:11";

        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void ToCounterTimeString_Returns_Correct_String_For_99_Minutes_59_Seconds()
    {
        var timeSpan = new TimeSpan(0, 99, 59);
        var result = timeSpan.ToCounterTimeString();
        var expectedResult = "99:59";

        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void ToCounterTimeString_Returns_Correct_String_For_Over_Maximal_Time()
    {
        var timeSpan = new TimeSpan(3, 0, 0);
        var result = timeSpan.ToCounterTimeString();
        var expectedResult = "--:--";

        Assert.AreEqual(expectedResult, result);
    }
}