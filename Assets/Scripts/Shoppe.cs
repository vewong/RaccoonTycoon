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
        sellButton.onClick.AddListener(SellRaccoon);

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

    }

    public void SellRaccoon()
    {
        //I guess just assume the object is a raccoon since there's no way to check easily
        Raccoon replacementRaccoon = null; // (Raccoon)raccoon;

        if (MissionController.sellEventHandler != null)
        {
            // Call all the methods that have subscribed to the delegate
            MissionController.sellEventHandler(replacementRaccoon, sellPrice[(int)replacementRaccoon.GetEnumType()]);
        }

        //how to get the number value based on enum?
        //FindSellPrice(replacementRaccoon);
    }

    int FindSellPrice(Raccoon raccoon)
    {
        return sellPrice[(int)raccoon.GetEnumType()];
    }
}
