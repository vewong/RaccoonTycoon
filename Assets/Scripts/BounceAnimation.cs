using UnityEngine;
using System.Collections;

public class BounceAnimation : MonoBehaviour 
{
    public Sprite mySprite;
    public Transform position;

    float jumpTime;
    public bool bounceTrigger;

	// Use this for initialization
	void Start () 
    {
        jumpTime = 30f;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (jumpTime <= 0)
        {
            bounceTrigger = true;
            Debug.Log("Hop!");
        }
        else
        {
            if (bounceTrigger)
            {
                bounceTrigger = false;
            }

            jumpTime -= Time.deltaTime;
        }
	}
}
