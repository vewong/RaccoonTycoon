using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Shoppe : MonoBehaviour
{
    int[] buyPrice, sellPrice, powerUpPrice;
    string powerUps;

    public Button sellButton, buyButton, exitButton;
    public AudioClip sellSound, buySound;
    public GameObject raccoonDisplay, scrollableContentContainer;

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

        int startBuyPrice = 5;
        int startSellPrice = 3;

        buyPrice = new int[MissionController.Instance.GetNumTypes()];
        sellPrice = new int[MissionController.Instance.GetNumTypes()];

        List<Text> displayStrings = new List<Text>();

        //populate buy and sell price arrays, initialize a whole bunch of shop entries
        for (int i = 0; i < buyPrice.Length; i++)
        {
            buyPrice[i] = startBuyPrice;
            sellPrice[i] = startSellPrice;

            startBuyPrice *= startBuyPrice;
            startSellPrice *= startSellPrice;

            GameObject tempObject;
            tempObject = Instantiate(raccoonDisplay) as GameObject;
            tempObject.transform.parent = scrollableContentContainer.transform;

            raccoonDisplay.GetComponentsInChildren<Text>(displayStrings);

            for (int j = 0; j < displayStrings.Count; j++)
            {
                Debug.Log("index " + j + ": " + displayStrings[j].text);
            }

            //assuming the first text is the name and the second is the price display
            displayStrings[0].text = (MissionController.Instance.NumToEnum<MissionController.Type>(i)).ToString();
            displayStrings[1].text = "$" + buyPrice[i];
        }
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
}