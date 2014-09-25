using UnityEngine;
using System.Collections;

public class Raccoon //: MonoBehaviour 
{

    string type;
    float minReproTime, maxReproTime;
    int minReproRate, maxReproRate, maxPop;

	// constructor
    public Raccoon(string myBreed, float minTime, float maxTime, int minOffspring, int maxOffspring, int maxRaccoons)
    {
        type = myBreed;
        minReproTime = minTime;
        maxReproTime = maxTime;
        minReproRate = minOffspring;
        maxReproRate = maxOffspring;
        maxPop = maxRaccoons;
    }
	
	//getters
    protected string Type()
    {
        return type;
    }

    protected int MaxPopulation()
    {
        return maxPop;
    }

    //setters
    //pretty much going to have to use these when players get powerups
    protected void ChangeBreedingTime(float minTime, float maxTime)
    {
        minReproTime = minTime;
        maxReproTime = maxTime;
    }

    protected void ChangeOffsprings(int minOffspring, int maxOffspring)
    {
        minReproRate = minOffspring;
        maxReproRate = maxOffspring;
    }

    //other methods
    //do raccoons keep track of when they should breed?
    //the bins need to tell the raccoons if they can breed?

    //make more raccoons!
    protected int Multiply()
    {
        int babies = Random.Range(minReproRate, maxReproRate);

        if (MissionController.breedEventHandler != null)
        {
            // Call all the methods that have subscribed to the delegate
            MissionController.breedEventHandler(this, babies);
        }

        return babies;
    }
}
