using System;
using System.Threading;

namespace nQ
{
  //------------------------------------------------------------------------------------------------
  //------------------------------------------------------------------------------------------------
  class cQueens
  {
    int N = 4;
    private int[][] m_Board;

    static ProgressBar m_Progress;
    static int m_nProgress = 0;

    //------------------------------------------------------------------------------------------------
    private bool CheckSq(int rownum, int colnum)
    {
      int rowtmp = rownum;
      int coltmp = colnum;

      //-- Checking upper diagonal squares
      while (--rowtmp >= 0 && --coltmp >= 0)
      {
        if (m_Board[rowtmp][coltmp] == 1)
        {
          return false;
        }
      }
      rowtmp = rownum;
      coltmp = colnum;

      //-- Checking lower diagonal squares
      while (++rowtmp < N && --coltmp >= 0)
      {
        if (m_Board[rowtmp][coltmp] == 1)
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
          if (m_Board[r][c] == 1)
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
    public void Print()
    {
      if (N < 30)
      {
        if (CheckFinal())
        {
          for (int i = 0; i < N; i++)
          {
            Console.WriteLine("");
            for (int j = 0; j < N; j++)
            {

              Console.Write("{0}", m_Board[i][j]);

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
              file.Write("{0}", m_Board[i][j]);
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
        if (m_Board[row][i] != 0)
          return false;

      for (i = row, j = col; i >= 0 && j >= 0; i--, j--)
        if (m_Board[i][j] != 0)
          return false;

      for (i = row, j = col; j >= 0 && i < N; i++, j--)
        if (m_Board[i][j] != 0)
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
          m_Board[i][col] = 1;

          m_Progress.Report((double)m_nProgress++ / N);

          if (FindOld(col + 1))
            return true;

          m_Progress.Report((double)m_nProgress-- / N);
          m_Board[i][col] = 0;
        }
      }
      
      return false;
    }

    //------------------------------------------------------------------------------------------------
    bool Find(int col)
    {
      if (col >= N)
        return true;

      for (int i = 0; i < N; i++)
      {
        if (i % 2 != 0)
        {
          if (IsValid(i, col))
          {
            m_Board[i][col] = 1;

            m_Progress.Report((double)m_nProgress++ / N);

            if (Find(col + 1))
              return true;

            m_Progress.Report((double)m_nProgress-- / N);
            m_Board[i][col] = 0;
          }
        }
      }

      for (int i = 0; i < N; i++)
      {
        if (i % 2 == 0)
        {
          if (IsValid(i, col))
          {
            m_Board[i][col] = 1;
            m_Progress.Report((double)m_nProgress++ / N);

            if (Find(col + 1))
              return true;

            m_Progress.Report((double)m_nProgress-- / N);
            m_Board[i][col] = 0;
          }
        }
      }
      
      return false;
    }

    //------------------------------------------------------------------------------------------------
    void Init()
    {
      Console.Write("Initializing... ");
      using (var progress = new ProgressBar())
      {
        m_Board = new int[N][];
        for (int i = 0; i < N; i++)
        {
          m_Board[i] = new int[N];
          for (int i2 = 0; i2 < N; i2++)
            m_Board[i][i2] = 0;
          progress.Report((double)i / N);
        }
      }
      Console.WriteLine("Done.");

    }

    //------------------------------------------------------------------------------------------------
    static cQueens queens = new cQueens();

    //------------------------------------------------------------------------------------------------
    static void Do_Work(object starttime)
    {
      m_Progress = new ProgressBar();
      var watch = System.Diagnostics.Stopwatch.StartNew();

      queens.Init();

      Console.WriteLine("Searching... ");
      if (queens.Find(0) == false)
      {
        Console.WriteLine("Solution does not exist");
      }
      else
      {
        Console.WriteLine("Done.");
      }
      watch.Stop();
      TimeSpan ts = watch.Elapsed;

      string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
      Console.WriteLine("RunTime " + elapsedTime);

      Console.WriteLine("Printing... ");

      queens.Print();
      string strFile = string.Format("result{0}.txt", queens.N);
      queens.PrintFile(strFile);
      Console.WriteLine("Done.");
      m_Progress.Dispose();
    }

    //------------------------------------------------------------------------------------------------
    static void Times_up(object thread)
    {
      Thread t = (Thread)thread;
      Console.WriteLine("\nTime's Up!");
      string strFile = string.Format("NOT found result{0}.txt", queens.N);
      queens.PrintFile(strFile);
      t.Abort();
    }

    //------------------------------------------------------------------------------------------------
    static void Main(string[] args)
    { 
      if (args.Length <= 0)
      {
        Console.WriteLine("Number of queens (0 to find all queens from 4 to 1000 waiting 10 secs to find bugs)");
        Console.Write("N: ");
        string strN = Console.ReadLine();
        Console.WriteLine("Finding {0} Queens in {0}x{0} board", strN);
        queens.N = int.Parse(strN);
      }
      else
      {
        queens.N = int.Parse(args[0].ToString());
      }

      if (queens.N == 0)
      {
        for (queens.N = 4; queens.N < 1000; queens.N++)
        {
          Thread work = new Thread(Do_Work);
          Timer timer = new Timer(Times_up, work, 10000, Timeout.Infinite);
          DateTime StartTime = DateTime.Now;
          work.Start(StartTime);
          work.Join();
          Console.Write("End");
          timer.Dispose();
        }
      }
      else
        Do_Work(DateTime.Now);


      Console.ReadKey();
    }
  }
}
