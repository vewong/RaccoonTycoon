using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ShopRaccoon : MonoBehaviour, IPointerEnterHandler
{
    public Button buyButton;
    public Text raccoonText;
    public Image raccoonPortrait;
    public Sprite[] portraits;

    //public delegate void SellEvent(Raccoon raccoonSold, int moneyEarned);
    //public static SellEvent sellEventHandler;

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

    void Awake()
    {
        if (portraits.Length < (int)MissionController.Type.count)
        {
            Debug.LogError("Portrait sprites not assigned!");
        }
    }

    void OnEnable()
    {
        MissionController.sellEventHandler += HandleSellEvent;
    }

    void OnDisable()
    {
        MissionController.sellEventHandler -= HandleSellEvent;
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

    //setters
    public void SetRaccoonPortrait (Sprite newPortrait)
    {
        raccoonPortrait.sprite = newPortrait;
    }

    //delegates
    void HandleSellEvent(Raccoon parent, float price)
    {
        //check if this ShopRaccoon's buy price is now lower than the current money total
        if (Shoppe.Instance.GetBuyPrice(GetRaccoonType()) <= MissionController.Instance.CheckMoney())
        {
            //reveal this raccoon's portrait, if so
            RevealPortrait();
        }

        //hide portraits if you don't have enough for those raccoons?
        //probably not...
    }

    //other methods
    public void OnPointerEnter(PointerEventData pointerData)
    {
        Debug.Log("Shop " + raccoonType + " " + pointerData.position);

        if (MissionController.hoverEventHandler != null)
        {
            MissionController.hoverEventHandler(null, this, null);
        }
    }

    public void RevealPortrait()
    {
        //reveal the current raccoon's correct portrait
        raccoonPortrait.sprite = portraits[(int)raccoonType];
    }
}
