using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Bin : MonoBehaviour 
{
    Raccoon currRaccoon;
    int maxRaccoons, currRaccoons;
    //List<int> pairOfRaccoons = new List<int>();
    float timeToNextBreed;
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
                    currRaccoons = maxRaccoons;
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
    void HandleSellEvent(Raccoon parent, int price)
    {
        //subtract raccoons from bin
        if (parent.Equals(currRaccoon))
        {
            currRaccoons--;
        }
    }

    void HandleBuyEvent(int buyPrice)
    {
        currRaccoons++;
    }

    void OnMouseEnter()
    {
        //Debug.Log("Hello!");

        if (MissionController.hoverEventHandler != null)
        {
            MissionController.hoverEventHandler(this, null);
        }
    }

    //void OnMouseExit()
    //{
    //    Debug.Log("Goodbye!");
    //}
}
