using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace nQ
{
  class CQueensMinConflict
  {
    public int N = 4;
    private Random random = new Random();

    int[] m_Board;

    //--------------------------------------------------------------------------------
    public CQueensMinConflict(int n)
    {
      N = n;
      m_Board = new int[n];
      Array.Clear(m_Board, 0, m_Board.Length);
      Init();
    }

    //--------------------------------------------------------------------------------
    public void Dispose()
    {

    }

    //--------------------------------------------------------------------------------
    private void Heuristic()
    {
      //-- 12
      //-- +.+.+
      //-- ..+..
      //-- ++Q++
      //-- ..+..
      //-- +.+.+
      int twelfth = N % 12;

      int[] tmpBoard = new int[N + 4];
      Array.Clear(tmpBoard, 0, tmpBoard.Length);

      int nIndex = 0;

      // Diagonal knight from 2
      for (int i = 2; i <= N; i += 2)
        tmpBoard[nIndex++] = i;

      if (twelfth == 3 || twelfth == 9)
      {
        tmpBoard[0] = 0;
        tmpBoard[nIndex++] = 2;
      }

      for (int i = 1; i <= N; i += 4)
      {
        if (twelfth == 8)
        {
          tmpBoard[nIndex++] = i + 2;
          tmpBoard[nIndex++] = i;
        }
        else
        {
          tmpBoard[nIndex++] = i;
          tmpBoard[nIndex++] = i + 2;
        }
      }

      if (twelfth == 2)
      {
        for (int i = 0; i <= N; i++)
        {
          if (tmpBoard[i] == 1)
            tmpBoard[i] = 3;
          else if (tmpBoard[i] == 3)
            tmpBoard[i] = 1;
          if (tmpBoard[i] == 5)
            tmpBoard[i] = 0;
        }
        tmpBoard[nIndex++] = 5;
      }

      if ((twelfth == 3) || (twelfth == 9))
      {
        for (int i = 0; i <= N; i++)
          if (tmpBoard[i] == 3 || tmpBoard[i] == 1)
            tmpBoard[i] = 0;
        tmpBoard[nIndex++] = 1;
        tmpBoard[nIndex] = 3;
      }

      nIndex = 0;

      for (int i = 0; i < tmpBoard.Length; i++)
        if (tmpBoard[i] != 0 && tmpBoard[i] <= N)
          m_Board[nIndex++] = tmpBoard[i] - 1;
    }


    //--------------------------------------------------------------------------------
    void Init(bool bRand = false)
    {
      if (bRand == true)
      {
        for (int i = 0, n = m_Board.Length; i < n; i++)
        {
          int j = random.Next(n);
          int rowToSwap = m_Board[i];
          m_Board[i] = m_Board[j];
          m_Board[j] = rowToSwap;
        }
      }
      else
      {
        Heuristic();
      }

      for (int i = 0; i < N; i++)
        if (m_Board[i] >= N || m_Board[i] < 0)
          m_Board[i] = random.Next(N - 1);
    }

    //--------------------------------------------------------------------------------
    int IsValid(int row, int col)
    {
      int count = 0;
      for (int c = 0; c < m_Board.Length; c++)
      {
        if (c == col) continue;
        int r = m_Board[c];
        if (r == row || Math.Abs(r - row) == Math.Abs(c - col)) count++;
      }
      return count;
    }

    //--------------------------------------------------------------------------------
    public void Find()
    {
      int moves = 0;

      List<int> candidates = new List<int>();

      while (true)
      {
        int maxConflicts = 0;
        candidates.Clear();
        for (int c = 0; c < m_Board.Length; c++)
        {
          int conflicts = IsValid(m_Board[c], c);
          if (conflicts == maxConflicts)
          {
            candidates.Add(c);
          }
          else if (conflicts > maxConflicts)
          {
            maxConflicts = conflicts;
            candidates.Clear();
            candidates.Add(c);
          }
        }

        if (maxConflicts == 0)
        {
          return;
        }

        int worstQueenColumn = candidates[random.Next(candidates.Count)];


        int minConflicts = m_Board.Length;
        candidates.Clear();
        for (int r = 0; r < m_Board.Length; r++)
        {
          int conflicts = IsValid(r, worstQueenColumn);
          if (conflicts == minConflicts)
          {
            candidates.Add(r);

          }
          else if (conflicts < minConflicts)
          {
            minConflicts = conflicts;
            candidates.Clear();
            candidates.Add(r);

          }
        }

        if (candidates.Count > 0)
        {
          m_Board[worstQueenColumn] =
              candidates[random.Next(candidates.Count)];
        }

        moves++;

        if (moves == m_Board.Length * 2)
        {
          Init(true);
          moves = 0;
        }

      }
    }

    //--------------------------------------------------------------------------------
    public void Print()
    {
      if (N < 34)
      {
        for (int i = 0; i < N; i++)
        {
          for (int j = 0; j < N; j++)
            if (m_Board[i] == j)
              Console.Write("Q");
            else
              Console.Write(".");
          Console.WriteLine("");
        }
      }
    }

    //------------------------------------------------------------------------------------------------
    public void PrintFile(string strFile)
    {
      using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFile))
      {
        char[] result = new char[N * N + N];

        int nIndex = 0;
        for (int i = 0; i < N; i++)
        {
          for (int j = 0; j < N; j++)
            if (m_Board[i] == j)
              result[nIndex++] = 'Q';
            else
              result[nIndex++] = '.';
          result[nIndex++] = '\n';
        }
        file.Write(result, 0, result.Length);
      }
    }

  }
}
