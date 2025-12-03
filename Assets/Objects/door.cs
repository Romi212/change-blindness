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
    public sewin sewingObj;

    public GameObject text1;
    public GameObject text2;
public AudioSource openAudio;
    public AudioSource closeAudio;
    public GameObject[] doorWals;
    public GameObject[] adjWalls;
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
                    var sources = GetComponents<AudioSource>();
        if (openAudio == null && sources.Length > 0) openAudio = sources[0];
        if (closeAudio == null && sources.Length > 1) closeAudio = sources[1];
        
                if (!opened)
                {
                    toRotate.transform.Rotate(0,90,0);
                    print("DOOR OPENED");
                    opened = true;
                     if (openAudio != null) openAudio.Play();
                        else GetComponent<AudioSource>()?.Play();
                    foreach(GameObject w in adjWalls)
                    {
                        w.SetActive(false);
                    }
                    foreach(GameObject w in doorWals)
                    {
                        w.SetActive(true);
                    }
                }
                else
                {
                    toRotate.transform.Rotate(0,-90,0);
                    print("DOOR CLOSED");
                    opened = false;
                    if (closeAudio != null) closeAudio.Play();
                    foreach(GameObject w in adjWalls)
                    {
                        w.SetActive(true);
                    }
                    foreach(GameObject w in doorWals)
                    {
                        w.SetActive(false);
                    }
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

            if (!canOpen)
            {
                text1.SetActive(true);
                text2.SetActive(true);
            }
            
            print("DOOR TRIGGERED");
        }
           
        
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.tag == "Player")
        {
            playerWatching = false;
            if (!canOpen)
            {
                text1.SetActive(false);
                text2.SetActive(false);
            }
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
            var sources = GetComponents<AudioSource>();
        if (openAudio == null && sources.Length > 0) openAudio = sources[0];
        if (closeAudio == null && sources.Length > 1) closeAudio = sources[1];
            if (closeAudio != null) closeAudio.Play();
            canOpen = false;
            foreach(GameObject w in adjWalls)
            {
                w.SetActive(true);
            }
            foreach(GameObject w in doorWals)
                    {
                        w.SetActive(false);
                    }
        }
    }

public void enableDoor()
    {
        canOpen = true;
        sewingObj.gotSissors();
    }
    //player has to watch for 5 seconds for door to activate
   
}
