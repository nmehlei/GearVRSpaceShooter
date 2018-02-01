using UnityEngine;
using NUnit.Framework;
using Assets.Scripts.MovementControl;

public class StrictDeadzoneHandlerTest
{
    [Test]
    public void AdjustValue_Returns_Correct_Value_For_0()
    {
        var handler = new StrictDeadzoneHandler(0.01f, 0.25f);
        var expectedValue = 0f;
        var value = handler.AdjustValue(0f, new Vector3(0, 0, 0));
        Assert.AreEqual(expectedValue, value);
    }

    [Test]
    public void AdjustValue_Returns_Correct_Value_For_Over_Range()
    {
        var handler = new StrictDeadzoneHandler(0.01f, 0.25f);
        var expectedValue = 1f;
        var value = handler.AdjustValue(2f, new Vector3(2f, 0, 0));
        Assert.AreEqual(expectedValue, value);
    }

    [Test]
    public void AdjustValue_Returns_Correct_Value_For_Under_Range()
    {
        var handler = new StrictDeadzoneHandler(0.01f, 0.25f);
        var expectedValue = -1f;
        var value = handler.AdjustValue(-2f, new Vector3(-2f, 0, 0));
        Assert.AreEqual(expectedValue, value);
    }

    [Test]
    public void AdjustValue_Returns_Correct_Value_For_Inside_Range()
    {
        var handler = new StrictDeadzoneHandler(0.01f, 0.25f);
        var expectedValue = 0.458333313f;
        var value = handler.AdjustValue(0.11f, new Vector3(0.11f, 0, 0));
        Assert.AreEqual(expectedValue, value);
    }

    [Test]
    public void AdjustValue_Returns_Correct_Value_For_Inside_Range_And_Widened_Deadzone()
    {
        var handler = new StrictDeadzoneHandler(0.01f, 0.5f);
        var expectedValue = 0.408163279f;
        var value = handler.AdjustValue(0.2f, new Vector3(0.2f, 0, 0));
        Assert.AreEqual(expectedValue, value);
    }
}
