using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public Board board;
    public int[] bestMove;
    public int bestScore;
    private long[] MangDiemTanCong;
    private long[] MangDiemPhongNgu;
    private void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        if (PlayerPrefs.GetInt("IsHard") == 0)
        {
            MangDiemTanCong = new long[7] { 0, 8, 72, 648, 5832, 52488, 472392 };
            MangDiemPhongNgu = new long[7] { 0, 2, 16, 128, 1024, 8192, 65536 };
        }
        else
        {
            MangDiemTanCong = new long[7] { 0, 4, 32, 256, 2048, 16384, 131072 };
            MangDiemPhongNgu = new long[7] { 0, 1, 11, 121, 1331, 14641, 161051 };
        }
    }
    private void Update()
    {
        if(!board.xTurn && !board.logic.gameOver) { NewAi(); board.xTurn = true; }
    }
    private int max(int a, int b)
    {
        if(a > b) { return a; }
        else { return b; }
    }
    private int min(int a, int b)
    {
        if(a < b) { return a; }
        else { return b;}
    }
    #region BadAi
    /*private void AiMove()
    {
        bestScore = -255;
        bestMove = new int[2];
        for(int i = board.row_min; i <= board.row_max; i++) {
            for(int j = board.col_min; j <= board.col_max; j++) {
                if (board.matrix[i,j] == 0) {
                    board.matrix[i, j] = 2;
                    int score = Minimax(board.matrix, 0 ,-255, 255, false, i, j);
                    board.matrix[i, j] = 0;
                    if(score > bestScore){
                        bestScore = score;
                        bestMove[0] = i; bestMove[1] = j;
                    }
                }
            }
        }
        board.matrix[bestMove[0], bestMove[1]] = 2;
        board.music.Click(); //because lazy :>
        if (board.Check(bestMove[0], bestMove[1])) { Debug.Log("Win"); board.whoWin(bestMove[0], bestMove[1]); }
    }
    
    // O = 1, X = -1, tie = 0
    private int Minimax(int[,] matrixBoard, int depth, int alpha, int beta, bool isMaximizing, int row, int col)
    {
        if(board.Check(row, col)) {
            if (board.matrix[row, col] == 1) { return 1; }
            else if (board.matrix[row, col] == 2) { return -1; }
        }
        else if(depth == 4) { return 0; }
        if (isMaximizing) {
            int bestScore = -255;
            for (int i = board.row_min; i <= board.row_max; i++) {
                for (int j = board.col_min; j <= board.col_max; j++) {
                    if (board.matrix[i, j] == 0) {
                        board.matrix[i, j] = 1;
                        int score = Minimax(matrixBoard, depth + 1,alpha, beta, false, i, j);
                        board.matrix[i, j] = 0;
                        if(score > bestScore) { bestScore = score; }
                        alpha = max(alpha, score);
                        if(beta <= alpha) { break; }
                    }
                }
            }
            return bestScore;
        } else {
            int bestScore = +255;
            for (int i = board.row_min; i <= board.row_max; i++) {
                for (int j = board.col_min; j <= board.col_max; j++) {
                    if (board.matrix[i, j] == 0)
                    {
                        board.matrix[i, j] = 2;
                        int score = Minimax(matrixBoard, depth + 1, alpha, beta, true, i, j);
                        board.matrix[i, j] = 0;
                        if (score < bestScore) { bestScore = score; }
                        beta = min(beta, score);
                        if(beta <= alpha) { break; }
                    }
                }
            }
            return bestScore;
        }
    }*/
    #endregion

    public void NewAi()
    {
        int[] GoodMove = FindMove();
        board.matrix[GoodMove[0], GoodMove[1]] = 2;
        board.music.Click(); //because lazy :>
        if (board.Check(GoodMove[0], GoodMove[1])) { Debug.Log("Win"); board.whoWin(GoodMove[0], GoodMove[1]); }
    }
    private int[] FindMove()
    {
        int[] cell = new int[2];
        long maxScore = 0;
        for(int i = 0; i < 25; i++)
        {
            for(int j = 0; j < 25; j++)
            {
                if (board.matrix[i,j] == 0)
                {
                    long DiemTanCong = DiemTC_Doc(i,j) + DiemTC_Ngang(i, j) + DiemTC_CheoXuoi(i, j) + DiemTC_CheoNguoc(i, j);
                    long DiemPhongNgu = DiemPN_Doc(i, j) + DiemPN_Ngang(i, j) + DiemPN_CheoXuoi(i, j) + DiemPN_CheoNguoc(i, j); ;
                    long temp = DiemTanCong > DiemPhongNgu ? DiemTanCong : DiemPhongNgu;
                    if (maxScore < temp) {
                        maxScore = temp;
                        cell[0] = i; cell[1] = j;
                    }
                }
            }
        }
        return cell;
    }
    #region TanCong
    private long DiemTC_Doc(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curRow + i < 25; i++)
        {
            if (board.matrix[curRow + i, curCol] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow + i, curCol] == 1) {
                SoQuanDich++;
                break;
            } else if (board.matrix[curRow + i, curCol] == 0 && curRow + i < 24 && curRow + i >= 1
                      && board.matrix[curRow + i + 1, curCol] == 2 && board.matrix[curRow + i - 1, curCol] == 2) 
            {
                continue;
            } 
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curRow - i >= 0; i++)
        {
            if (board.matrix[curRow - i, curCol] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow - i, curCol] == 1)
            {
                SoQuanDich++;
                break;
            }
            else if (board.matrix[curRow - i, curCol] == 0 && curRow - i >= 1 && curRow - i < 24
                    && board.matrix[curRow - i - 1, curCol] == 2 && board.matrix[curRow - i + 1, curCol] == 2) 
            {
                continue;
            }
            else { break; }
        }
        if(SoQuanDich == 2) { return 0; }
        DiemTong -= MangDiemPhongNgu[SoQuanDich];//de tang chi so ao :>
        DiemTong += MangDiemTanCong[SoQuanTa]; //cong diem tc
        return DiemTong;
    }
    private long DiemTC_Ngang(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curCol + i < 25; i++)
        {
            if (board.matrix[curRow, curCol + i] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow, curCol + i] == 1)
            {
                SoQuanDich++;
                break;
            }
            else if (board.matrix[curRow , curCol + i] == 0 && curCol + i < 24 && curCol + i >= 1
                      && board.matrix[curRow , curCol + i + 1] == 2 && board.matrix[curRow , curCol + i - 1] == 2) 
            {
                continue;
            }
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curCol - i >= 0; i++)
        {
            if (board.matrix[curRow, curCol - i] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow, curCol - i] == 1)
            {
                SoQuanDich++;
                break;
            }
            else if (board.matrix[curRow, curCol - i] == 0 && curCol - i < 24 && curCol - i >= 1
                      && board.matrix[curRow, curCol - i + 1] == 2 && board.matrix[curRow, curCol - i - 1] == 2)
            {
                continue;
            }
            else { break; }
        }
        if (SoQuanDich == 2) { return 0; }
        DiemTong -= MangDiemPhongNgu[SoQuanDich];//de tang chi so ao :>
        DiemTong += MangDiemTanCong[SoQuanTa]; //cong diem tc
        return DiemTong;
    }
    private long DiemTC_CheoXuoi(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curCol + i < 25 && curRow + i < 25; i++)
        {
            if (board.matrix[curRow + i, curCol + i] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow + i, curCol + i] == 1)
            {
                SoQuanDich++;
                break;
            } else if (board.matrix[curRow + i, curCol + i] == 0 && curCol + i < 24 && curCol + i >= 1 && curRow + i < 24 && curRow + i >= 1
                      && board.matrix[curRow + i + 1, curCol + i + 1] == 2 && board.matrix[curRow + i - 1, curCol + i - 1] == 2)
            {
                continue;
            }
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curCol - i >= 0 && curRow - i >= 0; i++)
        {
            if (board.matrix[curRow - i, curCol - i] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow - i, curCol - i] == 1)
            {
                SoQuanDich++;
                break;
            }
            else if (board.matrix[curRow - i, curCol - i] == 0 && curCol - i < 24 && curCol - i >= 1 && curRow - i < 24 && curRow - i >= 1
                      && board.matrix[curRow - i + 1, curCol - i + 1] == 2 && board.matrix[curRow - i - 1, curCol - i - 1] == 2)
            {
                continue;
            }
            else { break; }
        }
        if (SoQuanDich == 2) { return 0; }
        DiemTong -= MangDiemPhongNgu[SoQuanDich];//de tang chi so ao :>
        DiemTong += MangDiemTanCong[SoQuanTa]; //cong diem tc
        return DiemTong;
    }
    private long DiemTC_CheoNguoc(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curCol + i < 25 && curRow - i >= 0; i++)
        {
            if (board.matrix[curRow - i, curCol + i] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow - i, curCol + i] == 1)
            {
                SoQuanDich++;
                break;
            }
            else if (board.matrix[curRow - i, curCol + i] == 0 && curCol + i < 24 && curCol + i >= 1 && curRow - i < 24 && curRow - i >= 1
                      && board.matrix[curRow - i + 1, curCol + i + 1] == 2 && board.matrix[curRow - i - 1, curCol + i - 1] == 2)
            {
                continue;
            }
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curCol - i >= 0 && curRow + i < 25; i++)
        {
            if (board.matrix[curRow + i, curCol - i] == 2) { SoQuanTa++; }
            else if (board.matrix[curRow + i, curCol - i] == 1)
            {
                SoQuanDich++;
                break;
            }
            else if (board.matrix[curRow + i, curCol - i] == 0 && curCol - i < 24 && curCol - i >= 1 && curRow + i < 24 && curRow + i >= 1
                      && board.matrix[curRow + i + 1, curCol - i + 1] == 2 && board.matrix[curRow + i - 1, curCol - i - 1] == 2)
            {
                continue;
            }
            else { break; }
        }
        if (SoQuanDich == 2) { return 0; }
        DiemTong -= MangDiemPhongNgu[SoQuanDich];//de tang chi so ao :>
        DiemTong += MangDiemTanCong[SoQuanTa]; //cong diem tc
        return DiemTong;
    }
    #endregion
    #region PhongNgu
    private long DiemPN_Doc(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curRow + i < 25; i++)
        {
            if (board.matrix[curRow + i, curCol] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow + i, curCol] == 1)
            {
                SoQuanDich++;
            }
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curRow - i >= 0; i++)
        {
            if (board.matrix[curRow - i, curCol] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow - i, curCol] == 1)
            {
                SoQuanDich++;
            }
            else { break; }
        }
        if (SoQuanTa == 2) { return 0; }
        DiemTong -= MangDiemTanCong[SoQuanTa];
        DiemTong += MangDiemPhongNgu[SoQuanDich]; //cong diem tc
        return DiemTong;
    }
    private long DiemPN_Ngang(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curCol + i < 25; i++)
        {
            if (board.matrix[curRow, curCol + i] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow, curCol + i] == 1)
            {
                SoQuanDich++;
            }
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curCol - i >= 0; i++)
        {
            if (board.matrix[curRow, curCol - i] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow, curCol - i] == 1)
            {
                SoQuanDich++;
            }
            else { break; }
        }
        if (SoQuanTa == 2) { return 0; }
        DiemTong -= MangDiemTanCong[SoQuanTa];
        DiemTong += MangDiemPhongNgu[SoQuanDich]; //cong diem tc
        return DiemTong;
    }
    private long DiemPN_CheoXuoi(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curCol + i < 25 && curRow + i < 25; i++)
        {
            if (board.matrix[curRow + i, curCol + i] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow + i, curCol + i] == 1)
            {
                SoQuanDich++;
            }
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curCol - i >= 0 && curRow - i >= 0; i++)
        {
            if (board.matrix[curRow - i, curCol - i] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow - i, curCol - i] == 1)
            {
                SoQuanDich++;
            }
            else { break; }
        }
        if (SoQuanTa == 2) { return 0; }
        DiemTong -= MangDiemTanCong[SoQuanTa];
        DiemTong += MangDiemPhongNgu[SoQuanDich]; //cong diem tc
        return DiemTong;
    }
    private long DiemPN_CheoNguoc(int curRow, int curCol)
    {
        long DiemTong = 0;
        int SoQuanTa = 0;
        int SoQuanDich = 0;
        for (int i = 1; i < 6 && curCol + i < 25 && curRow - i >= 0; i++)
        {
            if (board.matrix[curRow - i, curCol + i] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow - i, curCol + i] == 1)
            {
                SoQuanDich++;
            }
            else { break; } //cai tien neu 2 o co cach nhau bang o trong thi noi lai
        }
        for (int i = 1; i < 6 && curCol - i >= 0 && curRow + i < 25; i++)
        {
            if (board.matrix[curRow + i, curCol - i] == 2) { SoQuanTa++; break; }
            else if (board.matrix[curRow + i, curCol - i] == 1)
            {
                SoQuanDich++;
            }
            else { break; }
        }
        if (SoQuanTa == 2) { return 0; }
        DiemTong -= MangDiemTanCong[SoQuanTa];
        DiemTong += MangDiemPhongNgu[SoQuanDich]; //cong diem tc
        return DiemTong;
    }
    #endregion
}
