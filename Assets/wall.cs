using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class wall : MonoBehaviour
{

    public float originalScale;
    public Vector3 orientation;
    public GameObject DarkScreen;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize()
    {
        Vector3 scale = this.transform.localScale;
        originalScale = 2f;
        orientation = this.transform.right;
    }
    public void RestoreScale(float d)
    {
        Vector3 currentScale = this.transform.localScale;
        this.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z + d);
    }

    public void RestorePosition(Vector3 direction)
    {
        Vector3 movevector;
        float scaleCoficient;
        if (orientation.z >= 0.5 || orientation.z <= -0.5)
        {
            movevector = new Vector3(direction.x, 0, 0);
            //scaleCoficient = 1 + (Math.Abs(direction.z) / this.GetComponent<Collider>().bounds.size.x);
        }
        else
        {
            movevector = new Vector3(0, 0, direction.z);
           // scaleCoficient = 1 + (Math.Abs(direction.x) / this.GetComponent<Collider>().bounds.size.z);
        }
        print(this.GetComponent<Collider>().bounds.size);
        print("direction: ");
        //print(direction);
       // print("We are moving: ");
       // print(movevector);
       // print("and scaling: ");
        //print(scaleCoficient);
       // RestoreScale(scaleCoficient);
        this.transform.position -= movevector;
        
        
    }
    public float RestoreOriginalScale(float d) {
        float currentScale = this.transform.localScale.z;
        float diference = originalScale - currentScale;
        RestoreScale(diference);
        return diference;
    }
    public void moveWall(Vector3 dir)
    {
        this.transform.position -= dir;
    }
    
    public void fillRoom(Vector3 direction) {
        Vector3 movevector;
        float scaleCoficient;
        if (orientation.z >= 0.5 || orientation.z <= -0.5)
        {
           
            movevector = new Vector3(0, 0, direction.z);
            //scaleCoficient = 1 + (Math.Abs(direction.z) / this.GetComponent<Collider>().bounds.size.x);
        }
        else
        {
            movevector = new Vector3(direction.x, 0, 0);
            // scaleCoficient = 1 + (Math.Abs(direction.x) / this.GetComponent<Collider>().bounds.size.z);
        }
        this.transform.position -= (movevector);
    }

    public Vector3 getOrientation()
    {
        return orientation;
    }

    public Vector3 getPosition()
    {
        return this.transform.position;
    }

    public Bounds getBounds()
    {
        return GetComponent<Collider>().bounds;
    }

    public bool isScaled()
    {
        float currentScale = this.transform.localScale.z;
        return currentScale == originalScale;
    }

    public float getScaleDiference()
    {
        float currentScale = this.transform.localScale.z;
        return originalScale - currentScale;
    }


    // on trigger dark
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            DarkScreen.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            DarkScreen.SetActive(false);
        }
    }
}
