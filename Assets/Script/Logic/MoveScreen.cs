using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveScreen : MonoBehaviour
{
    private Board board;
    public Logic logic;
    public string screen_mode;
    public GameObject mode_ui;
    public GameObject menu_ui;
    public GameObject difficulty_ui;
    public GameObject mode_Container;
    public Button easy_ui;
    public Button hard_ui;
    private void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        if (SceneManager.GetActiveScene().name == "PlayScreen" || SceneManager.GetActiveScene().name == "BotScreen")
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
        }
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "StartScreen")
        {
            if(PlayerPrefs.GetInt("IsHard") == 1)
            {
                hard_ui.interactable = false;
                easy_ui.interactable = true;
            } else
            {
                hard_ui.interactable = true;
                easy_ui.interactable = false;
            }
        }
    }
    public void Again() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); board.Reset(); board.SaveBoard(); }
   public void NewGame() {PlayerPrefs.SetInt("New",1); SceneManager.LoadScene(screen_mode); }
   public void Continue() { PlayerPrefs.SetInt("New", 0); SceneManager.LoadScene(screen_mode); }
   public void MainMenu() 
    {
        if (!logic.gameOver) { board.SaveBoard(); }
        else { board.Reset(); board.SaveBoard(); }
        SceneManager.LoadScene("StartScreen"); 
    }
   public void ExitGame() { Application.Quit();}
   public void Bot() { screen_mode = "BotScreen"; HideUI(); Difficulty(); }
   public void Player() { screen_mode = "PlayScreen"; HideUI(); }
   public void HideUI() { mode_ui.SetActive(false); menu_ui.SetActive(true); }
   public void Back_ShowUI() { menu_ui.SetActive(false); mode_ui.SetActive(true); difficulty_ui.SetActive(false); }
   public void Difficulty() { difficulty_ui.SetActive(true); mode_Container.SetActive(false); }
   public void Mode_On() { mode_Container.SetActive(true); }
   public void Mode_Off() { mode_Container.SetActive(false); }
   public void Hard() { PlayerPrefs.SetInt("IsHard", 1); }
   public void Easy() { PlayerPrefs.SetInt("IsHard", 0); }
}
