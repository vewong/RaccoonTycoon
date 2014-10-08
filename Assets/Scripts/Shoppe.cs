using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Shoppe : MonoBehaviour
{
    int[] buyPrice, sellPrice, powerUpPrice;
    string[] powerUps;

    public Button sellButton, buyButton, exitButton, mainTab, upgradesTab;
    public AudioClip sellSound, buySound;
    public GameObject raccoonDisplay, scrollableContentContainer, raccoonTabObject, upgradesTabObject;
    //public Text moneyDisplay;

    // Use this for initialization
    void Start()
    {
        //set up the sell button
        if (sellButton != null)
        {
            sellButton.onClick.AddListener(delegate { SellRaccoon(); });
        }
        else
        {
            Debug.Log("Sell button NULL!");
        }

        //set up the buy button
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(delegate { BuyRaccoon(); });
        }
        else
        {
            Debug.Log("Sell button NULL!");
        }

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

        buyPrice = new int[MissionController.Instance.GetNumTypes()];
        sellPrice = new int[MissionController.Instance.GetNumTypes()];

        List<Text> displayStrings = new List<Text>();

        //populate buy and sell price arrays, initialize a whole bunch of shop entries
        for (int i = 0; i < buyPrice.Length-1; i++)
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
        int raccoonCount = currentBin.GetRaccoonsInBin();
        Raccoon typeRaccoon = currentBin.GetRaccoon();

        if (raccoonCount <= 0)
        {
            sellButton.enabled = false;
        }
        else
        {
            sellButton.enabled = true;
        }

        int currentFunds = MissionController.Instance.CheckMoney();

        if (currentFunds <= 0 || buyPrice[(int)typeRaccoon.GetEnumType()] > currentFunds)
        {
            buyButton.enabled = false;
        }
        else if (currentBin.GetRaccoonsInBin() == currentBin.GetCapacity())
        {
            buyButton.enabled = false;
        }
        else
        {
            buyButton.enabled = true;
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
        //hopefully a raccoon type being bought will get passed in?
        Bin currentBin = MissionController.Instance.GetCurrentBin();

        if (MissionController.buyEventHandler != null)
        {
            // Call all the methods that have subscribed to the delegate
            MissionController.buyEventHandler(buyPrice[(int)currentBin.GetRaccoon().GetEnumType()]);
        }

        audio.PlayOneShot(buySound);
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