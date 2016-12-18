Namespace SecretaryProblem
    Friend Class SecretaryProblemProgram
        Private Shared rnd As New Random(0)

        Shared Sub Main(ByVal args() As String)
            Console.WriteLine(vbCrLf & "Begin Secretary Problem (best choice) demo " & vbCrLf)

            Dim ratings() As Double = {5.0, 2.0, 7.0, 1.0, 4.0, 0.0, 8.0, 3.0, 9.0, 6.0}
            Console.WriteLine("Applicant ratings are: " & vbCrLf)
            For i  = 0 To ratings.Length - 1
                Console.Write(ratings(i).ToString("F1") & "  ")
            Next i
            Console.WriteLine(vbCrLf)
            Dim selected As Integer = [Select](ratings, True) ' select an applicant, verbose

            Dim numHiringSessions As Integer = 10000
            Dim numApplicants As Integer = 100

            Console.WriteLine(vbCrLf & vbCrLf & "Begin simulation")
            Console.WriteLine("Number of simulated applicants      = " & numApplicants)
            Console.WriteLine("Number of simulated hiring sessions = " & numHiringSessions)

            Dim pBest As Double = Simulation(numApplicants, numHiringSessions)
            Console.WriteLine("Simulation probability of selecting best applicant  = " & pBest.ToString("F4"))

            Dim pTheory As Double = 1 / Math.E
            Console.WriteLine("Theoretical probability of selecting best applicant = " & pTheory.ToString("F4"))

            Console.WriteLine(vbCrLf & "End Secretary Problem demo " & vbCrLf)
            Console.ReadLine()
        End Sub ' Main

        Private Shared Function Simulation(ByVal numApplicants As Integer, ByVal numTrials As Integer) As Double
            ' generate applicants
            Dim ratings(numApplicants - 1) As Double ' index = applicant, value = rating
            For i  = 0 To numApplicants - 1
                ratings(i) = (i * 1.0) ' so best rating is numApp.0
            Next i
            Dim optimalRating As Double = 1.0 * (numApplicants - 1)

            Dim numBestSelected As Integer = 0 ' number times selected the best applicant
            For trial  = 0 To numTrials - 1
                Shuffle(ratings)

                Dim selected As Integer = [Select](ratings) ' select an applicant, non-verbose
                If selected = -1 Then ' failed to select an applicant
                    Continue For
                End If

                Dim rating As Double = ratings(selected) ' did we get best applicant?
                If rating = optimalRating Then
                    numBestSelected += 1
                End If
            Next trial

            Console.WriteLine("Number of times best applicant selected = " & numBestSelected)
            Dim pBest As Double = (numBestSelected * 1.0) / numTrials
            Return pBest
        End Function ' Simulation

        'static int Select2(double[] ratings)
        '{
        '  // select an applicant using skip n/e rule
        '  int numApplicants = ratings.Length;
        '  int numSkip = (int)(numApplicants / Math.E);  // truncate
        '  //int numSkip = (int)Math.Round(numApplicants / Math.E);  // round
        '  int candidate = 0;  // best applicant seen
        '  double bestRating = ratings0;
        '  double rating = 0.0;

        '  // 1. prelim loop
        '  for (int applicant = 0; applicant < numSkip; ++applicant)
        '  {
        '    rating = ratingsapplicant;
        '    if (rating > bestRating)  // new candidate
        '    {
        '      candidate = applicant;  // this person is best so far
        '      bestRating = rating;
        '    } // new candidate
        '  }

        '  // 2. hiring loop
        '  for (int applicant = numSkip; applicant < numApplicants; ++applicant)
        '  {
        '    rating = ratingsapplicant;
        '    if (rating > bestRating)  // new candidate
        '    {
        '      candidate = applicant;  // this person is best so far
        '      return candidate;
        '    } // new candidate
        '  }

        '  return -1;  // fail. no new candidate appeared
        '} // Select

        Private Shared Function [Select](ByVal ratings() As Double) As Integer
            ' select an applicant using skip n/e rule
            Dim numApplicants As Integer = ratings.Length
            Dim numSkip As Integer = CInt(Fix(numApplicants / Math.E)) ' truncate
            'int numSkip = (int)Math.Round(numApplicants / Math.E);  // round
            Dim candidate As Integer = 0 ' best applicant seen
            Dim bestRating As Double = ratings(0)
            Dim rating As Double = 0.0
            Dim readyToHire As Boolean = False

            For applicant  = 0 To numApplicants - 1
                rating = ratings(applicant)
                If applicant >= numSkip Then
                    readyToHire = True
                End If

                If rating > bestRating Then ' new candidate
                    candidate = applicant ' this person is best so far
                    bestRating = rating
                    If readyToHire = True Then
                        Return candidate
                    End If
                End If ' new candidate
            Next applicant ' for each applicant

            Return -1 ' fail. no new candidate appeared
        End Function ' Select

        Private Shared Function [Select](ByVal ratings() As Double, ByVal verbose As Boolean) As Integer
            ' select an applicant using skip n/e rule
            Dim numApplicants As Integer = ratings.Length
            Dim numSkip As Integer = CInt(Fix(numApplicants / Math.E)) ' truncate
            'int numSkip = (int)Math.Round(numApplicants / Math.E);  // round
            Dim candidate As Integer = 0 ' best applicant seen
            Dim bestRating As Double = ratings(0)
            Dim rating As Double = 0.0
            Dim readyToHire As Boolean = False
            Dim v As Boolean = verbose

            If v Then
                Console.WriteLine("numApplicants = " & numApplicants)
            End If
            If v Then
                Console.WriteLine("numSkip = " & numSkip & vbCrLf)
            End If

            For applicant  = 0 To numApplicants - 1
                If v Then
                    Console.WriteLine("==========================")
                End If
                rating = ratings(applicant)
                If v Then
                    Console.WriteLine("Applicant = " & applicant)
                End If
                If v Then
                    Console.WriteLine("Rating = " & rating.ToString("F1"))
                End If
                If applicant >= numSkip Then
                    readyToHire = True
                End If
                If v Then
                    Console.WriteLine("Ready to hire = " & readyToHire)
                End If

                If rating > bestRating Then ' new candidate
                    If v Then
                        Console.WriteLine("Applicant is the new candidate")
                    End If
                    candidate = applicant ' this person is best so far
                    bestRating = rating
                    If readyToHire = True Then
                        If v Then
                            Console.WriteLine("Hiring this applicant")
                        End If
                        Return candidate
                    Else
                        If v Then
                            Console.WriteLine("Not ready to hire. Moving to next applicant")
                        End If
                    End If ' new candidate
                Else
                    If v Then
                        Console.WriteLine("Applicant is not a candidate")
                    End If
                End If
                If v Then
                    Console.WriteLine("==========================")
                End If
            Next applicant

            If v Then
                Console.WriteLine("No applicant was selected")
            End If

            Return -1 ' no new candidate appeared
        End Function ' Select (with verbose parameter)

        Private Shared Sub Shuffle(ByVal ratings() As Double)
            ' Fisher-Yates in-place
            For i  = 0 To ratings.Length - 1
                Dim ri As Integer = rnd.Next(i, ratings.Length) ' random index in [i, N)
                Dim tmp As Double = ratings(i)
                ratings(i) = ratings(ri)
                ratings(ri) = tmp
            Next i
        End Sub
    End Class ' Program
End Namespace ' ns
