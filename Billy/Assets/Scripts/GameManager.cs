using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/****************************************/
// 2. nach Level 6 hat problem mit Material
// 3. kein Warnungsmessage 
// 4. power text ----> Done
// 5. gro?er Ball sinkt manchmal (1.warum der ball 1. sch?ttert immer nach dem schlagen???)
// 6. wenn der Ball immer noch innerhalb der Grenze ist.
// 7. wenn der Ball ausserhalb des Grounds ist dann Game over
/****************************************/

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Stick stick;

    public Ball lastBall;
    public GameObject ballPrefab;
    public GameObject particlePrefab;
    public GameObject stickParent;
    public GameObject border;
    public Transform ballGroup;
    public Transform particleGroup;
    public Slider slider;
    public TextMeshProUGUI scoreText;
    public GameObject Gameover;
    public float power;
    public float maxPower;
    public int maxLevel;
    public int score;
    public bool isOver;
    public bool hasPower;
    public bool isHit;

    GameObject ballAsGameObject;
    Ball newBall;
    Collider collBorder;

    float holdStartTime;
    bool canHit;


    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        collBorder = border.GetComponent<Collider>();
        NextBall();
        power = 0.1f;
        maxPower = 100f;
        canHit = true;
        
        
    }

    void Update()
    {
        Getpower();
        HitBall();
        blockWall();
        scoreText.text = "SCORE: " + score.ToString();


    }

    // to create a Ball as Instance game object and the particle effect.
    Ball GetBall()
    {
        ballAsGameObject = Instantiate(ballPrefab, ballGroup);
        newBall = ballAsGameObject.GetComponent<Ball>();

        GameObject particleInstant = Instantiate(particlePrefab, particleGroup);
        ParticleSystem newParticle = particleInstant.GetComponent<ParticleSystem >();

        newBall.particle = newParticle;
        

        return newBall;
    }

    void NextBall()
    {

        if (isOver)
        {
            return;
        }

        // to set the instance ball as last ball which is loaded.
        Ball newBall = GetBall();
        lastBall = newBall;
        lastBall.manager = this;
        lastBall.stick = stick;
        //lastBall.warning = warning;

        if (maxLevel < 6)
        {
            lastBall.level = Random.Range(1, maxLevel + 1);
        }
        else
        {
            lastBall.level = Random.Range(1, 6);
        }
        
        lastBall.gameObject.SetActive(true);

        StartCoroutine(WaitNextBall());

        

    }


    IEnumerator WaitNextBall()
    {
        
        while (lastBall != null)
        {
            
            yield return null;
        }

        // if the player didn't hit the ball
        while (!newBall.isCollided)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);

        canHit = true;
        NextBall();



    }


    public void GameOver()
    {
        if (isOver)
            return;

        isOver = true;

        StartCoroutine(GameOverRoutine());

        Gameover.SetActive(true);
    }

   


    IEnumerator GameOverRoutine()
    {
        Ball[] balls = GameObject.FindObjectsOfType<Ball>();

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].rb.useGravity = false;

        }

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void blockWall()
    {
        if (newBall.isPassed && !isHit)
        {
            collBorder.isTrigger = false;
            
        }
    }



    public void Getpower()
    {

        if (Input.GetMouseButtonDown(0) && canHit)
        {
            holdStartTime = Time.time;
        }
        if (Input.GetMouseButton(0) && canHit)
        {
            float holdTime = (Time.time - holdStartTime)*10;
            float holdTimeToPower = Mathf.Clamp(holdTime, 0,100);
            power = holdTimeToPower*20;
            //if(power < 10)
            //{
            //    power = 10;
            //}
            slider.value = power;
        }

        if (Input.GetMouseButtonUp(0) && canHit)
        {
            HitBall();
            lastBall = null;
            isHit = true;
            collBorder.isTrigger = true;
            StartCoroutine(powerZero());
            

        }
        else
        {
            isHit = false;
        }

        
    }

    IEnumerator powerZero()
    {
        
        yield return new WaitForSeconds(1.5f);
        slider.value = 0;

    }

    public void HitBall()
    {
        if (isHit)
        {
            StartCoroutine(HitPositionAnimation(new Vector3(0, -2, 0), new Vector3(0, 5, 0), new Vector3(0, -2, 0)));
            
        }

        // to prevent to use Stick when the ball moves
        if (newBall.isCollided)
            canHit = false;

    }


    IEnumerator HitPositionAnimation(Vector3 goal1, Vector3 goal2, Vector3 goal3)
    {
        int count = 0;
       
        Vector3 tmp1 = stick.transform.localPosition + goal1;

        float distance = (stick.transform.position - newBall.transform.position).magnitude;
        Vector3 direction = (stick.transform.position - newBall.transform.position) / distance;

        Vector3 moveVec = direction * distance;



        while (count < 20)
        {
            count++;
            stick.transform.localPosition = Vector3.Lerp(stick.transform.localPosition, tmp1, 0.2f);

            yield return null;
        }

        moveVec.y = moveVec.y + (stick.transform.localScale.y);
        moveVec.z = newBall.transform.localPosition.z - newBall.transform.localScale.z / 2;
        //moveVec.y = distance - (stick.transform.localScale.y/2);
        //moveVec.z = newBall.transform.localPosition.z;
        //moveVec.z = newBall.transform.localPosition.z - newBall.transform.localScale.z/2;
        moveVec.x = 0;
    
        count = 0;
        while (count < 20)
        {
            count++;
            stick.transform.localPosition = Vector3.Lerp(stick.transform.localPosition, moveVec, 0.2f);
            Debug.DrawLine(stick.transform.localPosition, moveVec, Color.green, 10);
            yield return null;
        }
        Vector3 tmp3 = transform.localPosition + goal3;
        tmp3.z = 0;
        count = 0;
        while (count < 20)
        {
            count++;

            stick.transform.localPosition = Vector3.Lerp(stick.transform.localPosition, tmp3, 0.2f);
            yield return null;
        }

        yield return null;
        
        //StartCoroutine(ReturnZero());


    }


    // has problem...
    IEnumerator ReturnZero()
    {
        int count = 0;
        while (count < 20)
        {
            count++;

            stickParent.transform.localPosition = Vector3.Lerp(stickParent.transform.localPosition, new Vector3(0, 0.5f, -25f), 0.2f);
            stickParent.transform.localEulerAngles = Vector3.Lerp(stickParent.transform.eulerAngles, new Vector3(90, 0, 0), 0.2f);
            yield return null;
        }
    }




}

