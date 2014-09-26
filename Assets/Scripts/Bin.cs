using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bin : MonoBehaviour 
{
    //magic numbers!
    Raccoon currRaccoon;
    int maxRaccoons, currRaccoons;
    List<int> pairOfRaccoons = new List<int>();
    float timeToNextBreed;

	// Use this for initialization
	void Start () 
    {
	    //PROTOTYPE STUFFFF
        currRaccoon = new Raccoon(MissionController.Type.trash, 3f, 5f, 2, 4);
        maxRaccoons = 25;
        currRaccoons = 2;

        timeToNextBreed = currRaccoon.GetReproTime();
	}

    void OnEnable()
    {
        MissionController.sellEventHandler += HandleSellEvent;
    }

    void OnDisable()
    {
        MissionController.sellEventHandler -= HandleSellEvent;
        
    }

    //getter
    public int GetRaccoonsInBin()
    {
        return currRaccoons;
    }

    public Raccoon GetRaccoon()
    {
        return currRaccoon;
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if (timeToNextBreed <= 0 && currRaccoons > 2)
        {
            currRaccoons += currRaccoon.Multiply();

            //need to add something in here about selling the excess raccoons if powerup is available
            if (currRaccoons > maxRaccoons)
            {
                currRaccoons = maxRaccoons;
            }

            timeToNextBreed = currRaccoon.GetReproTime();
        }
        else
        {
            timeToNextBreed -= Time.deltaTime;
        }
	}

    //other methods
    void HandleSellEvent(Raccoon parent, int babies)
    {
        //add raccoons to raccoon bin

    }
}
