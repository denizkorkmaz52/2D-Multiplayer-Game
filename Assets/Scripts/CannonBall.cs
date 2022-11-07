using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CannonBall : MonoBehaviour
{
    readonly float damage = 20;
    //public string ownerTag;
    GameObject owner;
    int ownerIndex;
    //public ParticleSystem ballDestroyEffect;
    GameObject gameControl;
    PhotonView pw;
    // Start is called before the first frame update
    void Start()
    {
        pw = GetComponent<PhotonView>();
        StartCoroutine(DestroyBall());
        gameControl = GameObject.FindWithTag("GameControl");
    }

    [PunRPC]
    public void AddTag(string tag)
    {
        owner = GameObject.FindWithTag(tag);
        if (tag == "Player")
        {
            ownerIndex = 1;
        }
        else
        {
            ownerIndex = 2;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Blocks"))
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            gameControl.GetComponent<GameControl>().PlayEffects(1, transform.gameObject);
            if (pw.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
        else if (collision.gameObject.CompareTag("Player_1_Tower") && !owner.CompareTag("Player"))
        {
            gameControl.GetComponent<PhotonView>().RPC("TowerTakeDamage", RpcTarget.All, 1);
            gameControl.GetComponent<GameControl>().PlayEffects(1, transform.gameObject);
            if (pw.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Player_2_Tower") && !owner.CompareTag("Player 2"))
        {
            gameControl.GetComponent<PhotonView>().RPC("TowerTakeDamage", RpcTarget.All, 2);
            gameControl.GetComponent<GameControl>().PlayEffects(1, transform.gameObject);
            if (pw.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Moving Bar"))
        {
            gameControl.GetComponent<GameControl>().PlayEffects(1, transform.gameObject);
            if (pw.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Prize"))
        {
            gameControl.gameObject.GetComponent<PhotonView>().RPC("TakePrize", RpcTarget.All, ownerIndex);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            gameControl.GetComponent<GameControl>().PlayEffects(1, transform.gameObject);
            if (pw.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
    IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(3f);
        gameControl.GetComponent<GameControl>().PlayEffects(1, transform.gameObject);
        if (pw.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
