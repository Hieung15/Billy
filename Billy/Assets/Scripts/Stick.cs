using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Stick : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject stick_parent;
    public GameObject parent;

    public bool isMouseClicked;
    public bool isDrag;
    public bool isMouseButtonUp;
    

    

    void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            parent.transform.localPosition = parent.transform.localPosition + new Vector3(0.1f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            parent.transform.localPosition = parent.transform.localPosition + new Vector3(-0.1f, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            parent.transform.localEulerAngles = parent.transform.localEulerAngles + new Vector3(0, 0, 1f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            parent.transform.localEulerAngles = parent.transform.localEulerAngles + new Vector3(0, 0, -1f);
        }

        if(transform.rotation.eulerAngles.y < 300 && transform.rotation.eulerAngles.y > 180)
        {
            parent.transform.localEulerAngles = new Vector3(parent.transform.localEulerAngles.x, 300,0);
        }   
        else if(transform.rotation.eulerAngles.y > 60 && transform.rotation.eulerAngles.y < 180)
            parent.transform.localEulerAngles = new Vector3(parent.transform.localEulerAngles.x, 60, 0);

    }


}
