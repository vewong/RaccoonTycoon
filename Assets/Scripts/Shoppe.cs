using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shoppe : MonoBehaviour 
{
    int[] buyPrice, sellPrice, powerUpPrice;
    string powerUps;

    public Button sellButton, buyButton;

	// Use this for initialization
	void Start () 
    {
        int startBuyPrice = 5;
        int startSellPrice = 3;

        buyPrice = new int[MissionController.Instance.GetNumTypes()];
        sellPrice = new int[MissionController.Instance.GetNumTypes()];

	    for (int i = 0; i < buyPrice.Length; i++)
        {
            buyPrice[i] = startBuyPrice;
            sellPrice[i] = startSellPrice;

            startBuyPrice *= startBuyPrice;
            startSellPrice *= startSellPrice;
        }

        
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 40, 80, 20), "Sell Raccoon"))
        {
            SellRaccoon(MissionController.Instance.GetCurrentBin().GetRaccoon());
        }
    }

    int SellRaccoon(Raccoon raccoon)
    {
        if (MissionController.sellEventHandler != null)
        {
            // Call all the methods that have subscribed to the delegate
            MissionController.sellEventHandler(raccoon, 1);
        }

        //how to get the number value based on enum?
        return sellPrice[(int)raccoon.GetEnumType()];
    }
}
