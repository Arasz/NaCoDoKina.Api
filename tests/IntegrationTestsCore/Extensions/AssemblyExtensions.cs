using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Reflection;

namespace IntegrationTestsCore.Extensions
{
    public static class ApplicationEnvironmentExtensions
    {
        /// <summary>
        /// Gets the full path to the target project path that we wish to test 
        /// </summary>
        /// <param name="solutionRelativePath">
        /// The parent directory of the target project. e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="application"> The target project's assembly. </param>
        /// <returns> The full path to the target project. </returns>
        public static string GetProjectPath(this ApplicationEnvironment application, string solutionRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            var solutionPath = GetSolutionPath(application);

            return Path.GetFullPath(Path.Combine(solutionPath, solutionRelativePath, projectName));
        }

        /// <summary> Gets the full path to the target project path that we wish to test </summary>
        /// </param> <param name="application"> The target project's assembly. </param> <returns> The
        /// full path to the target project. </returns>
        public static string GetSolutionPath(this ApplicationEnvironment application)
        {
            // Get currently executing test project path
            var applicationBasePath = application.ApplicationBasePath;

            // Find the folder which contains the solution file. We then use this information to find
            // the target project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, "NaCoDoKinaApi.sln"));
                if (solutionFileInfo.Exists)
                {
                    return directoryInfo.FullName;
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}