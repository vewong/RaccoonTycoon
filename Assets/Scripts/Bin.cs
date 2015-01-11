using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bin : MonoBehaviour, IPointerEnterHandler
{
    Raccoon currRaccoon;
    int maxRaccoons, currRaccoons;
    //List<int> pairOfRaccoons = new List<int>();
    float timeToNextBreed;
    bool autoSell = false;
    public Text binName, raccoonCountText;
    public Button sellButton; 

	// Use this for initialization
	void Start () 
    {
	    //PROTOTYPE STUFFFF
        //currRaccoon = Raccoon.CreateInstance("Raccoon") as Raccoon;
        //currRaccoon.Initialize(MissionController.Type.trash, 3f, 5f, 2, 4);
        maxRaccoons = 25;
        currRaccoons = 2;

        //timeToNextBreed = currRaccoon.GetReproTime();

        if (binName == null)
        {
            Debug.Log("Bin name left blank");
        }

        //set up the sell button
        if (sellButton != null)
        {
            sellButton.onClick.AddListener(delegate { Shoppe.Instance.SellRaccoon(); });
        }
        else
        {
            Debug.Log("Sell button NULL!");
        }
	}

    public void Initialize(Raccoon raccoonType, int capacity, int raccoonCount)
    {
        currRaccoon = raccoonType;
        maxRaccoons = capacity;
        currRaccoons = raccoonCount;
    }

    public void Initialize(Raccoon raccoonType, int capacity)
    {
        Debug.Log("Initialize");
        currRaccoon = raccoonType;
        maxRaccoons = capacity;
    }

    void OnEnable()
    {
        MissionController.sellEventHandler += HandleSellEvent;
        MissionController.buyEventHandler += HandleBuyEvent;
    }

    void OnDisable()
    {
        MissionController.sellEventHandler -= HandleSellEvent;
        MissionController.buyEventHandler -= HandleBuyEvent;
    }

    //getter
    public int GetRaccoonsInBin()
    {
        return currRaccoons;
    }

    public Raccoon GetRaccoon()
    {
        return currRaccoon;
    }

    public int GetCapacity()
    {
        return maxRaccoons;
    }

    //setters
    void SetCapacity(int capacity)
    {
        maxRaccoons = capacity;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (currRaccoon != null)
        {
            if (timeToNextBreed <= 0 && currRaccoons >= 2)
            {
                currRaccoons += currRaccoon.Multiply();

                //need to add something in here about selling the excess raccoons if powerup is available
                if (currRaccoons > maxRaccoons)
                {
                    if (!autoSell)
                    {
                        currRaccoons = maxRaccoons;
                    }
                    else
                    {
                        Debug.Log("Auto selling raccoons!");
                        //get the difference between the max bin capacity and the current raccoons trying to be put into the bin
                        int autoSellRaccoons = currRaccoons - maxRaccoons;
                        Debug.Log(" Num to sell: " + autoSellRaccoons);

                        //call sell for each raccoon being sold
                        for (int i = 0; i < autoSellRaccoons; i++)
                        {
                            if (MissionController.sellEventHandler != null)
                            {
                                // Call all the methods that have subscribed to the delegate
                                MissionController.sellEventHandler(currRaccoon, Shoppe.Instance.GetSellPrice(currRaccoon.GetEnumType()));
                            }
                            else
                            {
                                Debug.Log("No one is subscribed to sell events ;_;");
                            }

                            Debug.Log("Sold " + i + " raccoons on auto sell");
                        }
                    }
                }

                timeToNextBreed = currRaccoon.GetReproTime();
            }
            else
            {
                timeToNextBreed -= Time.deltaTime;
            }
        }
	}

    void OnGUI()
    {
        if (currRaccoon != null)
        {
            binName.text = currRaccoon.Type();
        }
        else
        {
            binName.text = "None";
        }

        if (raccoonCountText != null)
        {
            raccoonCountText.text = "Raccoons: " + currRaccoons;
        }
        else
        {
            raccoonCountText.text = "ERRORERRORERROR";
        }
    }

    //other methods
    void HandleSellEvent(Raccoon parent, float price)
    {
        //subtract raccoons from bin
        currRaccoons--;
    }

    void HandleBuyEvent(float buyPrice)
    {
        currRaccoons++;
    }

    public void OnPointerEnter(PointerEventData pointerData)
    {
        Debug.Log("Bin " + binName);

        if (MissionController.hoverEventHandler != null)
        {
            MissionController.hoverEventHandler(this, null, null);
        }
    }

    public void IncreaseBinCapacity()
    {
        //more fake math...
        float maxRaccoonsFloat = maxRaccoons;

        maxRaccoonsFloat *= 1.1f;

        Debug.Log("Fake math test bins: (float) " + maxRaccoonsFloat);

        maxRaccoons = (int)maxRaccoonsFloat;

        Debug.Log("Fake math test bins: (int) " + maxRaccoons);
    }
    
    public void AutoSellActivate()
    {
        Debug.Log("Activating auto sell!");
        autoSell = true;
    }
}
