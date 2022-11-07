using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gamer : MonoBehaviour
{
    public GameObject extractPoint;
    bool isStarted = false;

    [Header("PowerBar OPs")]
    bool isPowerBarOn = true;
    GameObject gameControl;
    public static float force;
    float shootDirection;
    PhotonView pw;
    // Start is called before the first frame update
    void Start()
    {
        pw = GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            gameControl = GameObject.FindWithTag("GameControl");
            GetComponent<Gamer>().enabled = true;
            if (PhotonNetwork.IsMasterClient)
            {
                transform.SetPositionAndRotation(GameObject.FindWithTag("Spawn Point 1").transform.position, GameObject.FindWithTag("Spawn Point 1").transform.rotation);
                shootDirection = 2f;
            }
            else
            {
                transform.SetPositionAndRotation(GameObject.FindWithTag("Spawn Point 2").transform.position, GameObject.FindWithTag("Spawn Point 2").transform.rotation);
                shootDirection = -2f;
            }
            InvokeRepeating(nameof(IsStarted), 0, .5f);
        }
       
        
    }
    void IsStarted()
    {
        if (pw.IsMine)
        {
            if (PhotonNetwork.PlayerList.Length == 2)
            {
                isStarted = true;               
            }
            else
            {
                isStarted = false;
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {

        if (pw.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && isPowerBarOn && isStarted)
            {
                
                gameControl.GetComponent<PhotonView>().RPC("StartAndStopPowerBar", RpcTarget.All, 2);
                //Debug.Log(force);
                if (force != -1)
                {
                    PhotonNetwork.Instantiate("BallCreateEffect", extractPoint.transform.position, extractPoint.transform.rotation);
                    GameObject ball = PhotonNetwork.Instantiate("cannonball", extractPoint.transform.position, extractPoint.transform.rotation);
                    ball.GetComponent<PhotonView>().RPC("AddTag", RpcTarget.All, gameObject.tag);
                    Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                    rb.AddForce(new Vector2(shootDirection, 1f) * force, ForceMode2D.Impulse);

                    isPowerBarOn = false;
                }
                

            }
            else if (Input.GetKeyDown(KeyCode.R) && !isPowerBarOn && isStarted)
            {
                gameControl.GetComponent<PhotonView>().RPC("StartAndStopPowerBar", RpcTarget.All, 1);
                isPowerBarOn = true;
            }
        }
        else
        {
            isPowerBarOn = false;
        }
        
    }

    [PunRPC]
    public void Win()
    {
        if (pw.IsMine)
        {
            PlayerPrefs.SetInt("Total_Game", PlayerPrefs.GetInt("Total_Game") + 1);
            PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win") + 1);
            PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point") + 100);
        }
    }

    [PunRPC]
    public void Lose()
    {
        PlayerPrefs.SetInt("Total_Game", PlayerPrefs.GetInt("Total_Game") + 1);
        PlayerPrefs.SetInt("Lose", PlayerPrefs.GetInt("Lose") + 1);
    }
}
