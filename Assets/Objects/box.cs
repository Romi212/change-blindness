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
                if (!opened)
                {
                    openBox();
                    print("DOOR OPENED");
                    opened = true;
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


private void openBox()
    {
        if(canSpawnKey)
        {
            //Spawn key
            Instantiate(key, this.transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
        }
        else
        {
            int index = Random.Range(0, surprisePrefabs.Length);
        Instantiate(surprisePrefabs[index], this.transform.position, Quaternion.identity);
        
        }
        //Create an istance of a random prefab from the array
        


        //destry this
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
