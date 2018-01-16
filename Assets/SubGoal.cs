using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class SubGoal : MonoBehaviour
{
    // Unity Fields

    public int subGoalNumber;

    [Header("Materials")]
    public Material InactiveMaterial;
    public Material ActiveMaterial;

    // Fields

    private bool isActive;

    // Methods

    void FixedUpdate ()
	{
	    var subGoalManager = SubGoalManager.GetInstance();
	    var isActiveNow = subGoalManager.NextSubGoalNumber == subGoalNumber;

	    if (isActive != isActiveNow)
	    {
	        Debug.Log("SubGoal-isActive changed!");
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (isActiveNow)
            {
                Debug.Log("New SubGoal is now active!");
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