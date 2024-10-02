using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Logic : MonoBehaviour
{
    public bool gameOver = false;
    public bool donePlay = false;
    public GameObject xWin;
    public GameObject oWin;
    public GameObject taskBox;
    public bool task = false;
    public float speed = 10f;
    public Selectable inputBox;
    public int xScore = 0;
    public int oScore = 0;
    public TextMeshProUGUI scoreXText;
    public TextMeshProUGUI scoreOText;
    public int xStreak = 0;
    public int oStreak = 0;
    public TextMeshProUGUI streakXText;
    public TextMeshProUGUI streakOText;
    public Button resetButton;
    public string str_winX;
    public string str_winO;
    public string str_streakX;
    public string str_streakO;
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "PlayScreen") 
        {
            str_winX = "WinX";
            str_winO = "WinO";
            str_streakX = "StreakX";
            str_streakO = "StreakO";
        }
        else if(SceneManager.GetActiveScene().name == "BotScreen")
        {
            str_winX = "WinXbot";
            str_winO = "WinObot";
            str_streakX = "StreakXbot";
            str_streakO = "StreakObot";
        }
        Load();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.U)) { inputBox.Select(); }
        if (xScore == 0 && oScore == 0) { resetButton.interactable = false; }
        else { resetButton.interactable = true; }
        if (!donePlay) 
        {
            if (xWin.activeSelf) { XWin(); donePlay = true; }
            else if (oWin.activeSelf) { OWin(); donePlay = true; }
        }
        if (!task)
        {
            if (taskBox.transform.position.x < 2220)
            {
                taskBox.transform.position += Vector3.right * Time.timeScale * speed;
            }
            if(taskBox.transform.position.x >= 2220)
            {
                taskBox.transform.position = new Vector3(2220, 680);
            }
        }
        else if(task)
        { 
            if (taskBox.transform.position.x >= 1660)
            {
                taskBox.transform.position += Vector3.left * Time.timeScale * speed;
            }
        }
    }
    public void Task()
    {
        if (task) { task = false; }
        else { task = true; }
    }
    public void AddWinX(int winToAdd)
    {
        xScore += winToAdd;
        scoreXText.text = xScore.ToString();
        PlayerPrefs.SetInt(str_winX, xScore);
    }
    public void AddStreakX(int streakToAdd)
    {
        xStreak += streakToAdd;
        streakXText.text = xStreak.ToString();
        PlayerPrefs.SetInt(str_streakX, xStreak);
    }
    public void AddWinO(int winToAdd)
    {
        oScore += winToAdd;
        scoreOText.text = oScore.ToString();
        PlayerPrefs.SetInt(str_winO, oScore);
    }
    public void AddStreakO(int streakToAdd)
    {
        oStreak += streakToAdd;
        streakOText.text = oStreak.ToString();
        PlayerPrefs.SetInt(str_streakO, oStreak);
    }
    public void Load()
    {
        xScore = PlayerPrefs.GetInt(str_winX);
        scoreXText.text = xScore.ToString();
        oScore = PlayerPrefs.GetInt(str_winO);
        scoreOText.text = oScore.ToString();

        xStreak = PlayerPrefs.GetInt(str_streakX);
        streakXText.text = xStreak.ToString();
        oStreak = PlayerPrefs.GetInt(str_streakO);
        streakOText.text = oStreak.ToString();
    }
    public void Reset()
    {
        PlayerPrefs.SetInt(str_winX, 0);
        PlayerPrefs.SetInt(str_winO, 0);
        PlayerPrefs.SetInt(str_streakX, 0);
        PlayerPrefs.SetInt(str_streakO, 0);
        Load();
    }
    public void XWin()
    {
        AddWinX(1);
        AddStreakX(1);
        PlayerPrefs.SetInt(str_streakO, 0);
        streakOText.text = 0.ToString();
    }
    public void OWin()
    {
        AddWinO(1);
        AddStreakO(1);
        PlayerPrefs.SetInt(str_streakX, 0);
        streakXText.text = 0.ToString();
    }
}
