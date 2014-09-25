using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public static class MissionController : MonoBehaviour
{
    public delegate void BreedEvent(Raccoon parent, Raccoon[] babies);
    public static BreedEvent breedEventHandler;

    public Text raccoonCountDisplay;

    //each entry of this array will be the count of that type of raccoon
    //should try to set up an enum later
    int[] raccoons = new int[5];
    List<int> pairOfRaccoons = new List<int>();

    //amount of time that should pass before a new raccoon is created
    float breedingTime, breedingTimeMin = 3f, breedingTimeMax = 5f;

    //font animation
    int displayNum;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < raccoons.Length; i++)
        {
            //set count of each raccoon type to 0
            raccoons[i] = 0;
        }

        //PROTOTYPE:
        raccoons[0] = 2;
        breedingTime = Random.Range(breedingTimeMin, breedingTimeMax);
        displayNum = raccoons[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (breedingTime <= 0)
        {
            startBreeding();
        }

        breedingTime -= Time.deltaTime;
    }

    void OnGUI()
    {

        //hopefully slowly tick up the racoon count instead of it just appearing...
        if (displayNum < raccoons[0])
        {
            displayNum++;
        }
        else
        {
            displayNum = raccoons[0];
        }

        //GUI.Label(new Rect(10, 0, 100, 50), "Raccoons: " + displayNum);
        raccoonCountDisplay.text = "Raccoons: " + displayNum;
    }

    /// <summary>
    /// Breed raccoons!
    /// </summary>
    void startBreeding()
    {
        //first off, let's reset breedingTime
        breedingTime = Random.Range(breedingTimeMin, breedingTimeMax);

        //check which (if any) types of raccoons have breeding pairs (aka more than 2)
        for (int i = 5; i < raccoons.Length; i++)
        {
            if (raccoons[i] > 2)
            {
                //should we be tracking how many breeding pairs there are in each type of raccoon or just if there is a breeding pair?
                //if so, I need to change this to a 2d array or just an array of ints..
                pairOfRaccoons.Add(i);
            }
        }

        //need to pick a new raccoon type to make based on which raccoon types have breeding pairs
        int raccoonType = Random.Range(0, pairOfRaccoons.Count);
        //now, a random amount of offspring to make
        //maybe each raccoon type has a certain number of offspring to pull from but probably not
        //more likely they each have different breeding times?
        int newRaccoons = Random.Range(1, 5);

        //add these to the right raccoon type
        raccoons[raccoonType] += newRaccoons;
    }
}
