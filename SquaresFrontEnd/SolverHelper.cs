using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ServiceStack;
using SquaresSolverTypes;

namespace Squares
{
    public class SolverHelper
    {
        private const string puzzleServerUrl = "http://techchallenge.cimpress.com/";
        private const string registrationKey = "yourCimpressTechChallengeRegistrationKey";

        private const string outputFolder = @".\\puzzles\\";

        public static PuzzleRequest FetchOnlinePuzzle(string mode)
        {
            PuzzleRequest pr;

            Directory.CreateDirectory(outputFolder + mode);

            string puzzleUrl = puzzleServerUrl + registrationKey + "/" + mode + "/puzzle";

            WebRequest wrPuzzle = WebRequest.Create(puzzleUrl);

            string puzzle = new StreamReader(wrPuzzle.GetResponse().GetResponseStream()).ReadToEnd();

            pr = JsonSerializer.DeserializeFromString<PuzzleRequest>(puzzle);

            string outputFileName = outputFolder + mode + "\\" + pr.id + ".json";

            using (var output = new StreamWriter(new FileStream(outputFileName, FileMode.Append)))
            {
                output.Write(puzzle);
            }

            return pr;
        }

        public static PuzzleRequest FetchLocalPuzzle(string fileName)
        {
            PuzzleRequest pr;

            using (var inputStream = new FileStream(fileName, FileMode.Open))
            {
                pr = JsonSerializer.DeserializeFromStream<PuzzleRequest>(inputStream);
            }

            return pr;
        }

        public static string SendOnlineResponse(PuzzleRequest pr, string mode, List<Square> solution)
        {
            PuzzleResponse response = new PuzzleResponse();

            response.id = pr.id;
            response.SetSquares(solution);

            string postData = JsonSerializer.SerializeToString<PuzzleResponse>(response);

            string solutionUrl = puzzleServerUrl + registrationKey + "/" + mode + "/solution";

            var wrSolution = (HttpWebRequest)WebRequest.Create(solutionUrl);

            wrSolution.Method = "POST";
            wrSolution.ContentType = "application/json";
            wrSolution.Accept = "application/json";
            Stream dataStream = wrSolution.GetRequestStream();
            dataStream.Write(postData);
            dataStream.Flush();
            dataStream.Close();

            WebResponse solutionResponse;

            solutionResponse = wrSolution.GetResponse();

            dataStream = solutionResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            Console.WriteLine(responseFromServer);

            reader.Close();
            dataStream.Close();
            solutionResponse.Close();
            return responseFromServer;
        }

        public static string LogOnlineResult(string mode, string responseFromServer, int resultsCount, int internalTotalTime, int totalTime)
        {
            string logLine = responseFromServer + "," + totalTime.ToString() + "," + internalTotalTime.ToString() + "," + resultsCount.ToString();

            var logFileName = outputFolder + mode + "_run.log";

            using (var output = new StreamWriter(new FileStream(logFileName, FileMode.Append)))
            {
                output.WriteLine(logLine);
            }
            return logLine;
        }

        public static string LogLocalResult(string fileName, int squaresCount, int resultsCount, int internalTotalTime, int totalTime)
        {
            string logLine = Path.GetFileNameWithoutExtension(fileName) + "," + squaresCount.ToString() + "," + totalTime.ToString() + "," + internalTotalTime.ToString() + "," + resultsCount.ToString();

            Directory.CreateDirectory(outputFolder);

            var logFileName = outputFolder + new DirectoryInfo(Path.GetDirectoryName(fileName)).Name + "_run.log";

            using (var output = new StreamWriter(new FileStream(logFileName, FileMode.Append)))
            {
                output.WriteLine(logLine);
            }
            return logLine;
        }
    }
}
