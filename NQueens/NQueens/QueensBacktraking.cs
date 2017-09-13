using System;
using System.IO;
using System.Text;
using System.Threading;

namespace nQ
{
  //------------------------------------------------------------------------------------------------
  //------------------------------------------------------------------------------------------------
  class CQueensBacktraking
  {
    public int N = 4;

    private int[] m_Board;

    static bool bRepeat = true;

    public CQueensBacktraking(int n)
    {
      N = n;
      m_Board = new int[n];
      Init();
    }

    //------------------------------------------------------------------------------------------------
    public void Dispose()
    {

    }

    //------------------------------------------------------------------------------------------------
    public void Print(bool noCheck = false)
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
      try
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
      catch (Exception /**/)
      {

      }

    }

    //------------------------------------------------------------------------------------------------
    static bool IsValid(int[] array, int ind)
    {
      for (int i = 0; i < ind; i++)
        if ((array[i] == array[ind]) || (array[i] == array[ind] + (ind - i)) || (array[i] == array[ind] - (ind - i)))
          return false;
      return true;
    }

    //------------------------------------------------------------------------------------------------
    public bool Find(int col = 0)
    {
      if (bRepeat && (col < N))
      {
        for (int i = 0; i < N; i++)
        {

          if (!bRepeat)
            break;
          m_Board[col] = i;
          if (IsValid(m_Board, col) == false)
            Find(col + 1);

        }

      }
      else
      {
        bRepeat = false;
      }

      return false;
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

    //------------------------------------------------------------------------------------------------
    void Init()
    {
      Heuristic();
    }
  }
}
