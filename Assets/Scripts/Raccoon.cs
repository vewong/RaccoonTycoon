using UnityEngine;
using System.Collections;

public class Raccoon //: MonoBehaviour 
{

    MissionController.Type type;
    float minReproTime, maxReproTime;
    int minReproRate, maxReproRate;

	// constructor
    public Raccoon(MissionController.Type myBreed, float minTime, float maxTime, int minOffspring, int maxOffspring)
    {
        type = myBreed;
        minReproTime = minTime;
        maxReproTime = maxTime;
        minReproRate = minOffspring;
        maxReproRate = maxOffspring;
    }
	
	//getters
    public string Type()
    {
        return type.ToString();
    }

    public MissionController.Type GetEnumType()
    {
        return this.type;
    }

    public float GetReproTime()
    {
        return Random.Range(minReproTime, maxReproTime);
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
    public int Multiply()
    {
        int babies = Random.Range(minReproRate, maxReproRate);

        return babies;
    }
}
