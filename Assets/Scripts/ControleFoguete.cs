using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControleFoguete : MonoBehaviour
{
    public static ControleFoguete Instance;

    public GameObject Rocket;
    public GameObject Satellite;
    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;
    public GameObject GenericObject;
    public GameObject ParticleStage1;
    public GameObject ParticleStage2;
    public GameObject ParticleStage3;
    public GameObject Clouds;
    public GameObject Coin;
    public Camera RocketCamera;
    public Text TextScore;
    public Text TextLife;
    public Text TextKM;
    
    private GameObject SatelliteObject;
    private GameObject CoinObject;
    
    bool trashIsOn = false, coinIsOn = false;
    bool rocketStart = false;
    float speedRhythm;
    float positionFirstStage = 47.2f;
    float positionSecondStage = 101.2f;
    float positionThirdStage = 10.5f;
    float limitsRight = 7.82f, limitsLeft = -20.04f;
    float rocketPositionX, rocketPositionY;
    int level = 0;
    int numBack = 3;
    int totalScore, currentLife = 3;
    float ConstantSpeed = 5f;
    float SpeedLimit = 10f;
    float startSpacePosition = 123f;

    // Start is called before the first frame update
    void Start()
    {
        speedRhythm = 5f;
        level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rocketPositionY = Rocket.transform.position.y;
        
        verifyStage();
        changeStage();

        level = 1;
        rocketStart = true;


        if (speedRhythm < SpeedLimit)
        {
            speedRhythm += ConstantSpeed;
        }

        TextKM.text = (speedRhythm * 100).ToString();
        
        if (rocketPositionY > 0 && rocketPositionY < Clouds.transform.position.y + 30){
            Clouds.transform.Translate(new Vector2((ConstantSpeed * Time.deltaTime) * -1, 0));
        }

        startRocket();

        if(rocketPositionY >= startSpacePosition){
            generateTrash();
            generateCoin();
        }
        
        if (rocketPositionY > positionThirdStage)
        {
            movements();
        }
    }

    void startRocket()
    {
        if(rocketStart == true)
        {
            Rocket.transform.Translate(new Vector2(0, speedRhythm * Time.deltaTime));
            RocketCamera.transform.Translate(new Vector2(0, speedRhythm * Time.deltaTime));
        }

    }

    void changeStage(){
        switch(level){
            case 0:
                ParticleStage1.SetActive(false);
                ParticleStage2.SetActive(false);
                ParticleStage3.SetActive(false);
            break;
            case 1:
                ParticleStage1.SetActive(true);
            break;
            case 2:
                ParticleStage1.SetActive(false);
                Stage1.transform.parent = GenericObject.transform;

                ParticleStage2.SetActive(true);
            break;
            case 3:
                ParticleStage2.SetActive(false);
                Stage2.transform.parent = GenericObject.transform;

                ParticleStage3.SetActive(true);
            break;
            default:
                Debug.Log("Error");
            break;
        }
    }

    void movements()
    {
        rocketPositionX = Rocket.transform.position.x;
        if (rocketPositionX <= limitsLeft)
        {
            Rocket.transform.position = new Vector2(limitsLeft, Rocket.transform.position.y);
        }
        if (rocketPositionX >= limitsRight)
        {
            Rocket.transform.position = new Vector2(limitsRight, Rocket.transform.position.y);
        }

        Rocket.transform.Translate(Vector2.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * 20);
    }

    void verifyStage()
    {
        if (rocketPositionY >= positionFirstStage && level == 1)
        {
            level = 2;
        }
        if (rocketPositionY >= positionSecondStage && level == 2)
        {
            level = 3;
        }
    }

    void generateTrash()
    {
        float becameInvisible = RocketCamera.transform.position.y - (RocketCamera.orthographicSize * 2);
        if (trashIsOn)
        {
            if(SatelliteObject.transform.position.y < becameInvisible)
            {
                Destroy(SatelliteObject);
                trashIsOn = false;
            }
        }
        if (!trashIsOn)
        {
            float randomX = Random.Range(-11.97f, 11.26f);
            Vector2 randomPosition = new Vector2(randomX, Rocket.transform.position.y + 22);
            SatelliteObject = Instantiate(Satellite, randomPosition, Quaternion.identity);
            trashIsOn = true;
        }
    }

    void generateCoin()
    {
        float becameInvisible = RocketCamera.transform.position.y - (RocketCamera.orthographicSize * 2);
        if (coinIsOn)
        {
            if (CoinObject.transform.position.y < becameInvisible)
            {
                Destroy(CoinObject);
                coinIsOn = false;
            }
        }
        if (!coinIsOn)
        {
            float randomX = Random.Range(-2.97f, 3.26f);
            Vector2 randomPosition = new Vector2(randomX, Rocket.transform.position.y + 20);
            CoinObject = Instantiate(Coin, randomPosition, Quaternion.identity);
            coinIsOn = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "loopSky" && other == other.GetComponent<EdgeCollider2D>())
        {
            
            float heightBack = other.GetComponent<BoxCollider2D>().size.y;
            Vector3 positionOther = other.GetComponent<BoxCollider2D>().transform.position;
            positionOther.y += ((heightBack * numBack) * 5);
            other.GetComponent<BoxCollider2D>().transform.position = positionOther;
           
        }

        if(other.transform.tag == "satellite")
        {
            Destroy(other);
            
            currentLife = currentLife - 1;
            TextLife.text = currentLife.ToString();
 
            other.gameObject.SetActive(false);
 
            if (currentLife <= 0)
            {
                currentLife = 3;
                SceneManager.LoadScene("GameOver");
            }
        }

        if (other.transform.tag == "coin")
        {
            Destroy(other);
            
            other.gameObject.SetActive(false);
            totalScore += 100;
            TextScore.text = totalScore.ToString();
        }
    }
}