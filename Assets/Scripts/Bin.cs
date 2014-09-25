using UnityEngine;
using System.Collections;

public class Bin : MonoBehaviour 
{
    //magic numbers!
    Raccoon currRaccoon;
    int maxRaccoons, currRaccoons;

	// Use this for initialization
	void Start () 
    {
	
	}

    void OnEnable()
    {
        MissionController.breedEventHandler += HandleBreedEvent;
    }

    void OnDisable()
    {
        MissionController.breedEventHandler -= HandleBreedEvent;

    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //other methods
    void HandleBreedEvent(Raccoon parent, int babies)
    {
        //add raccoons to raccoon bin
    }
}
