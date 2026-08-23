namespace portfolio_backend.Helpers
{
    static class CorsHelper
    {
        public static string[] GetAllowedOrigins()
        {
            return ["http://venhoff.org", "https://venhoff.org"];
        }
    }
}

