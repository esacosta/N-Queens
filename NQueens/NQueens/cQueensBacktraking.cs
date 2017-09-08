using System;
using System.Threading;

namespace nQ
{
  //------------------------------------------------------------------------------------------------
  //------------------------------------------------------------------------------------------------
  class CQueensBacktraking
  {
    public int N = 4;
    private bool[][] m_Board;
    //private ProgressBar m_Progress;
    //private int m_nProgress = 0;

    public CQueensBacktraking(int n)
    {
      N = n;
      Init();
    }

    public void Dispose()
    {
      //m_Progress.Dispose();
    }

    //------------------------------------------------------------------------------------------------
    private bool CheckSq(int rownum, int colnum)
    {
      int rowtmp = rownum;
      int coltmp = colnum;

      //-- Checking upper diagonal squares
      while (--rowtmp >= 0 && --coltmp >= 0)
      {
        if (m_Board[rowtmp][coltmp] == true)
        {
          return false;
        }
      }
      rowtmp = rownum;
      coltmp = colnum;

      //-- Checking lower diagonal squares
      while (++rowtmp < N && --coltmp >= 0)
      {
        if (m_Board[rowtmp][coltmp] == true)
        {
          return false;
        }
      }
      return true;
    }

    //------------------------------------------------------------------------------------------------
    private bool CheckFinal()
    {
      int nQueens = 0;
      for (int r = 0; r < N; r++)
      {
        for (int c = 0; c < N; c++)
        {
          if (m_Board[r][c] == true)
          {
            if (CheckSq(r, c) == false)
              return false;
            else
              nQueens++;
          }
        }
      }

      if (nQueens != N)
        return false;

      return true;
    }

    //------------------------------------------------------------------------------------------------
    public void Print(bool noCheck = false)
    {
      if (N < 34 || noCheck)
      {
        if (CheckFinal() || noCheck)
        {
          for (int i = 0; i < N; i++)
          {
            for (int j = 0; j < N; j++)
            {
              Console.Write("{0}", m_Board[i][j] == true? "Q": ".");
            }
            Console.WriteLine("");
          }
        }
        else
          Console.WriteLine("Solution does not exist");
      }
    }

    //------------------------------------------------------------------------------------------------
    public void PrintFile(string strFile)
    {
      using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFile))
      {
        if (CheckFinal())
        {
          for (int i = 0; i < N; i++)
          {
            for (int j = 0; j < N; j++)
            {
              file.Write("{0}", m_Board[i][j] == true ? "Q" : ".");
            }
            file.WriteLine("");
          }
        }
        else
        {
          file.WriteLine("Solution does not exist");
          Console.WriteLine("Solution does not exist");
        }
      }
    }

    //------------------------------------------------------------------------------------------------
    private bool IsValid(int row, int col)
    {
      int i, j;

      for (i = 0; i < col; i++)
        if (m_Board[row][i] == true)
          return false;

      for (i = row, j = col; i >= 0 && j >= 0; i--, j--)
        if (m_Board[i][j] == true)
          return false;

      for (i = row, j = col; j >= 0 && i < N; i++, j--)
        if (m_Board[i][j] == true)
          return false;

      return true;
    }

    //------------------------------------------------------------------------------------------------
    bool FindOld(int col)
    {
      if (col >= N)
        return true;

      for (int i = 0; i < N; i++)
      {
        if (IsValid(i, col))
        {
          m_Board[i][col] = true;

          //m_Progress.Report((double)m_nProgress++ / N);

          if (FindOld(col + 1))
            return true;

          //m_Progress.Report((double)m_nProgress-- / N);
          m_Board[i][col] = false;
        }
      }
      
      return false;
    }

    //------------------------------------------------------------------------------------------------
    public bool Find(int col = 0)
    {
      if (col >= N)
        return true;

      for (int i = 0; i < N; i++)
      {
        if (i % 2 != 0)
        {
          if (IsValid(i, col))
          {
            m_Board[i][col] = true;

            //m_Progress.Report((double)m_nProgress++ / N);

            if (Find(col + 1))
              return true;

            //m_Progress.Report((double)m_nProgress-- / N);
            m_Board[i][col] = false;
          }
        }
      }

      
      for (int i = 0; i < N; i++)
      {
        if (i % 2 == 0)
        {
          if (IsValid(i, col))
          {
            
            m_Board[i][col] = true;
            //m_Progress.Report((double)m_nProgress++ / N);

            if (Find(col + 1))
              return true;

            //m_Progress.Report((double)m_nProgress-- / N);
            m_Board[i][col] = false;
          }
        }
      }
      
      return false;
    }

    //------------------------------------------------------------------------------------------------
    void Init()
    {
      //Console.WriteLine("Initializing... ");
      //using (var progress = new ProgressBar())
      {
        m_Board = new bool[N][];
        for (int i = 0; i < N; i++)
        {
          m_Board[i] = new bool[N];
          Array.Clear(m_Board[i], 0, N);
                   
         // progress.Report((double)i / N);
        }
      }
      //m_Progress = new ProgressBar();
      
      //Console.WriteLine("Done.");

    }

    
  }
}
