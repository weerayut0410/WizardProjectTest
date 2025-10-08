using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 pos;
    public GameObject Player;
    float deadZoneX = 4f;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pos.x > Player.transform.position.x + deadZoneX)
        {
            pos.x = Player.transform.position.x + deadZoneX;
            transform.position = pos;
        }
        else if (pos.x < Player.transform.position.x - deadZoneX)
        {
            pos.x = Player.transform.position.x - deadZoneX;
            transform.position = pos;
        }

        
    }
}
