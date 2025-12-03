using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat : MonoBehaviour
{
    public float velocity = 1.0f;
    public float lifetime = 5.0f;
    private float lifeCounter = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeCounter += Time.deltaTime;
        if(lifeCounter >= lifetime)
        {
            this.GetComponent<AudioSource>().Stop();
            Destroy(this.gameObject);
        }
        //move random
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);
        transform.Rotate(0, 2f , 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Wall")
        {
            transform.Rotate(0,100,0);
        }
    }
}
