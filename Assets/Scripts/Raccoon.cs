using UnityEngine;
using System.Collections;

public class Raccoon : MonoBehaviour
{

    MissionController.Type type;
    float minReproTime, maxReproTime;
    int minReproRate, maxReproRate;

	// constructor
    public void Initialize(MissionController.Type myBreed, float minTime, float maxTime, int minOffspring, int maxOffspring)
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
    public void BreedTimeUpgrade()
    {
        //reduce the reproduction time by 5%
        minReproTime *= .95f;
        maxReproTime *= .95f;
    }

    public void OffspringIncreaseUpgrade()
    {
        //increase the amount of offspring produced by 5%
        float minReproRateFloat, maxReproRateFloat;

        minReproRateFloat = minReproRate;
        maxReproRateFloat = maxReproRate;

        minReproRateFloat *= 1.1f;
        maxReproRateFloat *= 1.1f;
        Debug.Log("Testing fake math (floats): " + minReproRateFloat + " min " + maxReproRateFloat + " max.");

        minReproRate = (int)minReproRateFloat;
        maxReproRate = (int)maxReproRateFloat;
        Debug.Log("Testing fake math (ints): " + minReproRate + " min " + maxReproRate + " max.");
    }

    //other methods

    //make more raccoons!
    public int Multiply()
    {
        int babies = Random.Range(minReproRate, maxReproRate);

        return babies;
    }
}
