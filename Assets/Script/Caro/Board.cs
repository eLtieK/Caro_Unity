using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;

public class Board : MonoBehaviour
{
    public Transform board;
    public GridLayoutGroup gridLayout;
    public GameObject cellPrefab;
    public int boardSize = 25;
    public Sprite xSprite;
    public Sprite oSprite;
    public Sprite noneSprite;
    public bool xTurn = true;
    public int[,] matrix;
    public int countrow = 0;
    public int countcol = 0;
    public int rcleft = 0;
    public int rcright = 0;
    public int countenemy = 0;
    public float timer = 0;
    public float timelimit = 0.5f;
    public bool isReset = false;
    public Logic logic;
    public Music music;
    public int row_min = 25;
    public int row_max = -1;
    public int col_min = 25;
    public int col_max = -1;
    void Awake()
    {
        countrow = 0;
        countcol = 0;
        rcleft = 0;
        rcright = 0;
        isReset = false;
        matrix = new int[boardSize,boardSize];
        gridLayout.constraintCount = boardSize;
        music = GameObject.FindGameObjectWithTag("Audio").GetComponent<Music>();
        if (SceneManager.GetActiveScene().name == "PlayScreen" || SceneManager.GetActiveScene().name == "BotScreen")
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
        }
        if (SceneManager.GetActiveScene().name == "StartScreen"){ CreateBoard(); }
        else
        {
            if (PlayerPrefs.GetInt("New") == 1) { CreateBoard(); }
            else if (PlayerPrefs.GetInt("New") == 0) { LoadBoard(); }
        }
    }
    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "StartScreen")
        {
            if(timer <= timelimit) { timer += Time.deltaTime; }
            else { RandomHit(); timer = 0; }
            if (FullBoard()) { isReset = true; Reset(); }
        }
    }
    public void CreateBoard()
    {
        for(int i = 0; i < boardSize; i++)
        {
            for(int j = 0; j < boardSize; j++)  
            {
                Cell cell = Instantiate(cellPrefab, board).GetComponent<Cell>();
                cell.row = i;
                cell.col = j;
                matrix[i, j] = 0;
            }
        }
    }
    public void LoadBoard()
    {
        int countX = 0, countO = 0;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (SceneManager.GetActiveScene().name == "PlayScreen")
                {
                    matrix[i, j] = PlayerPrefs.GetInt(i.ToString() + j.ToString());
                }
                else if (SceneManager.GetActiveScene().name == "BotScreen")
                {
                    matrix[i, j] = PlayerPrefs.GetInt(i.ToString() + j.ToString() + "bot");
                }
                Image cellimage = cellPrefab.GetComponent<Image>();
                if (matrix[i, j] == 1) { cellimage.sprite = xSprite; countX++; }
                else if (matrix[i, j] == 2) { cellimage.sprite = oSprite; countO++; }
                else { cellimage.sprite = noneSprite; }
                Cell cell = Instantiate(cellPrefab, board).GetComponent<Cell>();
                cell.row = i;
                cell.col = j;             
            }
        }
        if (countX > countO) { xTurn = false; }
        else { xTurn = true; }
    }
    public void SaveBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (SceneManager.GetActiveScene().name == "PlayScreen")
                {
                    PlayerPrefs.SetInt(i.ToString() + j.ToString(), matrix[i, j]);
                } 
                else if (SceneManager.GetActiveScene().name == "BotScreen")
                {
                    PlayerPrefs.SetInt(i.ToString() + j.ToString() + "bot", matrix[i, j]);
                }
            }
        }
    }
    public void Reset()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                matrix[i, j] = 0;
            }
        }
        xTurn = true;
    }
    public bool FullBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (matrix[i, j] != 1 || matrix[i, j] != 2) { return false; }
            }
        }
        return true;
    }
    public bool Check(int row, int col)
    {
        int turn = matrix[row, col];
        countrow = 1;
        countcol = 1;
        rcleft = 1;
        rcright = 1;
        int countenemy = 0;
        //check row
        for (int i = row - 1; i >= 0; i--)
        {
            if (matrix[i, col] == turn && turn != 0) { countrow++; }
            else if (matrix[i, col] != turn && matrix[i, col] != 0) { countenemy++;  break; }
            else { break; }
        }
        for (int i = row + 1; i < boardSize; i++)
        {
            if (matrix[i, col] == turn && turn != 0) { countrow++; }
            else if (matrix[i, col] != turn && matrix[i, col] != 0) { countenemy++;  break; }
            else { break; }
        }
        if (countrow >= 5 && countenemy != 2) { return true; }
        countenemy = 0;
        //check col
        for (int i = col - 1; i >= 0; i--)
        {
            if (matrix[row, i] == turn && turn != 0) { countcol++; }
            else if (matrix[row, i] != turn && matrix[row, i] != 0) { countenemy++;  break; }
            else { break; }
        }
        for (int i = col + 1; i < boardSize; i++)
        {
            if (matrix[row, i] == turn && turn != 0) { countcol++; }
            else if (matrix[row, i] != turn && matrix[row, i] != 0) { countenemy++;  break; }
            else { break; }
        }
        if (countcol >= 5 && countenemy != 2) { return true; }

        //check cheo trai
        int m = row - 1;
        int n = col - 1;
        countenemy = 0;
        while(m >= 0 && n >= 0)
        {
            if (matrix[m, n] == turn && turn != 0) { rcleft++; }
            else if (matrix[m, n] != turn && matrix[m, n] != 0) { countenemy++;  break; }
            else { break; }
            m--; n--;
        }
        m = row + 1;
        n = col + 1;
        while (m < boardSize && n < boardSize)
        {
            if (matrix[m, n] == turn && turn != 0) { rcleft++; }
            else if (matrix[m, n] != turn && matrix[m, n] != 0) { countenemy++;  break; }
            else { break; }
            m++; n++;
        }
        if (rcleft >= 5 && countenemy != 2) { return true; }
        //check cheo phai
        m = row - 1;
        n = col + 1;
        countenemy = 0;
        while (m >= 0 && n < boardSize)
        {
            if (matrix[m, n] == turn && turn != 0) { rcright++; }
            else if (matrix[m, n] != turn && matrix[m, n] != 0) { countenemy++;  break; }
            else { break; }
            m--; n++;
        }
        m = row + 1;
        n = col - 1;
        while (m < boardSize && n >= 0)
        {
            if (matrix[m, n] == turn && turn != 0) { rcright++; }
            else if (matrix[m, n] != turn && matrix[m, n] != 0) { countenemy++;  break; }
            else { break; }
            m++; n--;
        }
        if (rcright >= 5 && countenemy != 2) { return true; }
        return false;
    }
    public void whoWin(int row, int col)
    {
        if (matrix[row, col] == 1) { logic.xWin.SetActive(true); }
        else if (matrix[row, col] == 2) { logic.oWin.SetActive(true); }
        logic.gameOver = true;
        music.Over();
    }
    public void RandomHit()
    {
        int col, row;
        do
        {   
            col = Random.Range(0, 25);
            row = Random.Range(0, 25);
        } while (matrix[row, col] != 0);
        matrix[row, col] = -1;
    }
}
