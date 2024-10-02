using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GridBrushBase;

public class InputFieldGrabber : MonoBehaviour
{
    public string inputText;
    public int temp;
    public Logic logic;
    public Board board;
    public Music music;
    private void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
        music = GameObject.FindGameObjectWithTag("Audio").GetComponent<Music>();
    }
    public void GrabFromInputField(string s) { inputText = s;}

    public bool is_have_letter(char c)
    {
        if (c >= 'a' && c <= 'y') { return true; }
        else { return false; }
    }
    public bool is_int(char c)
    {
        if (c >= '0' && c <= '9') { return true; }
        else { return false; }
    }
    public bool makeMove(string playerMove)
    {
        if (logic.gameOver) { return false; }
        int size_c = playerMove.Length;
        char letter = playerMove[size_c - 1];
        char fletter = playerMove[0];
        int change_to_int;
        if (size_c > 3 || size_c < 2) { return false; }
        if (is_have_letter(fletter))
        {
            if (size_c == 2 && is_int(playerMove[1]))
            {
                string subs = playerMove.Substring(1, 1);
                temp = int.Parse(subs);
            }
            else if (size_c == 3 && is_int(playerMove[1]) && is_int(playerMove[2]))
            {
                string subs = playerMove.Substring(1, 2);
                temp = int.Parse(subs);
            }
            change_to_int = fletter - 'a' + 1;
        }
        else if(is_have_letter(letter))
        {
            if (size_c == 2 && is_int(playerMove[0]))
            {
                string subs = playerMove.Substring(0, 1);
                temp = int.Parse(subs);
            }
            else if (size_c == 3 && is_int(playerMove[0]) && is_int(playerMove[1]))
            {
                string subs = playerMove.Substring(0, 2);
                temp = int.Parse(subs);
            }
            change_to_int = letter - 'a' + 1;
        }
        else { return false; }
        
        if (temp > 25 || temp < 1) { return false; }
        if (!is_have_letter(letter) && !is_have_letter(fletter)) { return false; }
        if (board.matrix[temp - 1, change_to_int - 1] == 0)
        {
            if (board.xTurn)
            {
                board.matrix[temp - 1, change_to_int - 1] = 1;
                board.xTurn = false;
            }
            else { board.matrix[temp - 1, change_to_int - 1] = 2; board.xTurn = true; }
            if (board.Check(temp - 1, change_to_int - 1)) { board.whoWin(temp - 1, change_to_int - 1); }
            return true;
        }
        return false;
    }
    private void Update()
    {
        if (inputText.Length > 0) { 
            inputText.ToLower();
            if (makeMove(inputText)) { Debug.Log("Good"); music.Correct(); }
            else { music.Wrong(); }
            inputText = "";
        }
    }
}
