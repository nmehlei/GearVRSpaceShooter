using UnityEngine;
using NUnit.Framework;
using Assets.Scripts.MovementControl;

public class MovementCalculationHelperTest
{
	[Test]
	public void UpdateSpeedFactor_Returns_0_If_Input_0()
	{
	    var expectedResult = 0f;
	    var result = MovementCalculationHelper.UpdateSpeedFactor(0, 0, 0);
        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void UpdateSpeedFactor_Returns_Correct_Value_With_Simple_Input()
    {
        var expectedResult = 2f;
        var result = MovementCalculationHelper.UpdateSpeedFactor(1f, 2f, 1f);
        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void UpdateSpeedFactor_Returns_Correct_Value_With_0_Existing_Speed()
    {
        var expectedResult = 2f;
        var result = MovementCalculationHelper.UpdateSpeedFactor(0, 8f, 2f);
        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void UpdateSpeedFactor_Returns_Correct_Value_With_Remaining_Speed()
    {
        var expectedResult = 0f;
        var result = MovementCalculationHelper.UpdateSpeedFactor(2f, 0f, 2f);
        Assert.AreEqual(expectedResult, result);
    }
}