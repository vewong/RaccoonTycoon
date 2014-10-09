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

    public delegate void SellEvent(Raccoon raccoonSold, int moneyEarned);
    public static SellEvent sellEventHandler;

    public delegate void BuyEvent(int buyPrice);
    public static BuyEvent buyEventHandler;

    public Text raccoonCountDisplay;
    public Text moneyDisplay;

    public Button settingsButton;
    public Button shopButton;

    public Canvas shopUI;
    public GameObject binArea;

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
    //int displayNum;
    int moneys;

    //bins
    GameObject starterBinObject, starterRaccoonObject;
    Bin starterBin;
    Raccoon starterRaccoon;

    // Use this for initialization
    void Start()
    {
        //set up the shop button
        if (shopButton != null)
        {
            shopButton.onClick.AddListener(delegate { OpenShop(); });
        }
        else
        {
            Debug.Log("Shop button NULL!");
        }

        //PROTOTYPE STUFF
        starterBinObject = new GameObject("Starter Bin");
        starterBin = starterBinObject.AddComponent<Bin>();

        starterRaccoonObject = new GameObject("Starter Raccoon");
        starterRaccoon = starterRaccoonObject.AddComponent<Raccoon>();
        starterRaccoon.Initialize(Type.trash, 3f, 5f, 2, 4);

        starterBin.Initialize(starterRaccoon, 25);

        //displayNum = starterBin.GetRaccoonsInBin();
        moneys = 3;
    }

    void OnEnable()
    {
        sellEventHandler += HandleSellEvent;
        buyEventHandler += HandleBuyEvent;
    }

    void OnDisable()
    {
        sellEventHandler -= HandleSellEvent;
        buyEventHandler -= HandleBuyEvent;
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

    public int CheckMoney()
    {
        return moneys;
    }

    public T NumToEnum<T>(int number)
    {
        return (T)Type.ToObject(typeof(T), number);
    }

    void HandleSellEvent(Raccoon parent, int price)
    {
        moneys += price;
    }

    void HandleBuyEvent(int buyPrice)
    {
        moneys -= buyPrice;
    }

    void OpenShop()
    {
        //if the shop button is clicked and the shop is inactive, activate the UI
        if (shopUI.enabled == false)
        {
            shopUI.enabled = true;
        }
        // if the shop button is clicked but the shop is active, deactivate it.
        else
        {
            shopUI.enabled = false;
        }
    }
}
