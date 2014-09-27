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
    public Text moneyDisplay;

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
    GameObject starterBinObject;
    Bin starterBin;

    // Use this for initialization
    void Start()
    {
        starterBinObject = new GameObject("Starter Bin");
        starterBin = starterBinObject.AddComponent<Bin>();

        starterBin.Initialize(new Raccoon(Type.trash, 3f, 5f, 2, 4), 25);

        displayNum = starterBin.GetRaccoonsInBin();
        moneys = 200;
    }

    void OnEnable()
    {
        sellEventHandler += HandleSellEvent;
    }

    void OnDisable()
    {
        sellEventHandler -= HandleSellEvent;

    }

    void OnGUI()
    {
        raccoonCountDisplay.text = "Raccoons: " + starterBin.GetRaccoonsInBin();
        moneyDisplay.text = "$" + moneys;
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

    void HandleSellEvent(Raccoon parent, int price)
    {
        moneys += price;
    }
}
