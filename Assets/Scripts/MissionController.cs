using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

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

    public delegate void SellEvent(Raccoon raccoonSold, float moneyEarned);
    public static SellEvent sellEventHandler;

    public delegate void BuyEvent(float buyPrice);
    public static BuyEvent buyEventHandler;

    public delegate void HoverEvent(Bin hoveredBin, ShopRaccoon hoveredRaccoon, Upgrade hoveredUpgrade);
    public static HoverEvent hoverEventHandler;

    public delegate void UpgradeEvent(float upgradePrice, Upgrade upgradeBought);
    public static UpgradeEvent buyUpgradeEventHandler;

    public Text raccoonCountDisplay, moneyDisplay;

    public Button settingsButton, shopButton;

    public Canvas shopUI;
    public GameObject binArea;
    public Bin binPrefab;

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

    //DEBUG TAKE OUT FOR RELEASE
    public float moneys;

    GameObject starterBinObject, starterRaccoonObject;
    Bin currBin;
    List<Bin> bins = new List<Bin>();
    Raccoon starterRaccoon;
    ShopRaccoon currShopRaccoon;
    Upgrade currUpgrade;
    int[] minOffsprings, maxOffsprings;
    float[] minReproTimes, maxReproTimes;

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

        //set the repro times for all raccoon types
        minReproTimes = new float[(int)Type.count];
        float minReproTimeStart = 3f;
        maxReproTimes = new float[(int)Type.count];
        float maxReproTimeStart = 5f;
        minOffsprings = new int[(int)Type.count];
        int minOffspringStart = 2;
        maxOffsprings = new int[(int)Type.count];
        int maxOffspringStart = 4;

        for (int i = 0; i < (int)Type.count; i++)
        {
            //DEBUG STuFFF
            Debug.Log("Raccoon " + (Type)i + ":\nRepro min/max: " + minReproTimeStart + ", " + maxReproTimeStart + "; Offspring min/max: " + minOffspringStart + ", " + maxOffspringStart);

            minReproTimes[i] = minReproTimeStart;
            maxReproTimes[i] = maxReproTimeStart;
            minOffsprings[i] = minOffspringStart;
            maxOffsprings[i] = maxOffspringStart;

            minReproTimeStart *= 1.05f;
            maxReproTimeStart *= 1.05f;
            //need to figure out another way to calculate offspring
            minOffspringStart += (int)Math.Floor(i * 1.001f);
            maxOffspringStart += (int)Math.Floor(i * 1.001f);
        }

        starterRaccoonObject = new GameObject("Starter Raccoon");
        starterRaccoon = starterRaccoonObject.AddComponent<Raccoon>();
        starterRaccoon.Initialize(Type.trash);

        AddBin(starterRaccoon, 25, 2);
        
        moneys = 3;
    }

    void OnEnable()
    {
        sellEventHandler += HandleSellEvent;
        buyEventHandler += HandleBuyEvent;
        hoverEventHandler += HandleHoverEvent;
        buyUpgradeEventHandler += HandleUpgradeEvent;
    }

    void OnDisable()
    {
        sellEventHandler -= HandleSellEvent;
        buyEventHandler -= HandleBuyEvent;
        hoverEventHandler -= HandleHoverEvent;
        buyUpgradeEventHandler -= HandleUpgradeEvent;
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

    public float CheckMoney()
    {
        return moneys;
    }

    public ShopRaccoon GetCurrentShopRaccoon()
    {
        return currShopRaccoon;
    }

    public Upgrade GetCurrentUpgrade()
    {
        return currUpgrade;
    }

    public List<Bin> GetBins()
    {
        return bins;
    }

    public float[] GetRaccoonTimes(MissionController.Type raccoonType)
    {
        float[] times = new float[4];
        int type = (int)raccoonType;

        times[0] = minReproTimes[type];
        times[1] = maxReproTimes[type];
        times[2] = minOffsprings[type]; //hopefully this conversion won't fuck shit up
        times[3] = maxOffsprings[type];

        return times;
    }

    //other methods
    public T NumToEnum<T>(int number)
    {
        return (T)Type.ToObject(typeof(T), number);
    }

    void HandleSellEvent(Raccoon parent, float price)
    {
        moneys += price;

        //get rid of the raccoon from the current bin
        currBin.SellRaccoon();
    }

    void HandleBuyEvent(float buyPrice)
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

    public void AddBin(Raccoon sampleRaccoon)
    {
       //create a new bin from the prefab bin
       Bin newBin = Instantiate(binPrefab) as Bin;

        //set the new bin to be a child of the bin layout element
       newBin.gameObject.transform.SetParent(binArea.transform, false);
       newBin.Initialize();

       //set the current bin to the bin just created
       if (newBin == null)
       {
           Debug.Log("Bin null!");
       }
       else
       {
           bins.Add(newBin);

           currBin = newBin;
       }
    }

    public void AddBin(Raccoon sampleRaccoon, int capacity, int currRaccoons)
    {
        //create a new bin from the prefab bin
        Bin newBin = Instantiate(binPrefab) as Bin;

        //set the new bin to be a child of the bin layout element
        newBin.gameObject.transform.SetParent(binArea.transform, false);
        newBin.Initialize(starterRaccoon, capacity, currRaccoons);

        //set the current bin to the bin just created
        if (newBin == null)
        {
            Debug.LogError("Bin null!");
        }
        else
        {
            bins.Add(newBin);

            currBin = newBin;
        }
    }

    void HandleHoverEvent(Bin hoveredBin, ShopRaccoon hoveredRaccoon, Upgrade hoveredUpgrade)
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
        else if(hoveredUpgrade != null)
        {
            currUpgrade = hoveredUpgrade;
        }
    }

    void HandleUpgradeEvent(float buyPrice, Upgrade upgradeBought)
    {
        //first handle the money
        moneys -= buyPrice;

        //then check if there's any other business to attend to
    }
}
