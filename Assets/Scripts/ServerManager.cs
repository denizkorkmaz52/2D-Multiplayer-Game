using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class ServerManager : MonoBehaviourPunCallbacks
{
    bool isOtherLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Baðlandý");
        PhotonNetwork.JoinLobby();
    }
    public override void  OnJoinedLobby()
    {
        Debug.Log("Lobiye girdi");
    }

    public void RandomJoin()
    {
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.JoinRandomRoom();
    }
    public void CreateAndJoin()
    {
        PhotonNetwork.LoadLevel(1);
        int roomName = Random.Range(0, 10000);
        PhotonNetwork.JoinOrCreateRoom(roomName.ToString(), new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        InvokeRepeating(nameof(CheckInformation), 0, 1f);
        GameObject obj = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0, null);
        obj.GetComponent<PhotonView>().Owner.NickName = PlayerPrefs.GetString("PlayerName");
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            obj.tag = "Player 2";
            GameObject.FindWithTag("GameControl").gameObject.GetComponent<PhotonView>().RPC("CreatePrizeCaller", RpcTarget.All);
        }
    }

    public override void OnLeftRoom()
    {
        /*if (!isOtherLeft)
        {*/
            PlayerPrefs.SetInt("Total_Game", PlayerPrefs.GetInt("Total_Game") + 1);
            PlayerPrefs.SetInt("Lose", PlayerPrefs.GetInt("Lose") + 1);
        /*    isOtherLeft = true;
        }*/
        
    }

    public override void OnLeftLobby()
    {
     
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
        PlayerPrefs.SetInt("Total_Game", PlayerPrefs.GetInt("Total_Game") + 1);
        PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win") + 1);
        PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point") + 100);
        InvokeRepeating(nameof(CheckInformation), 0, 1f);
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
       
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }
    public void CheckInformation()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            CancelInvoke("CheckInformation");
            GameObject.FindWithTag("WaitingPlayer").SetActive(false);
            GameObject.FindWithTag("Player_1_Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Player_2_Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
        }
        else
        {
            //GameObject.FindWithTag("WaitingPlayer").SetActive(true);
            foreach (GameObject item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (item.gameObject.CompareTag("WaitingPlayer"))
                {
                    item.SetActive(true);
                }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject.FindWithTag("Player_1_Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
                GameObject.FindWithTag("Player_2_Name").GetComponent<TextMeshProUGUI>().text = "...";
            }
            else
            {
                GameObject.FindWithTag("Player_2_Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
                GameObject.FindWithTag("Player_1_Name").GetComponent<TextMeshProUGUI>().text = "...";
            }
            
        }
        
    }
}
