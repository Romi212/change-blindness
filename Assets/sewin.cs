using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sewin : MonoBehaviour
{

    public GameObject text1;
    public GameObject text2;

    public AudioSource audioSourceSewing;
    public GameObject clothes;
    public GameObject donee;
    public GameObject donee2;
    public float requiredWatchTime = 3f;
    public bool playerWatching = false;
    private bool textActive = false;
    private float watchCounter = 0f;

    public float sewingTime = 5f;
    public float sewingCounter = 0f;

    public bool noSissors = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(textActive)
        {
            watchCounter += Time.deltaTime;
            if(watchCounter >= requiredWatchTime)
            {
                text1.SetActive(false);
                text2.SetActive(false);
                watchCounter = 0f;
                textActive = false;
            }
        }

        if(playerWatching && !noSissors)
        {
            sewingCounter += Time.deltaTime;
            if(sewingCounter >= sewingTime)
            {
                audioSourceSewing.Stop();
                donee.SetActive(true);
                donee2.SetActive(true);
                clothes.SetActive(true);
                noSissors = true;
                
            }
        }
        
    }


private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Player" && noSissors)
        {
            playerWatching = true;
            textActive = true;
            text1.SetActive(true);
            text2.SetActive(true);
            this.GetComponent<AudioSource>().Play();


        }
        if( other.tag == "Player" && !noSissors)
        {
            playerWatching = true;
            audioSourceSewing.Play();


        }
           
        
    }
    public void gotSissors()
    {
        noSissors = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.tag == "Player")
        {
            playerWatching = false;
            
        }
           
        
    }
}
