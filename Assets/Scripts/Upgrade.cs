using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Upgrade : MonoBehaviour, IPointerEnterHandler 
{
    public Button buyButton;
    public Text upgradeText;

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
                    break;
                case "Fertility Treatment":
                    upgradeType = Shoppe.Upgrades.breedTimeDown;
                    break;
                case "Incubators":
                    upgradeType = Shoppe.Upgrades.reproNumUp;
                    break;
                case "Glitter":
                    upgradeType = Shoppe.Upgrades.raccoonPriceUp;
                    break;
                case "Bin Capacity":
                    upgradeType = Shoppe.Upgrades.binCapacityUp;
                    break;
                case "Auto-sell Raccoons":
                    upgradeType = Shoppe.Upgrades.autoSellMachine;
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
	
	// Update is called once per frame
	void Update () 
    {

	}

    //delegates
    void HandleUpgradeEvent(float buyPrice)
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerData)
    {
        Debug.Log("Shop " + upgradeType + " " + pointerData.position);

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
