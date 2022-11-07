using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    [Header("CANNONBALLS OPs")]
    public ParticleSystem ballEffect;
    public AudioSource ballSound;
    public float damage;

    [Header("NEUTRAL BOXES OPs")]
    public ParticleSystem neutralBoxEffect;
    public AudioSource neutralBoxSound;

    [Header("PLAYER HEALTH OPs")]
    public Image player_1_healthBar;
    float player_1_health = 100;
    public Image player_2_healthBar;
    float player_2_health = 100;

    [Header("POWERBAR SETTINGS")]
    Image powerBar;
    bool isEnd = false;
    Coroutine PowerBarCoroutine;
    PhotonView pw;
    public static string playerTag;
    bool isPowerbarStarted = false;
    // Start is called before the first frame update
    [Header("PRIZE OPs")]
    float waitTime;
    int limit;
    bool isStarted;
    int createdPrizes;
    public GameObject[] points;

    GameObject player1;
    GameObject player2;
    void Start()
    {
        waitTime = 5f;
        limit = 5;
        isStarted = false;
        createdPrizes = 0;

        player1 = GameObject.FindWithTag("Player");
        player2 = GameObject.FindWithTag("Player 2");
        pw = gameObject.GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            Debug.Log(pw.name);
            powerBar = GameObject.FindWithTag("Powerbar").GetComponent<Image>();
        }
        InvokeRepeating(nameof(FirstStart), 0, .1f);
    }
    IEnumerator CreatePrize()
    {   
        while (isStarted)
        {
            if (limit == createdPrizes)
            {
                isStarted = false;
            }
            yield return new WaitForSeconds(waitTime);
            int createPoint = Random.Range(0, 4);
            PhotonNetwork.Instantiate("Prize", points[createPoint].transform.position, points[createPoint].transform.rotation);
            createdPrizes++;
            Debug.Log(" count " + createdPrizes);
            Debug.Log(" point " + createPoint);
        }
    }
    [PunRPC]
    public void CreatePrizeCaller()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isStarted = true;
            StartCoroutine(CreatePrize());
        }    
    }
    void FirstStart()
    {
        StartAndStopPowerBar(1);
    }
    [PunRPC]
    public void StartAndStopPowerBar(int value)
    {
        if (PhotonNetwork.PlayerList.Length == 2 && value == 1)
        {
            PowerBarCoroutine = StartCoroutine(PowerBar());
            if (!isPowerbarStarted)
            {
                isPowerbarStarted = true;
                CancelInvoke(nameof(FirstStart));
            }
            
        }
        else if (value == 2)
        {
            StopCoroutine(PowerBarCoroutine);
            Gamer.force = powerBar.fillAmount * 41f;
        }
        else if (PhotonNetwork.PlayerList.Length == 1)
        {
            isPowerbarStarted = false;
            InvokeRepeating(nameof(FirstStart), 0, .1f);
        }
            

        
    }

    IEnumerator PowerBar()
    {

        powerBar.fillAmount = 0;
        isEnd = false;
        while (true)
        {
            if (powerBar.fillAmount < 1 && !isEnd)
            {
                powerBar.fillAmount += 0.01f;
                yield return new WaitForSeconds(0.1f * Time.deltaTime);
            }
            else
            {
                isEnd = true;
                powerBar.fillAmount -= 0.01f;
                yield return new WaitForSeconds(0.1f * Time.deltaTime);

                if (powerBar.fillAmount == 0)
                {
                    isEnd = false;
                }
                
            }
        }
    }

    [PunRPC]
    public void TowerTakeDamage(int player)
    {
        if (player == 1)
        {
            
            player_1_health -= damage;
            Debug.Log("player 1 hasar aldý " + player_1_health);
            player_1_healthBar.fillAmount = player_1_health / 100;
            if (player_1_health <= 0)
            {
                player2.GetComponent<PhotonView>().RPC("Win", RpcTarget.All);
                player1.GetComponent<PhotonView>().RPC("Lose", RpcTarget.All);
            }
        }
        else if (player == 2)
        {
            player_2_health -= damage;
            Debug.Log("player 2 hasar aldý " + player_2_health);
            player_2_healthBar.fillAmount = player_2_health / 100;
            player1.GetComponent<PhotonView>().RPC("Win", RpcTarget.All);
            player2.GetComponent<PhotonView>().RPC("Lose", RpcTarget.All);
        }
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    [PunRPC]
    public void TakePrize(int playerIndex)
    {
        if (playerIndex == 1)
        {
            player_1_health += 30;
            if (player_1_health > 100)
                player_1_health = 100;
            player_1_healthBar.fillAmount = player_1_health / 100;

        }
        else if (playerIndex == 2)
        {
            player_2_health += 30;
            if (player_2_health > 100)
                player_2_health = 100;
            player_2_healthBar.fillAmount = player_2_health / 100;

        }
    }
    public void PlayEffects(int effectCase, GameObject effectObject)
    {
        if (effectCase == 1)
        {
            PhotonNetwork.Instantiate("BallEffect", effectObject.transform.position, effectObject.transform.rotation);
            ballSound.Play();
        }
        else if (effectCase == 2)
        {
            PhotonNetwork.Instantiate("NeutralBoxEffect", effectObject.transform.position, effectObject.transform.rotation);
            neutralBoxSound.Play();
        }
    }

}
