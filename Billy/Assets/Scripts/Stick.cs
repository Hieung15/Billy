using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Stick : MonoBehaviour
{

    public bool isMouseButtonUp;
    public GameObject stick_parent;
    public GameObject parent;

    public bool isMouseClicked;
    public bool isDrag;
    public GameManager gameManager;

    Vector3 mousePos;
    Animator anim;
    

    void Awake()
    {
        //anim = parent.GetComponent<Animator>();
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

        //isHIt();
    }



    //public void isHIt()
    //{
    //    if (gameManager.isHit)
    //    {
    //        StartCoroutine(HitPositionAnimation(new Vector3(0,-2,0), new Vector3(0, 5, 0), new Vector3(0,-2,0)));
    //    }
    //}


    //IEnumerator HitPositionAnimation(Vector3 goal1, Vector3 goal2, Vector3 goal3 )
    //{
    //    int count = 0;
    //    //gameManager.lastBall.transform.position;
    //    Vector3 tmp1 = transform.localPosition + goal1;

    //    while (count < 20)
    //    {
    //        count++;
    //        transform.localPosition = Vector3.Lerp(transform.localPosition, tmp1, 0.2f);

    //        yield return null;
    //    }
        
    //    Vector3 tmp2 = transform.localPosition + goal2;
    //    count = 0;
    //    while (count < 20)
    //    {
    //        count++;
    //        transform.localPosition = Vector3.Lerp(transform.localPosition, tmp2, 0.2f);
    //        yield return null;
    //    }
    //    Vector3 tmp3 = transform.localPosition + goal3;
    //    count = 0;
    //    while (count < 20)
    //    {
    //        count++;

    //        transform.localPosition = Vector3.Lerp(transform.localPosition, tmp3, 0.2f);
    //        yield return null;
    //    }

    //    StartCoroutine(ReturnZero());
    //}

    //IEnumerator ReturnZero()
    //{
    //    int count = 0;
    //    while (count < 20)
    //    {
    //        count++;

    //        parent.transform.localPosition = Vector3.Lerp(parent.transform.localPosition, new Vector3(0, 0.5f, -25f), 0.2f);
    //        parent.transform.localEulerAngles = Vector3.Lerp(parent.transform.eulerAngles, new Vector3(90, 0, 0), 0.2f);
    //        yield return null;
    //    }
    //}

}
