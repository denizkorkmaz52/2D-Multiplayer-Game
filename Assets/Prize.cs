using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    PhotonView pw;
    // Start is called before the first frame update
    void Start()
    {
        pw = GetComponent<PhotonView>();
        StartCoroutine(DestroyPrize());
            
    }
    IEnumerator DestroyPrize()
    {
        yield return new WaitForSeconds(5f);
        if (pw.IsMine)
        {
            PhotonView.Destroy(gameObject);
        }

    }
}
