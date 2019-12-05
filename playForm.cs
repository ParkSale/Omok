using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omok
{
    public partial class playForm : Form
    {
        const int INF = 1000000000;
        const int limit = 4;
        const int size = 28;
        const int edgeCount = 20;
        int[] dx = { 1, -1, 0, 0, 1, -1, -1, 1 }, dy = { 0, 0, 1, -1, 1, -1, 1, -1 };
        int targetX, targetY, beforeX = -1, beforeY = -1;
        enum HORSE
        {
            none = 1,
            BLACK,
            WHITE
        };
        HORSE[,] board = new HORSE[edgeCount, edgeCount];
        void init()
        {
            for (int i = 0; i < edgeCount; i++)
            {
                for (int j = 0; j < edgeCount; j++)
                {
                    board[i, j] = HORSE.none;
                }
            }
            beforeX = -1; beforeY = -1;
        }
        private int judge(int x, int y, HORSE p)
        {
            int ret = 0;
            for (int k = 0; k < 8; k += 2)
            {
                int cnt = 1;
                for (int i = 1; i <= 4; i++)
                {
                    int px = x + i * dx[k], py = y + i * dy[k];
                    if (px >= 0 && px < edgeCount && py >= 0 && py < edgeCount && board[px, py] == p)
                    {
                        cnt++;
                    }
                    else break;
                }
                for (int i = 1; i <= 4; i++)
                {
                    int px = x + i * dx[k + 1], py = y + i * dy[k + 1];
                    if (px >= 0 && px < edgeCount && py >= 0 && py < edgeCount && board[px, py] == p)
                    {
                        cnt++;
                    }
                    else break;
                }
                if (ret < cnt) ret = cnt;
            }
            return ret;
        }
        public playForm()
        {
            InitializeComponent();
        }

        private void boardBox_Paint(object sender, PaintEventArgs e)
        {   
            Graphics g = e.Graphics;
            Color lineColor = Color.Black;
            Pen p = new Pen(lineColor, 2);
            //테두리
            g.DrawLine(p, size / 2, size / 2, size / 2, size * edgeCount - size / 2);
            g.DrawLine(p, size / 2, size / 2, size * edgeCount - size / 2, size / 2);
            g.DrawLine(p, size / 2, size * edgeCount - size / 2, size * edgeCount - size / 2, size * edgeCount - size / 2);
            g.DrawLine(p, size * edgeCount - size / 2, size / 2, size * edgeCount - size / 2, size * edgeCount - size / 2);
            p = new Pen(lineColor, 1);
            //내부
            for (int i = size + size / 2; i < size * edgeCount - size / 2; i += size)
            {
                g.DrawLine(p, size / 2, i, size * edgeCount - size / 2, i);
                g.DrawLine(p, i, size / 2, i, size * edgeCount - size / 2);
            }
            init();
        }

        private void boardBox_MouseDown(object sender, MouseEventArgs e)
        {
            Graphics g = this.boardBox.CreateGraphics();
            int x = e.X / size;
            int y = e.Y / size;
            
            if (x < 0 || y < 0 || x >= edgeCount || y >= edgeCount)
            {
                MessageBox.Show("테두리를 벗어날 수 없습니다.");
                return;
            }
            if (board[x, y] != HORSE.none)
            {
                return;
            }
            board[x, y] = HORSE.BLACK;

            SolidBrush brush = new SolidBrush(Color.Black);
            g.FillEllipse(brush, x * size, y * size, size, size);
            if(!(beforeX == -1 && beforeY == -1))
            {
                brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, beforeX * size, beforeY * size, size, size);
            }
            
            if (judge(x, y,HORSE.BLACK) == 5)
            {
                MessageBox.Show("BLACK이 승리하였습니다.");
                Refresh();
                init();
                return;
            }
            else
            {
                int whiteX = -1, whiteY = -1, blackX = -1, blackY = -1;
                for (int i = 0; i < edgeCount; i++)
                {
                    for(int j = 0; j < edgeCount; j++)
                    {
                        if (board[i,j] == HORSE.none)
                        {
                            if (judge(i, j,HORSE.WHITE) == 5)
                            {
                                whiteX = i; whiteY = j;
                            }
                            if(judge(i,j,HORSE.BLACK) == 5)
                            {
                                blackX = i; blackY = j;
                            }
                        }
                    }
                }
                if(whiteX != -1 && whiteY != -1)
                {
                    //white가 놓아서 이길수 있으면
                    board[whiteX, whiteY] = HORSE.WHITE;
                    brush = new SolidBrush(Color.Gray);
                    g.FillEllipse(brush, whiteX * size, whiteY * size, size, size);
                    MessageBox.Show("WHITE가 승리하였습니다.");
                    Refresh();
                    init();
                    return;
                }
                if(blackX != -1 && blackY != -1)
                {
                    board[blackX, blackY] = HORSE.WHITE;
                    brush = new SolidBrush(Color.Gray);
                    beforeX = blackX; beforeY = blackY;
                    g.FillEllipse(brush, blackX * size, blackY * size, size, size);
                    turn.Text = "현재 BLACK의 차례입니다.";
                    turn.Update();
                    return;
                }
                else
                {
                    turn.Text = "현재 WHITE의 차례입니다.";
                    turn.Update();
                    alphaBetaPruning(0, -INF, INF);
                    board[targetX, targetY] = HORSE.WHITE;
                    beforeX = targetX; beforeY = targetY;
                    brush = new SolidBrush(Color.Gray);
                    g.FillEllipse(brush, targetX * size, targetY * size, size, size);
                    if (judge(targetX, targetY, HORSE.WHITE) == 5)
                    {
                        MessageBox.Show("WHITE가 승리하였습니다.");
                        Refresh();
                        init();
                        return;
                    }
                    turn.Text = "현재 BLACK의 차례입니다.";
                    turn.Update();
                }    
            }
        }

        int getScore()
        {
            //White(ai)에서 Black(user)의 점수를 뺀다
            //양쪽 열린1개 한쪽열린1개 양쪽 열린 2개 한쪽열린 2개 양쪽열린3개 한쪽열린3개 양쪽열린4개 한쪽열린4개 그 이상~
            int aiScore = 0, userScore = 0;

            for (int i = 0; i < edgeCount; i++)
            {
                for (int j = 0; j < edgeCount; j++)
                {
                    if (board[i, j] == HORSE.WHITE || board[i, j] == HORSE.BLACK)
                    {
                        for (int k = 0; k < 8; k += 2)
                        {
                            //1방향에서 공백받는거 vs 2방향에서 공백받는거
                            bool flag = false;
                            int cnt1 = 1, cnt2 = 1;
                            bool chk1 = false, chk2 = false;
                            for (int a = 1; a <= 5; a++)
                            {
                                int px = i + a * dx[k], py = j + a * dy[k];
                                if (px < 0 || px >= edgeCount || py < 0 || py >= edgeCount) break;
                                if (board[px, py] == board[i, j])
                                {
                                    cnt1++;
                                }
                                else if (board[px,py] == HORSE.none && flag == false)
                                {
                                    flag = true;
                                }
                                else if(board[px,py] == HORSE.none && flag == true)
                                {
                                    break;
                                }
                                else {
                                    //막혀있는거
                                    if (a >= 2 && board[px - dx[k], py - dy[k]] == HORSE.none) break;
                                    else chk1 = true;
                                    break;
                                }
                            }
                            for (int a = 1; a <= 5; a++)
                            {
                                int px = i + a * dx[k + 1], py = j + a * dy[k + 1];
                                if (px < 0 || px >= edgeCount || py < 0 || py >= edgeCount) break;
                                if (board[px, py] == board[i, j])
                                {
                                    cnt1++;
                                }
                                else if(board[px,py] == HORSE.none)
                                {
                                    break;
                                }
                                else {
                                    if (a >= 2 && board[px - dx[k + 1], py - dy[k + 1]] == HORSE.none) break;
                                    else chk2 = true;
                                    break;
                                }
                            }
                            bool chk3 = false, chk4 = false;
                            flag = false;
                            for (int a = 1; a <= 5; a++)
                            {
                                int px = i + a * dx[k + 1], py = j + a * dy[k + 1];
                                if (px < 0 || px >= edgeCount || py < 0 || py >= edgeCount) break;
                                if (board[px, py] == board[i, j])
                                {
                                    cnt2++;
                                }
                                else if (board[px,py] == HORSE.none && flag == false)
                                {
                                    flag = true;
                                }
                                else if(board[px,py] == HORSE.none && flag == true)
                                {
                                    break;
                                }
                                else {
                                    if (a >= 2 && board[px - dx[k + 1], py - dy[k + 1]] == HORSE.none) break;
                                    else chk3 = true;
                                    break;
                                }
                            }
                            for (int a = 1; a <= 5; a++)
                            {
                                int px = i + a * dx[k], py = j + a * dy[k];
                                if (px < 0 || px >= edgeCount || py < 0 || py >= edgeCount) break;
                                if (board[px, py] == board[i, j])
                                {
                                    cnt2++;
                                }
                                else if(board[px,py] == HORSE.none)
                                {
                                    break;
                                }
                                else
                                {
                                    if (a >= 2 && board[px - dx[k], py - dy[k]] == HORSE.none) break;
                                    else chk4 = true;
                                    break;
                                }
                            }
                            int max = 0;
                            if(chk1 == true && chk2 == true && chk3 == true && chk4 == true)
                            {
                                continue;
                            }
                            else if(chk1 == true && chk2 == true)
                            {
                                max = cnt2;
                                chk1 = chk3; chk2 = chk4;
                            }
                            else if(chk3 == true && chk4 == true)
                            {
                                max = cnt1;
                            }
                            else
                            {
                                if (cnt1 >= cnt2)
                                {
                                    max = cnt1;
                                }
                                else
                                {
                                    max = cnt2;
                                    chk1 = chk3; chk2 = chk4;
                                }
                            }
                            if (max == 0) continue;
                            if(max == 1)
                            {
                                if (chk1 == true && chk2 == true) continue;
                                else if(chk1 == true || chk2 == true)
                                {
                                    //한쪽열린 1개
                                    if (board[i, j] == HORSE.BLACK)
                                    {
                                        userScore += 5;
                                    }
                                    else aiScore += 5;
                                }
                                else
                                {
                                    //양쪽 열린
                                    if (board[i, j] == HORSE.BLACK) userScore += 100;
                                    else aiScore += 100;
                                }
                            }
                            else if(max == 2)
                            {
                                if (chk1 == true && chk2 == true) continue;
                                else if(chk1 == true || chk2 == true)
                                {
                                    //한쪽열린 2개
                                    if (board[i, j] == HORSE.BLACK) userScore += 150;
                                    else aiScore += 150;
                                }
                                else
                                {
                                    if (board[i, j] == HORSE.BLACK) userScore += 800;
                                    else aiScore += 800;
                                }
                            }
                            else if (max == 3)
                            {
                                if (chk1 == true && chk2 == true) continue;
                                else if (chk1 == true || chk2 == true)
                                {
                                    //한쪽열린 3개
                                    if (board[i, j] == HORSE.BLACK) userScore += 5000;
                                    else aiScore += 5000;
                                }
                                else
                                {
                                    if (board[i, j] == HORSE.BLACK) userScore += 100000;
                                    else aiScore += 100000;
                                }
                            }
                            else if(max == 4)
                            {
                                if (chk1 == true && chk2 == true) continue;
                                else if (chk1 == true || chk2 == true)
                                {
                                    //한쪽열린 4개
                                    if (board[i, j] == HORSE.BLACK) userScore += 500000;
                                    else aiScore += 500000;
                                }
                                else
                                {
                                    if (board[i, j] == HORSE.BLACK) userScore += 1000000;
                                    else aiScore += 1000000;
                                }
                            }
                            else if(max == 5)
                            {
                                if (chk1 == true && chk2 == true) continue;
                                else if (chk1 == true || chk2 == true)
                                {

                                    if (board[i, j] == HORSE.BLACK) userScore += 1000000;
                                    else aiScore += 1000000;
                                }
                                else
                                {
                                    if (board[i, j] == HORSE.BLACK) userScore += 1000000;
                                    else aiScore += 1000000;
                                }
                            }
                        }
                    }
                }
            }
            return aiScore - userScore;
        }

        int alphaBetaPruning(int depth, int alpha, int beta)
        {
            if (depth == limit)
            {
                return getScore();
            }
            if (depth % 2 == 0)
            {
                //WHITE가 둘 차례
                int max = -INF;
                int find = 0;
                for (int i = 0; i < edgeCount; i++)
                {
                    for (int j = 0; j < edgeCount; j++)
                    {
                        if (board[i, j] == HORSE.none)
                        {
                            bool flag = false;
                            for (int k = 0; k < 8; k++)
                            {
                                int px = i + dx[k], py = j + dy[k];
                                if (px >= 0 && px < edgeCount && py >= 0 && py < edgeCount && board[px, py] != HORSE.none)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                board[i, j] = HORSE.WHITE;
                                int value = alphaBetaPruning(depth + 1, alpha, beta);
                                board[i, j] = HORSE.none;
                                if (max < value)
                                {
                                    max = value;
                                    if (depth == 0)
                                    {
                                        targetX = i;
                                        targetY = j;
                                    }
                                }
                                if (alpha < max)
                                {
                                    alpha = max;
                                    if (alpha >= beta) find = 1;
                                }
                            }
                        }
                        if (find == 1) break;
                    }
                    if (find == 1) break;
                }
                return max;
            }
            else
            {
                //BLACK이 둘 차례
                int min = INF;
                int find = 0;
                for (int i = 0; i < edgeCount; i++)
                {
                    for (int j = 0; j < edgeCount; j++)
                    {
                        if (board[i, j] == HORSE.none)
                        {
                            bool flag = false;
                            for (int k = 0; k < 8; k++)
                            {
                                int px = i + dx[k], py = j + dy[k];
                                if (px >= 0 && px < edgeCount && py >= 0 && py < edgeCount && board[px, py] != HORSE.none)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                board[i, j] = HORSE.BLACK;
                                int value = alphaBetaPruning(depth + 1, alpha, beta);
                                board[i, j] = HORSE.none;
                                if (min > value) min = value;
                                if (beta > min)
                                {
                                    beta = min;
                                }
                                if (alpha >= beta) find = 1;
                            }
                        }
                        if(find == 1) break;
                    }
                    if(find == 1) break;
                }
                return min;
            }
        }
    }
}