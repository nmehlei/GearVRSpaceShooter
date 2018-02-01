using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using Assets;

public class SubGoalManagerTest {

	[Test]
	public void Initializes_With_Correct_Values()
	{
	    var subGoalManager = SubGoalManager.GetInstance();
        Assert.AreEqual(1, subGoalManager.NextSubGoalNumber);
	}

    [Test]
    public void Returns_Correct_Value_After_Increment()
    {
        var subGoalManager = SubGoalManager.GetInstance();
        subGoalManager.IncrementSubGoalNumber();
        Assert.AreEqual(2, subGoalManager.NextSubGoalNumber);
    }

    [Test]
    public void Returns_Correct_Value_After_Multiple_Increments()
    {

        var subGoalManager = SubGoalManager.GetInstance();
        subGoalManager.IncrementSubGoalNumber();
        subGoalManager.IncrementSubGoalNumber();
        subGoalManager.IncrementSubGoalNumber();
        Assert.AreEqual(5, subGoalManager.NextSubGoalNumber);
    }

    [Test]
    public void Returns_Correct_Value_After_Reset()
    {

        var subGoalManager = SubGoalManager.GetInstance();
        subGoalManager.IncrementSubGoalNumber();
        subGoalManager.IncrementSubGoalNumber();
        subGoalManager.IncrementSubGoalNumber();
        subGoalManager.Reset();
        Assert.AreEqual(1, subGoalManager.NextSubGoalNumber);
    }
}