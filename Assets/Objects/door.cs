using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool playerWatching = false;
    public GameObject toRotate;
    public bool opened= false;
    private float watchCounter = 0f;
    public float requiredWatchTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerWatching)
        {
            watchCounter += Time.deltaTime;
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

    //player has to watch for 5 seconds for door to activate
   
}
