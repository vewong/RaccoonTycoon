using UnityEngine;
using System.Collections;

public class Raccoon : MonoBehaviour {

    string type;
    float minReproTime, maxReproTime;
    int minReproRate, maxReproRate;

	// constructor
	void Raccoon(string myBreed) 
    {
        type = myBreed;
        minReproTime = 5f;
        maxReproTime = 7f;
        minReproRate = 3;
        maxReproTime = 5;
	}

    void Raccoon(string myBreed, float minTime, float maxTime, int minOffspring, int maxOffspring)
    {
        type = myBreed;
        minReproTime = minTime;
        maxReproTime = maxTime;
        minReproRate = minOffspring;
        maxReproRate = maxOffspring;
    }
	
	//getters
    protected string Type()
    {
        return type;
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

    //make more raccoons!
    protected int Multiply()
    {
        return Random.Range(minReproRate, maxReproRate);
    }
}
