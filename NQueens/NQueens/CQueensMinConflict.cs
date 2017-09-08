using System;
using System.Collections.Generic;

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
      Init();
    }

    //--------------------------------------------------------------------------------
    public void Dispose()
    {

    }

    //--------------------------------------------------------------------------------
    void Init(bool bRand = false)
    {
    if(bRand == true)
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
        for (int i = 0, j = 0; i < N; i++)
        {
          if (((i) * 2) + 1 >= N)
          {
            if (j==0)
            {
              if (IsValid(i, (j++ * 2)) > 0)
              {
                m_Board[i] = random.Next(N - 1);
                j += 2;
              }
            }
            else
              m_Board[i] = (j++ * 2);
     
             

          }
          else
          {
            m_Board[i] = ((i) * 2) + 1;
          }

        }

      }
      
      
     
      Print();
      for (int i = 0; i < N; i++)
      if (m_Board[i]>=N ||m_Board[i]<0)
          m_Board[i]= random.Next(N-1);

      /*
      for (int i = 0, n = rows.Length; i < n; i++)
      {
        int j = random.Next(n);
        int rowToSwap = rows[i];
        rows[i] = rows[j];
        rows[j] = rowToSwap;
      }*/
      Print();
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

      //using (var progress = new ProgressBar())
      {
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

          int worstQueenColumn =  candidates[random.Next(candidates.Count)];

          
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
          //progress.Report((double)moves /( N/2));
          if (moves == m_Board.Length * 2)
          {
            Init(true);
            moves = 0;
          }
          
        }
      }
    }

    //--------------------------------------------------------------------------------
    public void Print()
    {
      if (N < 34)
      {
        for (int r = 0; r < m_Board.Length; r++)
        {
          for (int c = 0; c < m_Board.Length; c++)
          {
            Console.Write(m_Board[c] == r ? 'Q' : '.');
          }
          Console.WriteLine();
        }
      }
    }

    //--------------------------------------------------------------------------------
    public void PrintFile(string strFile)
    {
      using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFile))
      {
        for (int r = 0; r < m_Board.Length; r++)
        {
          for (int c = 0; c < m_Board.Length; c++)
          {
            file.Write(m_Board[c] == r ? 'Q' : '.');
          }
          file.WriteLine();
        }
      }
    }
  }
}
