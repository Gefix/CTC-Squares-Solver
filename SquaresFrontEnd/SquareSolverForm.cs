using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using ServiceStack;
using SquaresServiceInterface;
using SquaresSolverTypes;
using SquaresSolver;

namespace Squares
{
    public partial class SquareSolverForm : Form
    {
        public SquareSolverForm()
        {
            InitializeComponent();

            for (int i = 1; i <= 40; i++)
            {
                Console.WriteLine(i.ToString() + "," + SquareTilingCombinatorics.UniqueTilings(i) + "," + SquareTilingCombinatorics.DerivedTilings(i));
            }
        }

        private async Task<bool> OnlineSolverTask(string mode)
        {
            DateTime timeStarted = DateTime.UtcNow;

            PuzzleRequest pr;

            try
            {
                pr = SolverHelper.FetchOnlinePuzzle(mode);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while retrieving a puzzle from the puzzle server: \n" + ex.Message);
                return false;
            }

            int resultsCount;
            int bestApproach;

            DateTime internalTimeStarted = DateTime.UtcNow;

            var solution = findBestTiling(pr, out resultsCount, out bestApproach);

            int internalTotalTime = (int)(DateTime.UtcNow - internalTimeStarted).TotalMilliseconds;

            string responseFromServer;
            
            try
            {
                responseFromServer = SolverHelper.SendOnlineResponse(pr, mode, solution);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while retrieving a puzzle from the puzzle server: \n" + ex.Message);
                return false;
            }

            int totalTime = (int)(DateTime.UtcNow - timeStarted).TotalMilliseconds;

            string logLine = SolverHelper.LogOnlineResult(mode, responseFromServer, resultsCount, internalTotalTime, totalTime);

            UpdateGUI(pr, resultsCount, bestApproach, solution, logLine);

            return await CleanupWorkers();
        }

