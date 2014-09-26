using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionController : MonoBehaviour
{
    private static MissionController instance;

    public static MissionController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject instanceObject = new GameObject("MissionController");
                instance = instanceObject.AddComponent<MissionController>();
            }

            return instance;
        }
    }

    public delegate void SellEvent(Raccoon raccoonSold, int numSold);
    public static SellEvent sellEventHandler;

    public Text raccoonCountDisplay;

    //each entry of this array will be the count of that type of raccoon
    //should try to set up an enum later
    public enum Type
    {
        trash = 0,
        sewer = 1,
        street = 2,
        bush = 3,
        tree = 4,
        house = 5,
        middleclass = 6,
        uppermiddleclass = 7,
        bourgeoisie = 8,
        artisan = 9,
        courtjester = 10,
        squire = 11,
        knight = 12,
        rook = 13,
        king = 14,
        queen = 15,
        demigod = 16,
        god = 17,
        goddess = 18,
        space = 19,
        solar = 20,
        universe = 21,
        blackhole = 22,
        whitehole = 23,
        darkmatter = 24
    };
    //MAGIC NUMBER, CHANGE THIS IF YOU CHANGE THE ENUM
    int numTypes = 25;

    //font animation
    int displayNum;
    int moneys;

    //bins
    Bin starterBin = new Bin();

    // Use this for initialization
    void Start()
    {

        displayNum = starterBin.GetRaccoonsInBin();
        moneys = 200;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //GUI.Label(new Rect(10, 0, 100, 50), "Raccoons: " + displayNum);
        raccoonCountDisplay.text = "Raccoons: " + displayNum;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There can only be one!");
            Destroy(this);
        }
    }

    public int GetNumTypes()
    {
        return numTypes;
    }

    public Bin GetCurrentBin()
    {
        //eventually need some code in here about what bin the player is viewing
        //but for now...
        return starterBin;
    }

    /// <summary>
    /// Breed raccoons!
    /// </summary>
    //void startBreeding()
    //{
        //first off, let's reset breedingTime
        //breedingTime = Random.Range(breedingTimeMin, breedingTimeMax);

        //check which (if any) types of raccoons have breeding pairs (aka more than 2)
        //for (int i = 5; i < raccoons.Length; i++)
        //{
         //   if (raccoons[i] > 2)
         //   {
                //should we be tracking how many breeding pairs there are in each type of raccoon or just if there is a breeding pair?
                //if so, I need to change this to a 2d array or just an array of ints..
              //  pairOfRaccoons.Add(i);
          //  }
        //}

        //need to pick a new raccoon type to make based on which raccoon types have breeding pairs
        //int raccoonType = Random.Range(0, pairOfRaccoons.Count);
        //now, a random amount of offspring to make
        //maybe each raccoon type has a certain number of offspring to pull from but probably not
        //more likely they each have different breeding times?
       // int newRaccoons = Random.Range(1, 5);

        //add these to the right raccoon type
        //raccoons[raccoonType] += newRaccoons;
    //}
}
