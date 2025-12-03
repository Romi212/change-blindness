using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float lifetime = 5.0f;    
    private float lifeCounter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the key
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Update lifetime
        lifeCounter += Time.deltaTime;
        if (lifeCounter >= lifetime)
        {
            Destroy(this.gameObject);
        }
        
    }
}
