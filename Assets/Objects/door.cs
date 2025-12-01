using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class door : MonoBehaviour
{
    public bool playerWatching = false;
    public bool canOpen = true;
    public GameObject toRotate;
    public bool opened= false;
    public Slider slide1;
    public Slider slide2;
    private float watchCounter = 0f;
    public float requiredWatchTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerWatching && canOpen)
        {
            watchCounter += Time.deltaTime;
            slide1.value = Mathf.Clamp01(watchCounter / requiredWatchTime);
            slide2.value = Mathf.Clamp01(watchCounter / requiredWatchTime);
            if(watchCounter >= requiredWatchTime)
            {
                if (!opened)
                {
                    toRotate.transform.Rotate(0,90,0);
                    print("DOOR OPENED");
                    opened = true;
                }
                else
                {
                    toRotate.transform.Rotate(0,-90,0);
                    print("DOOR CLOSED");
                    opened = false;
                }
                //activate door
                
                playerWatching = false;
                watchCounter = 0f;
                


            }
        }
        else
        {
            slide1.value = 0f;
            slide2.value = 0f;
            watchCounter = 0f;

        }
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player")
        {
            playerWatching = true;

            
            print("DOOR TRIGGERED");
        }
           
        
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.tag == "Player")
        {
            playerWatching = false;
            print("DOOR UNTRIGGERED");
        }
           
        
    }

public void closeDoor()
    {
        if (opened)
        {
            toRotate.transform.Rotate(0, -90, 0);
            print("DOOR CLOSED");
            opened = false;
            canOpen = false;
        }
    }

public void enableDoor()
    {
        canOpen = true;
    }
    //player has to watch for 5 seconds for door to activate
   
}