        private async Task<bool> LocalSolverTask(string fileName)
        {
            DateTime timeStarted = DateTime.UtcNow;

            PuzzleRequest pr;

            try
            {
                pr = SolverHelper.FetchLocalPuzzle(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while retrieving a local puzzle: \n" + ex.Message);
                return false;
            }

            int resultsCount;
            int bestApproach;

            DateTime internalTimeStarted = DateTime.UtcNow;

            var solution = findBestTiling(pr, out resultsCount, out bestApproach);

            int internalTotalTime = (int)(DateTime.UtcNow - internalTimeStarted).TotalMilliseconds;

            int totalTime = (int)(DateTime.UtcNow - timeStarted).TotalMilliseconds;

            string logLine = SolverHelper.LogLocalResult(fileName, solution.Count, resultsCount, internalTotalTime, totalTime);

            UpdateGUI(pr, resultsCount, bestApproach, solution, logLine);

            return await CleanupWorkers();
        }

        private async Task<bool> CleanupWorkers()
        {
            foreach (var thread in threads)
            {
                thread.Join();
            }

            try
            {
                List<Task<CleanupRequestResponse>> cleanupTasks = new List<Task<CleanupRequestResponse>>();

                for (int i = 0; i < 9; i++)
                {
                    var client = new JsonServiceClient("http://localhost:" + (8900 + i).ToString() + "/");

                    var cleanup = client.PostAsync<CleanupRequestResponse>(new CleanupRequest());
                    cleanupTasks.Add(cleanup);
                }

                foreach (var task in cleanupTasks)
                {
                    var result = await task;

                    if (result.success != true)
                    {
                        MessageBox.Show("Something Strange Happened");
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                MessageBox.Show("Something Bad Happened");
                return false;
            }
        }

        private void UpdateGUI(PuzzleRequest pr, int resultsCount, int bestApproach, List<Square> solution, string logLine)
        {
            this.Invoke((MethodInvoker)delegate
            {
                int n = pr.width;
                int m = pr.height;

                map = new bool[n, m];

                for (int y = 0; y < m; y++)
                {
                    for (int x = 0; x < n; x++)
                    {
                        int ix = x;
                        int iy = y;

                        map[ix, iy] = pr.puzzle[y][x];
                    }
                }

                shownSolution = solution;

                this.labelSquareCount.Text = shownSolution.Count.ToString();

                this.labelResultsProcessed.Text = resultsCount.ToString();

                Console.WriteLine(logLine);

                lbLog.Items.Add(logLine);

                int visibleItems = lbLog.ClientSize.Height / lbLog.ItemHeight;
                lbLog.TopIndex = Math.Max(lbLog.Items.Count - visibleItems + 1, 0);

                this.cbTranspose.Checked = (bestApproach >> 2) % 2 == 1;
                this.cbInverseX.Checked = (bestApproach >> 1) % 2 == 1;
                this.cbInverseY.Checked = bestApproach % 2 == 1;

                pictureBox1.Refresh();
            });
        }

        private void btnSolveOnline_Click(object sender, EventArgs e)
        {
            var ProcessPool = StartServices();

            ThreadPool.QueueUserWorkItem(async arg =>
            {
                // Always do one trial run to warm up the services
                if (!await OnlineSolverTask("trial"))
                {
                    StopServices(ProcessPool);
                    return;
                }

                int count = 0;

                Int32.TryParse(mtbPuzzleCount.Text, out  count);

                string mode = cbMode.Checked ? "contest" : "trial";

                for (int i = 0; i < count; i++)
                {
                    if (!await OnlineSolverTask(mode)) break;
                }

                MessageBox.Show("Puzzle solving complete. You can review the services' console output. Click OK to clean-up.");

                StopServices(ProcessPool);
            });
        }

        private static void StopServices(List<Process> ProcessPool)
        {
            Thread.Sleep(2000);

            foreach (var process in ProcessPool)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                }
            }
        }

        private static List<Process> StartServices()
        {
            var ProcessPool = new List<Process>();

            for (int i = 0; i < 9; i++)
            {
                ProcessPool.Add(Process.Start(@"SquaresService.exe", i.ToString()));
            }

            Thread.Sleep(3000);
            return ProcessPool;
        }

        bool[,] map;

        List<Square> shownSolution;

        List<Thread> threads;

        private List<Square> findBestTiling(PuzzleRequest pr, out int results, out int best)
        {
            List<Square> bestSolution = new List<Square>();

            var solutions = new List<Square>[9];

            threads = new List<Thread>();

            int Directions = 8;

            DateTime begin = DateTime.UtcNow;

            threads.Add(new Thread(
                arg =>
                {
                    int index = (int)arg;

                    var solution = findMinRemoteFast(pr);
                    //List<Square> solution = null;

                    lock (solutions)
                    {
                        if (solution != null && (solutions[index] == null || solution.Count < solutions[index].Count))
                        {
                            solutions[index] = solution;
                        }
                    }

                }));

            threads[0].Start(8);

            for (int i = 0; i < Directions; i++)
            {
                threads.Add(new Thread(
                    arg =>
                    {
                        int index = (int)arg;

                        var solution = findMinRemote(pr, (index >> 2) % 2 == 1, (index >> 1) % 2 == 1, index % 2 == 1);

                        lock (solutions)
                        {
                            if (solution != null && (solutions[index] == null || solution.Count < solutions[index].Count))
                            {
                                solutions[index] = solution;
                            }
                        }

                    }));

                threads[threads.Count - 1].Start(i);
            }

            bool stillWaiting;

            var bestSolutionCount = Int32.MaxValue;
            var bestSolutionIndex = 0;

            var sleeper = new System.Threading.ManualResetEvent(false);

            for (int index = 0; index < Directions; index++)
            {
                var solution = findMin(pr, (index >> 2) % 2 == 1, (index >> 1) % 2 == 1, index % 2 == 1);

                lock (solutions)
                {
                    if (solution != null && (solutions[index] == null || solution.Count < solutions[index].Count))
                    {
                        solutions[index] = solution;
                    }
                }
            }

            do
            {
                sleeper.WaitOne(50);

                stillWaiting = false;

                foreach (var thread in threads)
                {
                    if (thread.IsAlive) stillWaiting = true;
                }

                lock (solutions)
                {
                    results = 0;

                    for (int i = 0; i <= Directions; i++)
                    {
                        if (solutions[i] == null) continue;

                        if (solutions[i].Count < bestSolutionCount)
                        {
                            bestSolutionIndex = i;
                            bestSolutionCount = solutions[i].Count;
                        }

                        results++;
                    }
                }

                if ((DateTime.UtcNow - begin).TotalMilliseconds > RunTimeConfiguration.TaskTimeout) break;

            } while (stillWaiting);

            best = bestSolutionIndex;

            bool transpose = (bestSolutionIndex >> 2) % 2 == 1;
            bool invertX = (bestSolutionIndex >> 1) % 2 == 1;
            bool invertY = bestSolutionIndex % 2 == 1; ;

            int N = transpose ? pr.height : pr.width;
            int M = transpose ? pr.width : pr.height;

            foreach (var square in solutions[bestSolutionIndex])
            {
                var newSquare = new Square()
                {
                    x = invertX ? N - square.x - square.size : square.x,
                    y = invertY ? M - square.y - square.size : square.y,
                    size = square.size
                };

                if (transpose)
                {
                    int temp = newSquare.x;
                    newSquare.x = newSquare.y;
                    newSquare.y = temp;
                }

                bestSolution.Add(newSquare);
            }

            return bestSolution;
        }

        List<Square> findMinRemote(PuzzleRequest pr, bool transpose, bool invX, bool invY)
        {
            int n = transpose ? pr.height : pr.width;
            int m = transpose ? pr.width : pr.height;

            var map = new List<bool[]>();

            for (int y = 0; y < m; y++)
            {
                map.Add(new bool[n]);
            }

            for (int y = 0; y < m; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    int ix = invX ? n - x - 1 : x;
                    int iy = invY ? m - y - 1 : y;

                    map[iy][ix] = transpose ? pr.puzzle[x][y] : pr.puzzle[y][x];
                }
            }

            var client = new JsonServiceClient("http://localhost:" + (8900 + (transpose ? 4 : 0) + (invX ? 2 : 0) + (invY ? 1 : 0)).ToString() + "/");

            int costMargin = 5; // pr.height > 30 ? 4 : 5;

            try
            {
                SquareSolverResponse response = client.Post<SquareSolverResponse>(new SquareSolver { Map = map, CostMargin = costMargin });

                return response.Solution;
            }
            catch
            {
                return null;
            }
        }

