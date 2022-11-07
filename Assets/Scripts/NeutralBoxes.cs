using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class NeutralBoxes : MonoBehaviour
{
    float health = 100;
    public GameObject healthCanvas;
    public Image healthBar;
    public GameObject gameControl;
    PhotonView pw;
    //public ParticleSystem boxDestroyEffect;
    // Start is called before the first frame update
    void Start()
    {
        pw = GetComponent<PhotonView>();
        gameControl = GameObject.FindWithTag("GameControl");
    }


    [PunRPC]
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / 100;
        //healthCanvas.SetActive(true);
        if (health <= 0)
        {
            gameControl.GetComponent<GameControl>().PlayEffects(2, transform.gameObject);
            if (pw.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            StartCoroutine(ShowHealthCanvas());
        }
    }

    IEnumerator ShowHealthCanvas()
    {
        if (!healthCanvas.activeInHierarchy)
        {
            healthCanvas.SetActive(true);
            yield return new WaitForSeconds(2);
            healthCanvas.SetActive(false);
        }
    }
}
