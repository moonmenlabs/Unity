using System;
using GitHub.Unity;

namespace IntegrationTests
{
    class IntegrationTestEnvironment : IEnvironment
    {
        private static readonly ILogging logger = Logging.GetLogger<IntegrationTestEnvironment>();
        private readonly bool enableTrace;

        private readonly string extensionInstallPath;
        private readonly NPath integrationTestEnvironmentPath;

        private DefaultEnvironment defaultEnvironment;
        private string unityProjectPath;

        public IntegrationTestEnvironment(NPath solutionDirectory, NPath environmentPath = null,
            bool enableTrace = false)
        {
            defaultEnvironment = new DefaultEnvironment();

            environmentPath = environmentPath ??
                defaultEnvironment.GetSpecialFolder(Environment.SpecialFolder.LocalApplicationData)
                                  .ToNPath()
                                  .EnsureDirectoryExists(ApplicationInfo.ApplicationName + "-IntegrationTests");

            extensionInstallPath = solutionDirectory.Parent.Parent.Parent.Combine("GitHub.Api");
            integrationTestEnvironmentPath = environmentPath;
            this.enableTrace = enableTrace;

            if (enableTrace)
            {
                logger.Trace("EnvironmentPath: \"{0}\" SolutionDirectory: \"{1}\" ExtensionInstallPath: \"{2}\"",
                    environmentPath, solutionDirectory, extensionInstallPath);
            }
        }

        public string ExpandEnvironmentVariables(string name)
        {
            throw new NotImplementedException();
        }

        public string GetEnvironmentVariable(string v)
        {
            var environmentVariable = defaultEnvironment.GetEnvironmentVariable(v);
            if (enableTrace)
            {
                logger.Trace("GetEnvironmentVariable: {0}={1}", v, environmentVariable);
            }
            return environmentVariable;
        }

        public string GetSpecialFolder(Environment.SpecialFolder folder)
        {
            var ensureDirectoryExists = integrationTestEnvironmentPath.EnsureDirectoryExists(folder.ToString());
            var specialFolderPath = ensureDirectoryExists.ToString();

            if (enableTrace)
            {
                logger.Trace("GetSpecialFolder: {0}", specialFolderPath);
            }

            return specialFolderPath;
        }

        public string UserProfilePath => integrationTestEnvironmentPath.CreateDirectory("user-profile-path");

        public string Path => Environment.GetEnvironmentVariable("PATH");
        public string NewLine => Environment.NewLine;

        public string GitExecutablePath
        {
            get { return defaultEnvironment.GitExecutablePath; }
            set
            {
                if (enableTrace)
                {
                    logger.Trace("Setting GitExecutablePath to " + value);
                }
                defaultEnvironment.GitExecutablePath = value;
            }
        }

        public bool IsWindows => defaultEnvironment.IsWindows;
        public bool IsLinux => defaultEnvironment.IsLinux;
        public bool IsMac => defaultEnvironment.IsMac;

        public string UnityApplication { get; set; }

        public string UnityAssetsPath { get; set; }

        public string UnityProjectPath
        {
            get { return unityProjectPath; }
            set
            {
                if (enableTrace)
                {
                    logger.Trace("Setting UnityProjectPath to " + value);
                }
                unityProjectPath = value;
            }
        }

        public string ExtensionInstallPath
        {
            get { return extensionInstallPath; }
            set {}
        }

        public NPath UserCachePath { get; set; }
        public NPath SystemCachePath { get; set; }

        public string RepositoryPath { get; set; }

        public string GitInstallPath
        {
            get { return defaultEnvironment.GitInstallPath; }
        }

        public IRepository Repository { get; set; }
    }
}