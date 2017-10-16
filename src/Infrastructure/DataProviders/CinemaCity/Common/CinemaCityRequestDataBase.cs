using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Settings;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataProviders.CinemaCity.Common
{
    public abstract class CinemaCityRequestDataBase : RequestDataBase
    {
        private readonly IEnumerable<IRequestParameter> _defaultRequestParameters;
        public string RequestSpecificPattern { get; }

        protected ILogger<CinemaCityRequestDataBase> Logger { get; }

        private string PatternBase => "data-api-service/v1/quickbook/10103/";

        protected CinemaCityRequestDataBase(string requestSpecificPattern, IEnumerable<IRequestParameter> defaultRequestParameters, CinemaNetworksSettings cinemaNetworksSettings, ILogger<CinemaCityRequestDataBase> logger)
        {
            _defaultRequestParameters = defaultRequestParameters ?? throw new ArgumentNullException(nameof(defaultRequestParameters));
            RequestSpecificPattern = requestSpecificPattern ?? throw new ArgumentNullException(nameof(requestSpecificPattern));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
        }

        protected override string PathPattern => PatternBase + RequestSpecificPattern;

        protected override string QueryPattern => "attr={Attributes}&lang={LanguageTag}";

        protected override string BaseUrl => CinemaNetworksSettings.CinemaCityNetwork.Url;

        public CinemaNetworksSettings CinemaNetworksSettings { get; }

        public override Request Parse(params IRequestParameter[] requestParameters)
        {
            using (Logger.BeginScope(nameof(Parse)))
            {
                Logger.LogDebug("Parsing Cinema City request data {@RequestData} with parameters {@RequestParameters}", this, requestParameters);

                var completeUrl = ResourcePattern;
                foreach (var requestParameter in requestParameters.Concat(_defaultRequestParameters))
                {
                    var result = requestParameter.SubstituteTemplate(completeUrl);

                    if (!result.IsSuccess)
                    {
                        Logger.LogWarning("Request parameter {@RequestParameter} substitute failed with message {SubstituteMessage}", requestParameter, result.FailureReason);
                        continue;
                    }

                    completeUrl = result.Value;
                }

                Logger.LogDebug("Parsing completed with url {Url}", completeUrl);

                return new GetRequest(AbsoluteUrl(completeUrl));
            }
        }
    }
}