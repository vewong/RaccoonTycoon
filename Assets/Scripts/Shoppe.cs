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

    int[] buyPrice, sellPrice, powerUpPrice;
    string[] powerUps;

    public Button exitButton, mainTab, upgradesTab;
    public AudioClip sellSound, buySound;
    public GameObject raccoonDisplay, scrollableContentContainer, raccoonTabObject, upgradesTabObject;
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

        int startBuyPrice = 5;
        int startSellPrice = 3;

        buyPrice = new int[MissionController.Instance.GetNumTypes()-1];
        sellPrice = new int[MissionController.Instance.GetNumTypes()-1];

        List<Text> displayStrings = new List<Text>();

        //populate buy and sell price arrays, initialize a whole bunch of shop entries
        for (int i = 0; i < buyPrice.Length; i++)
        {
            buyPrice[i] = startBuyPrice;
            sellPrice[i] = startSellPrice;

            startBuyPrice *= 2;
            startSellPrice *= 2;

            GameObject tempObject;
            tempObject = Instantiate(raccoonDisplay) as GameObject;
            tempObject.transform.parent = scrollableContentContainer.transform;

            tempObject.GetComponentsInChildren<Text>(displayStrings);

            //assuming the first text is the name and the second is the price display
            displayStrings[0].text = (MissionController.Instance.NumToEnum<MissionController.Type>(i)).ToString();
            displayStrings[1].text = "$" + buyPrice[i];
        }

        //now the original prefab isn't needed, so deactivate it
        raccoonDisplay.SetActive(false);
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

        Debug.Log("Bin " + (currentBin != null ? currentBin.GetRaccoon().Type() : "null") + " Raccoon " + (currentShopRaccoon != null ? currentShopRaccoon.GetRaccoonType().ToString() : "null"));

        if (currentBin != null)
        {
            if (currentBinRaccoonCount <= 0)
            {
                currentBin.sellButton.enabled = false;
            }
            else
            {
                currentBin.sellButton.enabled = true;
            }
        }

        if (currentShopRaccoon != null)
        {
            int currentFunds = MissionController.Instance.CheckMoney();
            Debug.Log("Moneys: " + currentFunds + "  Current Price: " + buyPrice[(int)currentShopRaccoon.GetRaccoonType()]);

            if (currentFunds > buyPrice[(int)currentShopRaccoon.GetRaccoonType()] && currentBinRaccoonCount < currentBin.GetCapacity())
            {
                currentShopRaccoon.buyButton.enabled = true;
                //hopefully this will change the portrait when there's enough money in the baaank
                currentShopRaccoon.RevealPortrait();
            }
            else
            {
                currentShopRaccoon.buyButton.enabled = false;
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

        audio.PlayOneShot(sellSound);
    }

    public void BuyRaccoon()
    {
        //get the raccoon the player is trying to buy from the shop
        ShopRaccoon currentRaccoon = MissionController.Instance.GetCurrentShopRaccoon();

        if (MissionController.buyEventHandler != null && currentRaccoon != null)
        {
            // Call all the methods that have subscribed to the delegate
            MissionController.buyEventHandler(buyPrice[(int)currentRaccoon.GetRaccoonType()]);
            audio.PlayOneShot(buySound);
        }
        else
        {
            Debug.Log("current shop raccoon is null!... or maybe no one is subscribed to buyEventHandler");
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
        upgradesTab.enabled = false;

        //swap the active tabs
        raccoonTabObject.SetActive(false);
        upgradesTabObject.SetActive(true);

        //enable the main shop button/tab so you can switch back
        mainTab.enabled = true;
    }

    //Open the main/raccoon tab/section of the store on button press
    void OpenRaccoonTab()
    {
        //disable the main tab/button now that it's opening
        mainTab.enabled = false;

        //swap active tabs
        upgradesTabObject.SetActive(false);
        raccoonTabObject.SetActive(true);

        upgradesTab.enabled = true;
    }
}