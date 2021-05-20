namespace PaymentGateway.Contracts
{
    public static class ApiRoutes
    {
        private const string Root = "api";
        private const string Version = "v1";
        private const string Base = Root + "/" + Version;

        public static class Payment
        {
            public const string Process = Base + "/payment";
        }

        public static class Transaction
        {
            public const string Get = Base + "/transaction/{transactionId}";
        }
    }
}