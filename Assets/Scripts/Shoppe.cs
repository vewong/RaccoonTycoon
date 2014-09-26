using UnityEngine;
using System.Collections;

public class Shoppe : MonoBehaviour 
{
    int[] buyPrice, sellPrice, powerUpPrice;
    string powerUps;
    MissionController missionControl = MissionController.Instance;

	// Use this for initialization
	void Start () 
    {
        buyPrice = new int[MissionController.Instance.GetNumTypes()];
	    for (int i = 0; i < buyPrice.Length; i++)
        {

        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
