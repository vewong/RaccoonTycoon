using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Shoppe : MonoBehaviour
{
    private static Shoppe instance;
    public static Shoppe Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No instance of Shoppe exists in the scene!");
            }

            return instance;
        }
    }

    float[] buyPrice, sellPrice, powerUpPrice;
    string[] powerUps;

    public enum Upgrades
    {
        bin,
        breedTimeDown,
        reproNumUp,
        binCapacityUp,
        raccoonPriceUp,
        autoSellMachine,
        count
    };

    public Button exitButton, mainTab, upgradesTab;
    public AudioClip sellSound, buySound;
    public GameObject raccoonDisplay, raccoonScrollableContentContainer, upgradesDisplay, upgradesContentContainer;
    public GameObject raccoonTabObject, upgradesTabObject;
    public Sprite mysteryRaccoonSprite;

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

    // Use this for initialization
    void Start()
    {
        //set up the exit button
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(delegate { CloseShop(); });
        }
        else
        {
            Debug.Log("Exit button NULL!");
        }

        //set up the main shop tab button
        if (mainTab != null)
        {
            mainTab.onClick.AddListener(delegate { OpenRaccoonTab(); });
        }
        else
        {
            Debug.Log("Raccoon tab button NULL!");
        }

        //set up the upgrades tab button
        if (upgradesTab != null)
        {
            upgradesTab.onClick.AddListener(delegate { OpenUpgradesTab(); });
        }
        else
        {
            Debug.Log("Upgrades tab NULL!");
        }

        //populate the upgrades
        powerUps = new string[(int)Upgrades.count];
        powerUpPrice = new float[powerUps.Length];
        int startPowerUpsPrice = 100;

        List<Text> displayStrings = new List<Text>();

        for (int i = 0; i < powerUps.Length; i++)
        {
            //get the name of the upgrade based on the enum
            string powerUpName;

            switch ((Upgrades)i)
            {
                case Upgrades.bin:
                    powerUpName = "New Bin";
                    break;
                case Upgrades.breedTimeDown:
                    powerUpName = "Fertility Treatment";
                    break;
                case Upgrades.reproNumUp:
                    powerUpName = "Incubators";
                    break;
                case Upgrades.raccoonPriceUp:
                    powerUpName = "Glitter";
                    break;
                case Upgrades.binCapacityUp:
                    powerUpName = "Bin Capacity";
                    break;
                case Upgrades.autoSellMachine:
                    powerUpName = "Auto-sell Raccoons";
                    break;
                default:
                    powerUpName = "???";
                    break;
            };

            powerUps[i] = powerUpName;
            //set the power up's price
            powerUpPrice[i] = startPowerUpsPrice;

            startPowerUpsPrice *= 2;

            //add upgrade display to upgrades tab
            GameObject tempObject;
            tempObject = Instantiate(upgradesDisplay) as GameObject;
            tempObject.transform.SetParent(upgradesContentContainer.transform, false);

            tempObject.GetComponentsInChildren<Text>(displayStrings);

            //assuming the first text is the name and the second is the price display
            displayStrings[1].text = powerUps[i];
            displayStrings[2].text = "$" + powerUpPrice[i];

            //can set upgrade type here?
        }

        //deactivate the upgrades tab once finished
        upgradesTabObject.SetActive(false);

        int startBuyPrice = 5;
        int startSellPrice = 3;

        buyPrice = new float[MissionController.Instance.GetNumTypes()-1];
        sellPrice = new float[MissionController.Instance.GetNumTypes()-1];

        //populate buy and sell price arrays, initialize a whole bunch of shop entries
        for (int i = 0; i < buyPrice.Length; i++)
        {
            buyPrice[i] = startBuyPrice;
            sellPrice[i] = startSellPrice;

            startBuyPrice *= 2;
            startSellPrice *= 2;

            GameObject tempObject;
            tempObject = Instantiate(raccoonDisplay) as GameObject;
            tempObject.transform.SetParent(raccoonScrollableContentContainer.transform, false);

            tempObject.GetComponentsInChildren<Text>(displayStrings);

            //assuming the first text is the name and the second is the price display
            displayStrings[0].text = (MissionController.Instance.NumToEnum<MissionController.Type>(i)).ToString();
            displayStrings[1].text = "$" + buyPrice[i];
        }

        //now the original prefabs aren't needed, so deactivate them
        raccoonDisplay.SetActive(false);
        upgradesDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //enable or disable buttons based on available raccoons/money
        Bin currentBin = MissionController.Instance.GetCurrentBin();
        int currentBinRaccoonCount = currentBin.GetRaccoonsInBin();
        Raccoon currentBinRaccoon = currentBin.GetRaccoon();

        ShopRaccoon currentShopRaccoon = MissionController.Instance.GetCurrentShopRaccoon();

        //Debug.Log("Bin " + (currentBin != null ? currentBin.GetRaccoon().Type() : "null") + " Raccoon " + (currentShopRaccoon != null ? currentShopRaccoon.GetRaccoonType().ToString() : "null"));

        if (currentBin != null)
        {
            if (currentBinRaccoonCount <= 0)
            {
                currentBin.sellButton.interactable = false;
            }
            else
            {
                currentBin.sellButton.interactable = true;
            }
        }

        if (currentShopRaccoon != null)
        {
            float currentFunds = MissionController.Instance.CheckMoney();

            if (currentFunds > buyPrice[(int)currentShopRaccoon.GetRaccoonType()] && currentBinRaccoonCount < currentBin.GetCapacity())
            {
                currentShopRaccoon.buyButton.interactable = true;
            }
            else
            {
                currentShopRaccoon.buyButton.interactable = false;
            }
        }
    }

    public void SellRaccoon()
    {
        Raccoon replacementRaccoon = MissionController.Instance.GetCurrentBin().GetRaccoon();

        if (MissionController.sellEventHandler != null)
        {
            // Call all the methods that have subscribed to the delegate
            MissionController.sellEventHandler(replacementRaccoon, sellPrice[(int)replacementRaccoon.GetEnumType()]);
        }

        GetComponent<AudioSource>().PlayOneShot(sellSound);
    }

    public void BuyRaccoon()
    {
        //get the raccoon the player is trying to buy from the shop
        ShopRaccoon currentRaccoon = MissionController.Instance.GetCurrentShopRaccoon();

        if (MissionController.buyEventHandler != null && currentRaccoon != null)
        {
            //first check if this is a new raccoon type
            foreach (Bin b in MissionController.Instance.GetBins())
            {
                if (b.GetRaccoon() == null)
                {
                    //we found an empty bin!
                    GameObject newRaccoonObject = new GameObject("New Raccoon");
                    Raccoon newRaccoon = newRaccoonObject.AddComponent<Raccoon>();
                    newRaccoon.Initialize(currentRaccoon.GetRaccoonType());
                    b.SetRaccoon(newRaccoon);

                    MissionController.buyEventHandler(buyPrice[(int)currentRaccoon.GetRaccoonType()]);
                    GetComponent<AudioSource>().PlayOneShot(buySound);

                    return;
                }
                else if (b.GetRaccoon().GetEnumType() == currentRaccoon.GetRaccoonType())
                {
                    //put some raccoons in this bin!

                    //call all the methods that have subscribed to the delegate
                    MissionController.buyEventHandler(buyPrice[(int)currentRaccoon.GetRaccoonType()]);
                    GetComponent<AudioSource>().PlayOneShot(buySound);

                    //stop looking, we already found somewhere to put this raccoon
                    return;
                }
            }

            //if we're down here, there was no bins with room or no bins with that same type
            //try to narrow it down
            foreach (Bin b in MissionController.Instance.GetBins())
            {
                if (!b.IsFull())
                {
                    //in this case, all the bins were not full so the cause of not being able to sell was having a new raccoon type
                    Debug.LogError("You have no empty bins to add this new Raccoon to!");
                    return;
                }
            }
            //okay we shouldn't be able to get down here without hitting another type of error/exit
            Debug.LogError("All of your bins are full!");
        }
        else
        {
            Debug.LogError("current shop raccoon is null!... or maybe no one is subscribed to buyEventHandler");
        }
    }

    public void BuyUpgrade()
    {
        Debug.Log("Upgrade clicked!");
        Upgrade currentUpgrade = MissionController.Instance.GetCurrentUpgrade();

        if (MissionController.buyUpgradeEventHandler != null && currentUpgrade != null)
        {
            //call all the methods subscribed to the delegate
            MissionController.buyUpgradeEventHandler(powerUpPrice[(int)currentUpgrade.GetUpgradeType()], currentUpgrade);
            //play audio?

            currentUpgrade.AddUpgradeCount();

            //check upgrade type and apply changes to whatever needs changing.
            switch (currentUpgrade.GetUpgradeType())
            {
                case Upgrades.bin:
                    //I guess duplicate the types of raccoons in the current bin... or make it possible for there to be an empty bin
                    MissionController.Instance.AddBin(MissionController.Instance.GetCurrentBin().GetRaccoon());
                    break;
                case Upgrades.raccoonPriceUp:
                    //increase the sell price of all raccoons by a percent (5%?)
                    for (int i = 0; i < sellPrice.Length; i++)
                    {
                        //ugh probably should have used a double and not a float but I'm too lazy to change all that bullshit again
                        sellPrice[i] *= 1.05f;
                    }
                    break;
                case Upgrades.autoSellMachine:
                    //activate some method that sells any raccoons that might have pushed bins over capacity
                    //activate for all bins? probably prudent
                    MissionController.Instance.GetCurrentBin().AutoSellActivate();
                    break;
                default:
                    Debug.LogError("Upgrade type missing!!");
                    break;
            }
        }
        else
        {
            Debug.LogError("No valid upgrade found! Or no one cares about upgrades...");
        }
    }

    void CloseShop()
    {
        Canvas shoppeUI = GetComponentInParent<Canvas>();

        if (shoppeUI != null)
        {
            shoppeUI.enabled = false;
        }
        else
        {
            Debug.Log("Shop canvas not found!");
        }
    }

    //Open the upgrade tab/section of the store on button press
    void OpenUpgradesTab()
    {
        //deactivate the upgrade tab button once the actual tab has been opened
        upgradesTab.interactable = false;

        //swap the active tabs
        raccoonTabObject.SetActive(false);
        upgradesTabObject.SetActive(true);

        //enable the main shop button/tab so you can switch back
        mainTab.interactable = true;
    }

    //Open the main/raccoon tab/section of the store on button press
    void OpenRaccoonTab()
    {
        //disable the main tab/button now that it's opening
        mainTab.interactable = false;

        //swap active tabs
        upgradesTabObject.SetActive(false);
        raccoonTabObject.SetActive(true);

        upgradesTab.interactable = true;
    }

    //get the buy price of a raccoon
    public float GetBuyPrice(MissionController.Type raccoonType)
    {
        return buyPrice[(int)raccoonType];
    }

    public float GetSellPrice(MissionController.Type raccoonType)
    {
        return sellPrice[(int)raccoonType];
    }

    public float GetUpgradeBuyPrice(Upgrades upgradeType)
    {
        return powerUpPrice[(int)upgradeType];
    }
}