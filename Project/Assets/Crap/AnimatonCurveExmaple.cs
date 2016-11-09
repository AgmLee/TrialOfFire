using UnityEngine;
using System.Collections;

public class AnimatonCurveExmaple : MonoBehaviour 
{
    public AnimationCurve MyCurve = AnimationCurve.EaseInOut(0,0,1,1);
    float EndTime = 1;

	// Use this for initialization
	void Start () 
    {
        EndTime = MyCurve.keys[MyCurve.keys.Length - 1].time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        MyCurve.Evaluate(0.25f);
	}
}
