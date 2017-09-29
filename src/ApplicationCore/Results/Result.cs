namespace NaCoDoKina.Api.Results
{
    /// <summary>
    /// Operation result without value 
    /// </summary>
    public class Result
    {
        public static Result<TData> Success<TData>(TData data) => Result<TData>.CreateSucceeded(data);

        public static Result<TData> Failure<TData>(string failReason) => Result<TData>.CreateFailed(failReason);

        public static Result<TData> Failure<TData>() => Failure<TData>(string.Empty);

        public static Result Success() => new Result();

        public static Result Failure(string failReason) => new Result(false, failReason);

        public static Result Failure() => new Result(false);

        /// <summary>
        /// Indicates that operation ended with success 
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Failure reason 
        /// </summary>
        public string FailureReason { get; }

        protected Result(bool isSuccess = true, string failureReason = "")
        {
            IsSuccess = isSuccess;
            FailureReason = failureReason;
        }
    }

    /// <inheritdoc/>
    /// <summary>
    /// Operation result with value 
    /// </summary>
    /// <typeparam name="TValue"> Returned value type </typeparam>
    public class Result<TValue> : Result
    {
        /// <summary>
        /// Operation result return value 
        /// </summary>
        public TValue Value { get; }

        public static Result<TValue> CreateSucceeded(TValue data) => new Result<TValue>(data);

        public static Result<TValue> CreateFailed(string failReason) => new Result<TValue>(default(TValue), false, failReason);

        protected Result(TValue value, bool isSuccess = true, string failureReason = "") : base(isSuccess, failureReason)
        {
            Value = value;
        }
    }
}