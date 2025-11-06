using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class wall : MonoBehaviour
{

    public float originalScale;
    public Vector3 orientation;
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
        originalScale = scale.z;
        orientation = this.transform.right;
    }
    public void RestoreScale(float d)
    {
        Vector3 currentScale = this.transform.localScale;
        this.transform.localScale = new Vector3(currentScale.x, currentScale.y, (currentScale.z * d));
    }

    public void RestorePosition(Vector3 direction)
    {
        Vector3 movevector;
        float scaleCoficient;
        if (orientation.z >= 0.5 || orientation.z <= -0.5)
        {
            movevector = new Vector3(direction.x, direction.y, direction.z / 2);
            scaleCoficient = 1 + (Math.Abs(direction.z) / this.GetComponent<Collider>().bounds.size.x);
        }
        else
        {
            movevector = new Vector3(direction.x / 2, direction.y, direction.z);
            scaleCoficient = 1 + (Math.Abs(direction.x) / this.GetComponent<Collider>().bounds.size.z);
        }
        print(this.GetComponent<Collider>().bounds.size);
        print("direction: ");
        print(direction);
        print("We are moving: ");
        print(movevector);
        print("and scaling: ");
        print(scaleCoficient);
        RestoreScale(scaleCoficient);
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
}
