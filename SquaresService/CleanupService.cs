using ServiceStack;
using SquaresServiceInterface;
using System;

namespace SquaresService
{
    public class CleanupService : Service
    {
        public object Any(CleanupRequest request)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();

            Console.WriteLine("Cleanup Complete.");

            return new CleanupRequestResponse() { success = true };
        }
    }
}
