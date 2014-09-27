using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bin : MonoBehaviour 
{
    //magic numbers!
    Raccoon currRaccoon;
    int maxRaccoons, currRaccoons;
    //List<int> pairOfRaccoons = new List<int>();
    float timeToNextBreed;

	// Use this for initialization
	void Start () 
    {
	    //PROTOTYPE STUFFFF
        //currRaccoon = Raccoon.CreateInstance("Raccoon") as Raccoon;
        //currRaccoon.Initialize(MissionController.Type.trash, 3f, 5f, 2, 4);
        maxRaccoons = 25;
        currRaccoons = 2;

        //timeToNextBreed = currRaccoon.GetReproTime();
	}

    public void Initialize(Raccoon raccoonType, int capacity, int raccoonCount)
    {
        currRaccoon = raccoonType;
        maxRaccoons = capacity;
        currRaccoons = raccoonCount;
    }

    public void Initialize(Raccoon raccoonType, int capacity)
    {
        currRaccoon = raccoonType;
        maxRaccoons = capacity;
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

    void SetCapacity(int capacity)
    {
        maxRaccoons = capacity;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (currRaccoon != null)
        {
            if (timeToNextBreed <= 0 && currRaccoons >= 2)
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
	}

    //other methods
    void HandleSellEvent(Raccoon parent, int price)
    {
        //subtract raccoons from bin
        if (parent.Equals(currRaccoon))
        {
            currRaccoons--;
        }
    }
}
