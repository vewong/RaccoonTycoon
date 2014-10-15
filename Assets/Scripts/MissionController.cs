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

    public delegate void HoverEvent(Bin hoveredBin, ShopRaccoon hoveredRaccoon);
    public static HoverEvent hoverEventHandler;

    public Text raccoonCountDisplay, moneyDisplay;

    public Button settingsButton, shopButton;

    public Canvas shopUI;
    public GameObject binArea;
    public Bin binPrefab;

    //probably need a power up array/key value thingie

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
        darkmatter = 24,
        count
    };

    //font animation
    //int displayNum;
    int moneys;

    //bins
    GameObject starterBinObject, starterRaccoonObject;
    Bin currBin;
    Raccoon starterRaccoon;
    ShopRaccoon currShopRaccoon;

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

        starterRaccoonObject = new GameObject("Starter Raccoon");
        starterRaccoon = starterRaccoonObject.AddComponent<Raccoon>();
        starterRaccoon.Initialize(Type.trash, 3f, 5f, 2, 4);

        AddBin(starterRaccoon);
        
        moneys = 3;
    }

    void OnEnable()
    {
        sellEventHandler += HandleSellEvent;
        buyEventHandler += HandleBuyEvent;
        hoverEventHandler += HandleHoverEvent;
    }

    void OnDisable()
    {
        sellEventHandler -= HandleSellEvent;
        buyEventHandler -= HandleBuyEvent;
        hoverEventHandler -= HandleHoverEvent;
    }

    void OnGUI()
    {
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

    //getters
    public int GetNumTypes()
    {
        return (int)Type.count;
    }

    public Bin GetCurrentBin()
    {
        return currBin;
    }

    public int CheckMoney()
    {
        return moneys;
    }

    public ShopRaccoon GetCurrentShopRaccoon()
    {
        return currShopRaccoon;
    }

    //other methods
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

    void AddBin(Raccoon sampleRaccoon)
    {
       //create a new bin from the prefab bin
       Bin newBin = Instantiate(binPrefab) as Bin;

        //set the new bin to be a child of the bin layout element
       newBin.gameObject.transform.parent = binArea.transform;
       newBin.Initialize(starterRaccoon, 25);

       //set the current bin to the bin just created
       if (newBin == null)
       {
           Debug.Log("Bin null!");
       }
       else
       {
           currBin = newBin;
       }
    }

    void HandleHoverEvent(Bin hoveredBin, ShopRaccoon hoveredRaccoon)
    {
        //might need to add in other types of things that can be hovered over later
        if (hoveredBin != null)
        {
            currBin = hoveredBin;
        }
        else if (hoveredRaccoon != null)
        {
            currShopRaccoon = hoveredRaccoon;
        }
    }
}
