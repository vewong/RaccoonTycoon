using UnityEngine;
using System.Collections;

public class BounceAnimation : MonoBehaviour 
{
    public Sprite mySprite;
    public Transform position;
    public Animator animator;
    public AudioClip hopSound;

    float jumpTime;
    public bool bounceTrigger;

	// Use this for initialization
	void Start () 
    {
        jumpTime = Random.Range(5, 15);

        if (animator != null)
        {
            //animator = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Help! Someone forgot to set the animator!");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (jumpTime <= 0)
        {
            bounceTrigger = true;
            Debug.Log("Hop!");
            jumpTime = Random.Range(5, 15);

            audio.PlayOneShot(hopSound);
        }
        else
        {
            if (bounceTrigger)
            {
                bounceTrigger = false;
            }

            jumpTime -= Time.deltaTime;
        }

        animator.SetBool("bounceTrigger", bounceTrigger);
	}
}
