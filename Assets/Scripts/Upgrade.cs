using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Upgrade : MonoBehaviour, IPointerEnterHandler 
{
    public Button buyButton;
    public Text upgradeText;

    //public delegate void SellEvent(Raccoon raccoonSold, int moneyEarned);
    //public static SellEvent sellEventHandler;

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

            }

            //Debug.Log("Raccoon text: " + raccoonText.text + "\nRaccoon type: " + raccoonType.ToString());
        }
	}
	
	// Update is called once per frame
	void Update () 
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
}
