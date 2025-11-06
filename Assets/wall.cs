using System.Collections;
using System.Collections.Generic;
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
        this.transform.localScale = new Vector3(currentScale.x, currentScale.y, (currentScale.z + (originalScale * d)));
    }

    public void RestorePosition(Vector3 direction)
    {
        this.transform.position -= (direction);
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
