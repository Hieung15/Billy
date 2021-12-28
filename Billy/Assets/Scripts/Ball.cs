using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ball : MonoBehaviour
{
    
    public Stick stick;
    public GameManager manager;
    public ParticleSystem particle;
    public bool isDrag;
    public int level;
    public bool isPassed;
    public bool isCollided;

    public Rigidbody rb;
    public Vector3 reflectedBall;
    public Vector3 velocity;
   
    Animator anim;
    Material material;
    Collider coll;
    Vector3 startPos;
    Vector3 stickPos;
    

    bool isMerge;
    float deadTime;
    float holdStartTime;
    float power;
    float tmp;
    


    
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();    
        anim = this.GetComponent<Animator>();
        coll = this.GetComponent<Collider>();
        //material = GetComponent<Material>();
        material = this.GetComponent<MeshRenderer>().material;

        
        velocity = rb.velocity;

    }

     void OnEnable()
    {
        anim.SetInteger("Level", level);
        startPos = transform.position;

        SetTextrue();

        

    }

    void FixedUpdate()
    {
        if (manager.isHit)
        {
            power = manager.power;

        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Ball otherBall = collision.gameObject.GetComponent<Ball>();

            if(level == otherBall.level && !isMerge && !otherBall.isMerge && level<10)
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
        level++;
        SetTextrue();
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("Level", level);
        ParticlePlay();
        yield return new WaitForSeconds(0.3f);
        

        /***************************************/
        manager.maxLevel = Mathf.Max(level, manager.maxLevel);

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

            if(deadTime > 2)
            {
                material.color = Color.red;
            }
            if(deadTime > 5)
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
            velocity = reflectVec.normalized ;
            Debug.DrawLine(hitPos, reflectVec, Color.red, 5, false);
            Debug.DrawLine(startPos, hitPos, Color.blue, 5, false);
            //velocity.z = 0;
            //Debug.Log("Ball:    " + velocity);
            startPos = transform.position;
            //Debug.Log("Ball//   power before: " + power);
            power = power/2;
            //Debug.Log("Ball//   power after: " + power);
            //rb.AddForce(velocity * power);
            rb.velocity = velocity*power;

            //Debug.Log("Ball//       velocity " + velocity);
        }

        if(collision.collider.tag == "Stick")
        {
            isCollided = true;
            stickPos = collision.transform.position;
            Vector3 incomingVec = hitPos - stickPos;
            velocity = incomingVec.normalized * manager.power;
            rb.velocity = velocity;

            

            //rb.AddForce(velocity);

        }
        
    }

    private void SetTextrue()
    {
        if (level == 1)
        {
            material.mainTexture = Resources.Load("Textures/1") as Texture;
        }
        else if(level == 2)
            material.mainTexture = Resources.Load("Textures/2") as Texture;
        else if (level == 3)
            material.mainTexture = Resources.Load("Textures/3") as Texture;
        else if (level == 4)
            material.mainTexture = Resources.Load("Textures/4") as Texture;
        else if (level == 5)
            material.mainTexture = Resources.Load("Textures/5") as Texture;

    }

}
