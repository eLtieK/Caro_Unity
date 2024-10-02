
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int row;
    public int col;
    public Sprite xSprite;
    public Sprite oSprite;
    public Sprite noneSprite;
    private Image image;
    private Button button;
    private Board board;
    public float timer = 0;
    public float timelimit = 0.2f;
    public bool ishit = false;
    public Logic logic;
    public Music music;
    private int Find_Max(int a, int b)
    {
        if ((a >= 0 && a < 24 && b >= 0 && b < 24) || b == -1) 
        {
            if (a > b) { return a; }
            else { return b; }
        } else if ( a == 24 ) { return a; }
        else { return b; }
    }
    private int Find_Min(int a, int b)
    {
        if ((a > 0 && a <= 24 && b > 0 && b <= 24) || b == 25)
        {
            if (a < b) { return a; }
            else { return b; }
        } else if (a == 0) { return a; }
        else { return b; }
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "PlayScreen" || SceneManager.GetActiveScene().name == "BotScreen")
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
        }
        music = GameObject.FindGameObjectWithTag("Audio").GetComponent<Music>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScreen"){
            if(board.isReset == false) { AutoHit(); timer = 0; }
            else if (board.isReset == true)
            {
                image.sprite = noneSprite;
                ishit = false;
                if (image.sprite == noneSprite)
                {
                    board.Reset();
                    if (timer <= timelimit) { timer += Time.deltaTime; }
                    else {
                        board.isReset = false;
                        timer = 0; 
                        Debug.Log("Error");
                    }
                }
            }
        } else if(SceneManager.GetActiveScene().name == "PlayScreen" || SceneManager.GetActiveScene().name == "BotScreen") {
            if (board.Check(row, col))
            {
                image.color = new Color32(233, 218, 193, 255);
            }
            if (board.matrix[row,col] == 1) { image.sprite = xSprite; }
            else if (board.matrix[row, col] == 2) { image.sprite = oSprite; }
            else { image.sprite = noneSprite; }
        }
        /*if (SceneManager.GetActiveScene().name == "BotScreen" && board.matrix[row, col] != 0)
        {
            if (col == 0) { board.col_min = Find_Min(col, board.col_min); }
            else { board.col_min = Find_Min(col - 1, board.col_min); }
            if (row == 0) { board.row_min = Find_Min(row, board.row_min); }
            else { board.row_min = Find_Min(row - 1, board.row_min); }
            if (col == 24) { board.col_max = Find_Max(col, board.col_max); }
            else { board.col_max = Find_Max(col + 1, board.col_max); }
            if (row == 24) { board.row_max = Find_Max(row, board.row_max); }
            else { board.row_max = Find_Max(row + 1, board.row_max); }
        }*/
    }
    public void ChangeImage()
    {
        if (board.xTurn)
        {
            image.sprite = xSprite;
            board.matrix[row, col] = 1;
            board.xTurn = false;
        }
        else
        {
            image.sprite = oSprite;
            board.matrix[row, col] = 2;
            board.xTurn = true;
        }
    }
    private void OnClick()
    {
        if (SceneManager.GetActiveScene().name == "StartScreen")
        {
            if (!ishit)
            {
                ChangeImage();
                ishit = true;
                music.Click();
            }
            if (board.Check(row, col))
            {
                board.isReset = true;
                Debug.Log("Reset");
            }
        }
        else if (!logic.gameOver && board.matrix[row, col] == 0)
        {
            if (!ishit)
            {
                ChangeImage();
                ishit = true;
                music.Click();
            }
            /*if (SceneManager.GetActiveScene().name == "BotScreen")
            { 
                if(col == 0) { board.col_min = Find_Min(col , board.col_min); }
                else { board.col_min = Find_Min(col - 1, board.col_min); }
                if(row == 0) { board.row_min = Find_Min(row , board.row_min); }
                else { board.row_min = Find_Min(row - 1, board.row_min); }
                if(col == 24) { board.col_max = Find_Max(col , board.col_max); }
                else { board.col_max = Find_Max(col + 1, board.col_max); }
                if(row == 24) { board.row_max = Find_Max(row , board.row_max); }
                else { board.row_max = Find_Max(row + 1, board.row_max); }
            }*/
            if (board.Check(row, col)) { Debug.Log("Win"); board.whoWin(row, col); }
        }
    }
    public void AutoHit()
    {
        if (board.matrix[row, col] == -1)
        {
            if (!ishit)
            {
                ChangeImage();
                ishit = true;
            }
            if (board.Check(row, col)) {
                board.isReset = true; 
                Debug.Log("Reset"); 
            }
        }
    }
}
