using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Upgrade : MonoBehaviour, IPointerEnterHandler
{
    public Button buyButton;
    public Text upgradeText;
    public Text upgradeDescription;

    int upgradedTimes = 1;
    string upgradeName;

    Shoppe.Upgrades upgradeType;
    
	// Use this for initialization
	void Start () 
    {
        //set up the buy button
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(delegate { Shoppe.Instance.BuyUpgrade(); });
        }
        else
        {
            Debug.Log("Buy (upgrade) button NULL!");
        }

        //assign upgrade enum
        if (upgradeText != null)
        {
            //process the text into the enum
            switch (upgradeText.text)
            {
                case "New Bin":
                    upgradeType = Shoppe.Upgrades.bin;
                    upgradeDescription.text = "A new bin to add raccoons to.";
                    break;
                case "Fertility Treatment":
                    upgradeType = Shoppe.Upgrades.breedTimeDown;
                    upgradeDescription.text = "Decrease the time it takes for your raccoons to breed.";
                    break;
                case "Incubators":
                    upgradeType = Shoppe.Upgrades.reproNumUp;
                    upgradeDescription.text = "Increase the number of baby raccoons!";
                    break;
                case "Glitter":
                    upgradeType = Shoppe.Upgrades.raccoonPriceUp;
                    upgradeDescription.text = "Increase the selling price of raccoons.";
                    break;
                case "Bin Capacity":
                    upgradeType = Shoppe.Upgrades.binCapacityUp;
                    upgradeDescription.text = "Your bins now hold more raccoons!";
                    break;
                case "Auto-sell Raccoons":
                    upgradeType = Shoppe.Upgrades.autoSellMachine;
                    upgradeDescription.text = "A robot that will sell any raccoons that don't fit in your bins.";
                    break;
                default:
                    Debug.LogError("Upgrade type missing!");
                    break;
            }
        }

        upgradeName = upgradeText.text;
        upgradeText.text += " " + upgradedTimes;
	}

    void OnEnable()
    {
        MissionController.buyUpgradeEventHandler += HandleUpgradeEvent;
    }

    void OnDisable()
    {
        MissionController.buyUpgradeEventHandler -= HandleUpgradeEvent;
    }
	
	void OnGUI() 
    {
        float currentFunds = MissionController.Instance.CheckMoney();

        if (currentFunds >= Shoppe.Instance.GetUpgradeBuyPrice(upgradeType))
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
	}

    //delegates
    void HandleUpgradeEvent(float buyPrice, Upgrade upgradePurchased)
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerData)
    {
        //Debug.Log("Shop " + upgradeType + " " + pointerData.position);

        if (MissionController.hoverEventHandler != null)
        {
            MissionController.hoverEventHandler(null, null, this);
        }
    }

    //getters
    public Shoppe.Upgrades GetUpgradeType()
    {
        return upgradeType;
    }

    //setters
    public void AddUpgradeCount()
    {
        upgradedTimes++;

        upgradeText.text = upgradeName + " " + upgradedTimes;
    }
}
