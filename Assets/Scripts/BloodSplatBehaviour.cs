using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatBehaviour : MonoBehaviour
{
    public int bleedSpeed;
    
    private Vector3 initPosition;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        targetPosition = new Vector3(initPosition.x, initPosition.y - 1, initPosition.z);
    }

    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, bleedSpeed * Time.deltaTime);
    }
}
