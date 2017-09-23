namespace NaCoDoKina.Api.Results
{
    /// <summary>
    /// Operation result without data 
    /// </summary>
    public class Result
    {
        public static Result CreateSucceeded() => new Result(null);

        public static Result CreateFailed(string failReason) => new Result(failReason);

        public static Result CreateFailed() => new Result(string.Empty);

        public bool Succeeded { get; }

        public string FailReason { get; }

        protected Result(string failReason)
        {
            Succeeded = failReason is null;
            FailReason = failReason;
        }
    }

    /// <inheritdoc/>
    /// <summary>
    /// Operation result with data 
    /// </summary>
    /// <typeparam name="TData"> Returned data type </typeparam>
    public class Result<TData> : Result
    {
        public TData Data { get; }

        public static Result<TData> CreateSucceeded(TData data) => new Result<TData>(data, null);

        public new static Result<TData> CreateFailed(string failReason) => new Result<TData>(default(TData), failReason);

        public new static Result<TData> CreateFailed() => new Result<TData>(default(TData), string.Empty);

        protected Result(TData data, string failReason) : base(failReason)
        {
            Data = data;
        }
    }
}