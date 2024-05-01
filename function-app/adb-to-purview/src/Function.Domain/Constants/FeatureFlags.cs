namespace Function.Domain.Constants
{
    public static class FeatureFlags
    {
        public const string CreateOrUpdateSynapseAsset = "CreateOrUpdateSynapseAsset";
        public static class Logging
        {
            public const string LogOlMessageToExternalStore = "LogOlMessageToExternalStore";
        }
        public static class Security
        {
            public const string ValidateHttpOlSourceHeader = "ValidateHttpOlSourceHeader";
        }
    }
}