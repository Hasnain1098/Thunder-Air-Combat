using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float heightoffset;

    private Vector2 position;
    internal void updatePosition(float x,float y)
    {
        position.x += y;
        position.y += x;
    }
    void lateUpdate()
    {
        transform.rotation = Quaternion.Euler(position.x,position.y,0);
        transform.position = transform.rotation * new Vector3(0, heightoffset, -distance) + target.position;

    }
}