        List<Square> findMinRemoteFast(PuzzleRequest pr)
        {
            int n = pr.width;
            int m = pr.height;

            var map = new List<bool[]>();

            for (int y = 0; y < m; y++)
            {
                map.Add(new bool[n]);
            }

            for (int y = 0; y < m; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    int ix = x;
                    int iy = y;

                    map[iy][ix] = pr.puzzle[y][x];
                }
            }

            var client = new JsonServiceClient("http://localhost:" + (8900 + 8).ToString() + "/");

            try
            {
                SquareSolverResponse response = client.Post<SquareSolverResponse>(new SquareSolver { Map = map, CostMargin = 0 });

                return response.Solution;
            }
            catch
            {
                return null;
            }
        }

        List<Square> findMin(PuzzleRequest pr, bool transpose, bool invX, bool invY)
        {
            int n = transpose ? pr.height : pr.width;
            int m = transpose ? pr.width : pr.height;

            var map = new bool[n, m];

            for (int y = 0; y < m; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    int ix = invX ? n - x - 1 : x;
                    int iy = invY ? m - y - 1 : y;

                    map[ix, iy] = transpose ? pr.puzzle[x][y] : pr.puzzle[y][x];
                }
            }

            int costMargin = 0;

            SquareTilingBase squareTiling;

            if (m <= 40)
            {
                squareTiling = new SquareTilingHeuristic(map, false, costMargin);
            }
            else
            {
                squareTiling = new SquareTilingHeuristicLarge(map, false, costMargin, 0);
            }

            var solution = squareTiling.Solve();

            return solution;
        }

        private void workerThread(Object o)
        {
        }

        private void btnSolveLocal_Click(object sender, EventArgs e)
        {
            DialogResult result = openLocalFileDialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK || openLocalFileDialog.FileNames.Count() == 0) return;

            string[] fileNames = openLocalFileDialog.FileNames;

            var ProcessPool = StartServices();

            ThreadPool.QueueUserWorkItem(async arg =>
            {
                foreach (string fileName in openLocalFileDialog.FileNames)
                {
                    if (!await LocalSolverTask(fileName)) break;
                }

                MessageBox.Show("Puzzle solving complete. You can review the services' console output. Click OK to clean-up.");

                StopServices(ProcessPool);
            });
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            Pen p = new Pen(Color.Black);

            Brush b = new SolidBrush(Color.Black);

            int rs = 20;

            if (map == null) return;

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y])
                    {
                        e.Graphics.DrawRectangle(p, rs + x * rs, rs + y * rs, rs, rs);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(b, rs + x * rs, rs + y * rs, rs, rs);
                    }
                }
            }

            if (shownSolution == null) return;
            
            Random r = new Random(0);

            for (int i = 0; i < shownSolution.Count(); i++)
            {
                Brush b1 = new SolidBrush(Color.FromArgb((1280 - shownSolution[i].x * 10) % 128 + 128, (shownSolution[i].y * 10) % 128 + 128, (shownSolution[i].size * 40) % 128 + 128));

                e.Graphics.FillRectangle(b1, rs + shownSolution[i].x * rs, rs + shownSolution[i].y * rs, shownSolution[i].size * rs, shownSolution[i].size * rs);
                
                e.Graphics.DrawRectangle(p, rs + shownSolution[i].x * rs, rs + shownSolution[i].y * rs, shownSolution[i].size * rs, shownSolution[i].size * rs);
            }
        }
    }
}
