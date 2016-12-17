using System;
namespace SecretaryProblem
{
  class SecretaryProblemProgram
  {
    static Random rnd = new Random(0);

    static void Main(string[] args)
    {
      Console.WriteLine("\nBegin Secretary Problem (best choice) demo \n");

      double[] ratings = new double[] { 5.0, 2.0, 7.0,    1.0, 4.0, 0.0, 8.0, 3.0, 9.0, 6.0 };
      Console.WriteLine("Applicant ratings are: \n");
      for (int i = 0; i < ratings.Length; ++i)
        Console.Write(ratings[i].ToString("F1") + "  ");
      Console.WriteLine("\n");
      int selected = Select(ratings, true);  // select an applicant, verbose

      int numHiringSessions = 10000;
      int numApplicants = 100;

      Console.WriteLine("\n\nBegin simulation");
      Console.WriteLine("Number of simulated applicants      = " + numApplicants);
      Console.WriteLine("Number of simulated hiring sessions = " + numHiringSessions);

      double pBest = Simulation(numApplicants, numHiringSessions);
      Console.WriteLine("Simulation probability of selecting best applicant  = " + pBest.ToString("F4"));

      double pTheory = 1 / Math.E;
      Console.WriteLine("Theoretical probability of selecting best applicant = " + pTheory.ToString("F4"));

      Console.WriteLine("\nEnd Secretary Problem demo \n");
      Console.ReadLine();

    } // Main

    static double Simulation(int numApplicants, int numTrials)
    {
      // generate applicants
      double[] ratings = new double[numApplicants];  // index = applicant, value = rating
      for (int i = 0; i < numApplicants; ++i)
      {
        ratings[i] = (i * 1.0);  // so best rating is numApp.0
      }
      double optimalRating = 1.0 * (numApplicants - 1);

      int numBestSelected = 0;  // number times selected the best applicant
      for (int trial = 0; trial < numTrials; ++trial)
      {
        Shuffle(ratings);

        int selected = Select(ratings);  // select an applicant, non-verbose
        if (selected == -1)  // failed to select an applicant
          continue; 

        double rating = ratings[selected];  // did we get best applicant?
        if (rating == optimalRating)
          ++numBestSelected;
      }

      Console.WriteLine("Number of times best applicant selected = " + numBestSelected);
      double pBest = (numBestSelected * 1.0) / numTrials;
      return pBest;

    } // Simulation

    //static int Select2(double[] ratings)
    //{
    //  // select an applicant using skip n/e rule
    //  int numApplicants = ratings.Length;
    //  int numSkip = (int)(numApplicants / Math.E);  // truncate
    //  //int numSkip = (int)Math.Round(numApplicants / Math.E);  // round
    //  int candidate = 0;  // best applicant seen
    //  double bestRating = ratings[0];
    //  double rating = 0.0;

    //  // 1. prelim loop
    //  for (int applicant = 0; applicant < numSkip; ++applicant)
    //  {
    //    rating = ratings[applicant];
    //    if (rating > bestRating)  // new candidate
    //    {
    //      candidate = applicant;  // this person is best so far
    //      bestRating = rating;
    //    } // new candidate
    //  }

    //  // 2. hiring loop
    //  for (int applicant = numSkip; applicant < numApplicants; ++applicant)
    //  {
    //    rating = ratings[applicant];
    //    if (rating > bestRating)  // new candidate
    //    {
    //      candidate = applicant;  // this person is best so far
    //      return candidate;
    //    } // new candidate
    //  }

    //  return -1;  // fail. no new candidate appeared
    //} // Select

    static int Select(double[] ratings)
    {
      // select an applicant using skip n/e rule
      int numApplicants = ratings.Length;
      int numSkip = (int)(numApplicants / Math.E);  // truncate
      //int numSkip = (int)Math.Round(numApplicants / Math.E);  // round
      int candidate = 0;  // best applicant seen
      double bestRating = ratings[0];
      double rating = 0.0;
      bool readyToHire = false;
      
      for (int applicant = 0; applicant < numApplicants; ++applicant)
      {
        rating = ratings[applicant];
        if (applicant >= numSkip)
          readyToHire = true;
       
        if (rating > bestRating)  // new candidate
        {
          candidate = applicant;  // this person is best so far
          bestRating = rating;
          if (readyToHire == true)
            return candidate;
        } // new candidate
      } // for each applicant

      return -1;  // fail. no new candidate appeared
    } // Select

    static int Select(double[] ratings, bool verbose)
    {
      // select an applicant using skip n/e rule
      int numApplicants = ratings.Length;
      int numSkip = (int)(numApplicants / Math.E);  // truncate
      //int numSkip = (int)Math.Round(numApplicants / Math.E);  // round
      int candidate = 0;  // best applicant seen
      double bestRating = ratings[0];
      double rating = 0.0;
      bool readyToHire = false;
      bool v = verbose;

      if (v) Console.WriteLine("numApplicants = " + numApplicants);
      if (v) Console.WriteLine("numSkip = " + numSkip + "\n");

      for (int applicant = 0; applicant < numApplicants; ++applicant)
      {
        if (v) Console.WriteLine("==========================");
        rating = ratings[applicant];
        if (v) Console.WriteLine("Applicant = " + applicant);
        if (v) Console.WriteLine("Rating = " + rating.ToString("F1"));
        if (applicant >= numSkip)
          readyToHire = true;
        if (v) Console.WriteLine("Ready to hire = " + readyToHire);

        if (rating > bestRating)  // new candidate
        {
          if (v) Console.WriteLine("Applicant is the new candidate");
          candidate = applicant;  // this person is best so far
          bestRating = rating;
          if (readyToHire == true)
          {
            if (v) Console.WriteLine("Hiring this applicant");
            return candidate;
          }
          else
          {
            if (v) Console.WriteLine("Not ready to hire. Moving to next applicant");
          }
        } // new candidate
        else
        {
          if (v) Console.WriteLine("Applicant is not a candidate");
        }
        if (v) Console.WriteLine("==========================");
      }

      if (v) Console.WriteLine("No applicant was selected");

      return -1;  // no new candidate appeared
    } // Select (with verbose parameter)

    static void Shuffle(double[] ratings)
    {
      // Fisher-Yates in-place
      for (int i = 0; i < ratings.Length; ++i)
      {
        int ri = rnd.Next(i, ratings.Length);  // random index in [i, N)
        double tmp = ratings[i];
        ratings[i] = ratings[ri];
        ratings[ri] = tmp;
      }
    }

  } // Program

} // ns
