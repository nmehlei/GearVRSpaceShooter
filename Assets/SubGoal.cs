using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class SubGoal : MonoBehaviour
{
    // Unity Fields

    [Tooltip("The incrementing number that indicates the order the SubGoals shall be reached.")]
    public int subGoalNumber;

    [Header("Materials")]
    [Tooltip("Material that is being used while SubGoal is in inactive state.")]
    public Material InactiveMaterial;
    [Tooltip("Material that is being used while SubGoal is in active state.")]
    public Material ActiveMaterial;

    // Fields

    private bool isActive;

    // Methods

    void FixedUpdate ()
	{
	    var subGoalManager = SubGoalManager.GetInstance();
	    var isActiveNow = subGoalManager.NextSubGoalNumber == subGoalNumber;

        // if active state changed ..
	    if (isActive != isActiveNow)
	    {
            // .. switch out material
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (isActiveNow)
            {
                meshRenderer.material = ActiveMaterial;
            }
	        else
	        {
	            meshRenderer.material = InactiveMaterial;
            }

	        isActive = isActiveNow;
	    }
	}
}