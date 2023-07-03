using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CameraMarker : MonoBehaviour
{
    public Vector3 position;
    public int associatedPlayerCP;
    public CameraMarker OnDeathMarker;

    void Update()
    {
        position = transform.position;
    }
}
