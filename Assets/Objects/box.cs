using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class box : MonoBehaviour
{
    public bool playerWatching = false;
    public bool canOpen = true;
    public bool canSpawnKey = false;
    public bool opened= false;
    public Slider slide1;
    public Slider slide2;
    private float watchCounter = 0f;
    public float requiredWatchTime = 3f;

    public Room roomRef;

    //Add an array with different prefabs to create random surprise
    public GameObject[] surprisePrefabs;

    public GameObject key;
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
                playerWatching = false;
                watchCounter = 0f;
                if (!opened)
                {
                    opened = true;
                    this.GetComponent<AudioSource>().Stop();
                    openBox();
                    print("DOOR OPENED");
                    
                }
                
                //activate door
                
                
                


            }
        }
        else
        {
            if( slide1!= null && slide2 != null)
            {
                slide1.value = 0f;
            slide2.value = 0f;
            }
            
            watchCounter = 0f;

        }
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player")
        {
            playerWatching = true;
            this.GetComponent<AudioSource>().Play();

            
            print("DOOR TRIGGERED");
        }
           
        
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.tag == "Player")
        {
            this.GetComponent<AudioSource>().Stop();
            playerWatching = false;
            print("DOOR UNTRIGGERED");
        }
           
        
    }


private void openBox()
    {
        if(canSpawnKey)
        {
            //Spawn key
            Instantiate(key, this.transform.position , Quaternion.identity);
            roomRef.keySpawned();
        }
        else
        {
            int index = Random.Range(0, surprisePrefabs.Length);
        Instantiate(surprisePrefabs[index], this.transform.position, Quaternion.identity);
        roomRef.boxOpened();
        
        }
        //Create an istance of a random prefab from the array
        


        //destry this
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
public void enableBox()
    {
        canOpen = true;
    }
   
   public void SpawnKey()
    {
        canSpawnKey = true;
    }
   
}
