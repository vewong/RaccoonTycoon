using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ShopRaccoon : MonoBehaviour, IPointerEnterHandler
{
    public Button buyButton;
    public Text raccoonText;

    MissionController.Type raccoonType;

	// Use this for initialization
	void Start () 
    {
        //set up the buy button
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(delegate { Shoppe.Instance.BuyRaccoon(); });
        }
        else
        {
            Debug.Log("Buy button NULL!");
        }

        if (raccoonText != null)
        {
            //process the text into the enum
            raccoonType = (MissionController.Type)Enum.Parse(typeof(MissionController.Type), raccoonText.text);

            //Debug.Log("Raccoon text: " + raccoonText.text + "\nRaccoon type: " + raccoonType.ToString());
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //getters
    public MissionController.Type GetRaccoonType()
    {
        return raccoonType;
    }

    //other methods
    public void OnPointerEnter(PointerEventData pointerData)
    {
        Debug.Log("Shop " + raccoonType + " " + pointerData.position);

        if (MissionController.hoverEventHandler != null)
        {
            MissionController.hoverEventHandler(null, this);
        }
    }
}
