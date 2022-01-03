using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Ball : MonoBehaviour
{
    
    public Stick stick;
    public GameManager manager;
    public Rigidbody rb;
    public ParticleSystem particle;
    public Vector3 reflectedBall;
    public Vector3 velocity;
    //public TextMeshProUGUI warning;
    public int level;
    public bool isDrag;
    public bool isPassed;
    public bool isCollided;
    
   
    
   
    Animator anim;
    Material material;
    Collider coll;
    Vector3 startPos;
    Vector3 stickPos;
    

    bool isMerge;
    bool canLevelUp;
    float deadTime;
    float holdStartTime;
    float power;
    float tmp;
    float stayTime;
    public int ballLevel;
    




    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();    
        anim = this.GetComponent<Animator>();
        coll = this.GetComponent<Collider>();
        material = this.GetComponent<MeshRenderer>().material;

        
        velocity = rb.velocity;

    }

     void OnEnable()
    {
        anim.SetInteger("Level", level);
        startPos = transform.position;
        ballLevel = level;
        SetTextrue();
        SetMass();

    }

    void FixedUpdate()
    {
        if (manager.isHit)
        {
            power = manager.power;

        }

        //isInside();

        //if (transform.position.z < manager.border.transform.position.z && isCollided && )
        //{
        //    canLevelUp = false;
        //    material.color = Color.red;
        //    Invoke("WaitToMove", 2.5f);
        //}
        //else
        //    canLevelUp = true;

    }

    void WaitToMove()
    {
        manager.GameOver();
    }

    void isInside()
    {
        stayTime += Time.deltaTime;
        if (transform.position.z < manager.border.transform.position.z+0.1f && isCollided)
        {
            isMerge = false;

            if (stayTime > 1)
            {
                Debug.Log("Ball//   inside");
                material.color = Color.red;
                Invoke("WaitToMove", 0.5f);
                manager.GameOver();
               
            }

        }

        //if (transform.position.z < manager.border.transform.position.z && isCollided)
        //{
        //    canLevelUp = false;
        //    material.color = Color.red;
        //    Invoke("WaitToMove", 2.5f);
        //}
        //else
        //    canLevelUp = true;

        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Ball otherBall = collision.gameObject.GetComponent<Ball>();

            if(ballLevel == otherBall.ballLevel && !isMerge && !otherBall.isMerge && level<10)
            {
                // 2 Dimension
                float myX = transform.position.x;
                float myY = transform.position.y;

                float otherX = otherBall.transform.position.x;
                float otherY = otherBall.transform.position.y;

                if(myY < otherY || (myX > otherX && myY == otherY))
                {
                    otherBall.Hide(transform.position);
                    LevelUP();
                } 
            }
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Ground")
    //    {
    //        Vector3 tmp = transform.localPosition;
    //        tmp.y = -1 - transform.localScale.y / 2;
    //        transform.localPosition = tmp;
    //    }
    //}

    public void Hide(Vector3 targetPos)
    {
        isMerge = true;
        coll.enabled = false;

        if (targetPos == Vector3.up * 100)
            ParticlePlay();

        StartCoroutine(HideMotion(targetPos));
    }

    IEnumerator HideMotion(Vector3 targetPos)
    {
        int count = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector2.zero;
        while (count < 20)
        {
            count++;

            if(targetPos != Vector3.up * 100)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
                
            }
            else if(targetPos == Vector3.up * 100)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }
            
            yield return null;
        }

        manager.score += (int) Mathf.Pow(2, level);

        isMerge = false;
        gameObject.SetActive(false);
       
        
    }

    void LevelUP()
    {
        isMerge = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector2.zero;
        

        StartCoroutine(LevelUpMotion());
    }

    IEnumerator LevelUpMotion()
    {

        manager.maxLevel = Mathf.Max(level, manager.maxLevel);

        if (manager.maxLevel > 6)
        {
            level = 5;
            ballLevel++;
        }
        else
        {
            level++;
            ballLevel++;
        }

        SetTextrue();
        SetMass();
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("Level", level);
        ParticlePlay();
        yield return new WaitForSeconds(0.3f);
        

        /***************************************/
        

        isMerge = false;
    }

    void ParticlePlay()
    {
        particle.transform.position = transform.position;
        particle.transform.localScale = transform.localScale;
        particle.Play();
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Finish")
        {
            deadTime += Time.deltaTime;

            if(deadTime > 5)
            {
                material.color = Color.red;
                //warning.enabled = true;
                
            }
            if(deadTime > 10)
            {
                manager.GameOver();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Finish")
        {
            deadTime = 0;
            isPassed = true;
            material.color = Color.white;
            //Debug.Log("Ball//   Enter");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish" && manager.isHit)
        {
            isPassed = true;
            Debug.Log("Ball//   Enter");
        }

            
    }





    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPos = collision.contacts[0].point;
        

        if (collision.collider.tag == "Wall")
        {
            Vector3 incomingVec = hitPos - startPos;
            Vector3 reflectVec = Vector3.Reflect(incomingVec, collision.contacts[0].normal);
            reflectVec = reflectVec.normalized ;
            Debug.DrawLine(hitPos, reflectVec, Color.red, 5, false);
            Debug.DrawLine(startPos, hitPos, Color.blue, 5, false);
            //velocity.z = 0;
            startPos = transform.position;

            power = Mathf.Pow(power, 0.8f);

            Vector3 newVelocity = -collision.relativeVelocity; 


            float currentPower = rb.velocity.magnitude;
            rb.velocity = reflectVec * currentPower ;
            //rb.velocity = velocity*power;

        }

        if(collision.collider.tag == "Stick")
        {
            isCollided = true;
            stickPos = collision.transform.position;
            Vector3 incomingVec = hitPos - stickPos;
            velocity = incomingVec.normalized * manager.power;
            velocity.y = 0;
            rb.velocity = velocity;

            //rb.AddForce(velocity);

        }
        
    }

    private void SetTextrue()
    {
        if (ballLevel == 1 || level == 1)
        {
            material.mainTexture = Resources.Load("Textures/1") as Texture;
        }
        else if(ballLevel == 2 || level == 2)
            material.mainTexture = Resources.Load("Textures/2") as Texture;
        else if (ballLevel == 3 || level == 3)
            material.mainTexture = Resources.Load("Textures/3") as Texture;
        else if (ballLevel == 4 || level == 4)
            material.mainTexture = Resources.Load("Textures/4") as Texture;
        else if (ballLevel == 5 || level == 5)
            material.mainTexture = Resources.Load("Textures/5") as Texture;
        else if(ballLevel == 6 && level == 5)
            material.mainTexture = Resources.Load("Textures/6") as Texture;
        else if (ballLevel == 7 && level == 5)
            material.mainTexture = Resources.Load("Textures/7") as Texture;
        else if (ballLevel == 8 && level == 5)
            material.mainTexture = Resources.Load("Textures/8") as Texture;
        else if (ballLevel == 9 && level == 5)
            material.mainTexture = Resources.Load("Textures/9") as Texture;
        else if (ballLevel == 10 && level == 5)
            material.mainTexture = Resources.Load("Textures/10") as Texture;

    }

    private void SetMass()
    {
        rb.mass = level*1.5f;
    }

}
