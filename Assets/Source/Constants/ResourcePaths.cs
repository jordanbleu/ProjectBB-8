namespace Assets.Source.Constants
{
    /// <summary>
    /// Paths to required resources should be defined here
    /// </summary>
    public static class ResourcePaths
    {
        /// <summary>
        /// Points to the strings folder
        /// </summary>
        public static string StringsDirectory => $"{ResourcesDirectory}/Strings"; 


        /// <summary>
        /// This should point to the resources folder independent of platform (hopefully)
        /// </summary>
        public static string ResourcesDirectory => $"{UnityEngine.Application.dataPath}/Resources";

    }
}
