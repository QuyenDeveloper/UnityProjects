using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentScript : MonoBehaviour
{
    [SerializeField]
    Transform planeTranform;
    public Vector3 pos = new Vector3(0, 0, 0);
    public float speed = 1.0f;

    float smooth = 2.0f;
    float tiltAngle = 60.0f;
    float tiltAngle2 = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.UpArrow) )
        {
            transform.position = SetZ(transform.position);
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
            float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
            float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
            Quaternion target = Quaternion.Euler(0, tiltAroundX, -tiltAroundZ);

            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        }*/
        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle2;

        Quaternion target = Quaternion.Euler(tiltAroundX, 0, -tiltAroundZ);

        planeTranform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
    Vector3 SetZ(Vector3 vector)
    {
        vector.z += speed;
        return vector;
    }
}
