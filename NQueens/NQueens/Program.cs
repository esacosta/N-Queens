using System;
using System.Collections.Generic;
using System.Threading;

namespace nQ
{
  class Program
  {
    //------------------------------------------------------------------------------------------------
    static int N = 4;

    static List<Thread> listThBacktraking = new List<Thread>();
    static List<Thread> listThMinConflict = new List<Thread>();
    
    static void Do_Work(object starttime)
    {
      Thread workBacktraking = new Thread(Do_WorkBacktraking);
      Thread workMinConflict = new Thread(Do_WorkMinConflict);
    
      listThBacktraking.Add(workBacktraking);
      listThMinConflict.Add(workMinConflict);

      workBacktraking.Start(starttime);
      workMinConflict.Start(starttime);

      workBacktraking.Join();
      workMinConflict.Join();
    }
    //------------------------------------------------------------------------------------------------
    static void Do_WorkBacktraking(object starttime)
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();

      CQueensBacktraking board = new CQueensBacktraking(N);

      Console.WriteLine("Searching by Heuristic confirm by Backtraking... ");
      board.Find();

      board.Dispose();
      watch.Stop();

      foreach (Thread th in listThMinConflict)
        th.Abort();
      TimeSpan ts = watch.Elapsed;

      string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
      Console.WriteLine("RunTime " + elapsedTime);
      Console.WriteLine("Done by Backtraking.");

      Console.Write("Printing... ");
      board.Print();

      board.PrintFile(string.Format("result{0}.txt", N));
      Console.WriteLine("Done");
    }

    //------------------------------------------------------------------------------------------------
    static void Do_WorkMinConflict(object starttime)
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();

      CQueensMinConflict board = new CQueensMinConflict(N);
     
      Console.WriteLine("Searching by Heuristic confirm by MinConflict... ");
      board.Find();
      
      board.Dispose();
      watch.Stop();
      foreach (Thread th in listThBacktraking)
        th.Abort();



      TimeSpan ts = watch.Elapsed;

      string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
      Console.WriteLine("RunTime " + elapsedTime);
      Console.WriteLine("Done by MinConflict.");

      Console.Write("Printing... ");
      board.Print();
      board.PrintFile(string.Format("result{0}.txt", N));
      Console.WriteLine("Done");

    }

    //------------------------------------------------------------------------------------------------
    static void Times_up(object thread)
    {
      Thread t = (Thread)thread;
      Console.WriteLine("\nTime's Up!");
      string strFile = string.Format("NOT found result{0}.txt", N);
      using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFile))
      {
        file.WriteLine(strFile);
      }
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
        N = int.Parse(strN);
      }
      else
      {
        N = int.Parse(args[0].ToString());
      }

      if (N == 0)
      {
        for (N = 0; N <= 1000; N++)
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
