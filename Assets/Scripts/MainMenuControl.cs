using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuControl : MonoBehaviour
{
    public GameObject namePanel;
    public GameObject menuPanel;
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI[] statistics;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("HasPlayerName"))
        {
            namePanel.SetActive(true);

            PlayerPrefs.SetInt("Total_Game", 0);
            PlayerPrefs.SetInt("Win", 0);
            PlayerPrefs.SetInt("Lose", 0);           
            PlayerPrefs.SetInt("Point", 0);
            WriteStatistics();
        }
        else
        {
            menuPanel.SetActive(true);
            playerName.text = PlayerPrefs.GetString("PlayerName");
            WriteStatistics();
        }
    }
    
    public void SavePlayerName()
    {
        PlayerPrefs.SetInt("HasPlayerName", 1);
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        namePanel.SetActive(false);
        menuPanel.SetActive(true);
        playerName.text = PlayerPrefs.GetString("PlayerName");
    }

    void WriteStatistics()
    {
        statistics[0].text = PlayerPrefs.GetInt("Total_Game").ToString();
        statistics[1].text = PlayerPrefs.GetInt("Win").ToString();
        statistics[2].text = PlayerPrefs.GetInt("Lose").ToString();
        statistics[3].text = PlayerPrefs.GetInt("Point").ToString();
    }
}
