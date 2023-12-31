﻿using FluentResults;
using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using System.Threading;

namespace MongoNet.MongoDataAPI.Client
{
    public class MongoDataAPIClient
    {
        //mongodb atlas db and cluster instance access configuration
        private string DataSource { get; set; } = string.Empty;
        private string DataBase { get; set; } = string.Empty;
        private string Collection { get; set; } = string.Empty;

        //mongodb data api access configuration
        private const string ApiAuthOption = "api-key";
        private string ApiUrl { get; set; } = string.Empty;
        private string ApiId { get; set; } = string.Empty;
        private string ApiKey { get; set; } = string.Empty;

        public MongoDataAPIClient(string dataSource, string dataBase, string collection, string apiUrl, string apiId, string apiKey)
        {
            DataSource = dataSource;
            DataBase = dataBase;
            Collection = collection;
            ApiUrl = apiUrl;
            ApiId = apiId;
            ApiKey = apiKey;
        }


        /// <summary>
        /// <para>Performs a Find One request to the mongodb data api using the provided Atlas' cluster, database and collection configurations.</para>
        /// <para>It's possible to override API Access Options through the use of <paramref name="apiOptions"/> optional parameter, also it's possible to</para>
        /// <para>override App Service's EndpPoint and Action through the use of <paramref name="requestOptions"/> optional parameter.</para>
        /// </summary>
        /// <param name="apiOptions">The API options.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <returns>A <see cref="Result"/> that holds an <see cref="IFlurlResponse"/> instance.</returns>
        public Task<IFlurlResponse> FindOne(FilterOptions filterOptions,
                                            CancellationToken cancellationToken,
                                            ApiAccessOptions?
                                            apiOptions = null,
                                            RequestOptions? requestOptions = null)
        {
            requestOptions ??= new RequestOptions { EndPoint = "data", Version = "v1", };

            Url url = new(apiOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.FindOne.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiOptions?.BearerToken}");
            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiOptions?.ApiKey ?? ApiKey));
            }

            var response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,
                projection = requestOptions.Projection,
                filter = filterOptions.Filter,
            }, cancellationToken);

            return response;
        }

        /// <summary>
        /// <para>Performs a Find request using the specified <paramref name="filterOptions"/>.</para>
        /// <para>It's possible to override API Access Options through the use of <paramref name="apiOptions"/> optional parameter, also it's possible to</para>
        /// <para>override App Service's EndpPoint and Action through the use of <paramref name="requestOptions"/> optional parameter.</para>
        /// </summary>
        /// <param name="filterOptions">The filter options.</param>
        /// <param name="apiOptions">The API options.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <returns>A <see cref="Result"/> that holds an <see cref="IFlurlResponse"/> instance.</returns>
        public Task<IFlurlResponse> FindMany(FilterOptions filterOptions,
                                             CancellationToken cancellationToken,
                                             ApiAccessOptions? apiOptions = null,
                                             RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response = null;
            requestOptions ??= new RequestOptions();

            if (filterOptions.Filter is null)
            {
                return response;
            }

            Url url = new(apiOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.Find.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");
            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,

                filter = filterOptions.Filter,
                projection = filterOptions.Projection,
                sort = filterOptions.Sort,
                limit = filterOptions.Limit,
                skip = filterOptions.Skip,
            }, cancellationToken);

            return response;
        }

        /// <summary>
        /// <para>Performs a Insert One request to the mongodb data api using the providedA tlas' cluster, database and collection configurations.</para>
        /// <para>It's possible to override API Access Options through the use of <paramref name="apiOptions"/> optional parameter, also it's possible to</para>
        /// <para>override App Service's EndpPoint and Action through the use of <paramref name="requestOptions"/> optional parameter.</para>
        /// </summary>
        /// <param name="apiOptions">The API options.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <returns>A <see cref="Result"/> that holds an <see cref="IFlurlResponse"/> instance.</returns>
        public Task<IFlurlResponse> InsertOne(CancellationToken cancellationToken,
                                              ApiAccessOptions? apiOptions = null,
                                              RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response;
            requestOptions ??= new RequestOptions();

            if (requestOptions?.Document is null)
            {
                return null;
            }

            Url url = new(apiOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.InsertOne.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");

            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,
                document = requestOptions.Document
            }, cancellationToken);

            return response;
        }

        /// <summary>
        /// <para>Performs a Insert Many request to the mongodb data api using the providedA tlas' cluster, database and collection configurations.</para>
        /// <para>It's possible to override API Access Options through the use of <paramref name="apiOptions" /> optional parameter, also it's possible to</para>
        /// <para>override App Service's EndpPoint and Action through the use of <paramref name="requestOptions" /> optional parameter.</para>
        /// </summary>
        /// <param name="documents">The documents to be inserted.</param>
        /// <param name="apiOptions">The API options.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <returns>A <see cref="Result"/> that holds an <see cref="IFlurlResponse"/> instance.</returns>
        public Task<IFlurlResponse> InsertMany(object[] documents,
                                               CancellationToken cancellationToken,
                                               ApiAccessOptions? apiOptions = null,
                                               RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response;
            requestOptions ??= new RequestOptions();

            if (documents is null)
            {
                return null;
            }

            Url url = new(apiOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.InsertMany.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");

            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,

                documents
            }, cancellationToken);

            return response;
        }

        public Task<IFlurlResponse> UpdateOne(FilterOptions filterOptions,
                                              UpdateOptions updateOptions,
                                              CancellationToken cancellationToken,
                                              ApiAccessOptions? apiAccessOptions = null,
                                              RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response;
            requestOptions ??= new RequestOptions();

            if (filterOptions is null || updateOptions is null)
            {

                return null;
            }

            Url url = new(apiAccessOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiAccessOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.UpdateOne.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiAccessOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiAccessOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");

            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiAccessOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            request.BeforeCall(RewriteBodyAsync);

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,

                filter = filterOptions.Filter,
                update = updateOptions.UpdateDefinition,
                upsert = updateOptions.IsUpsert,

            }, cancellationToken);

            return response;
        }

        public Task<IFlurlResponse> UpdateMany(FilterOptions filterOptions,
                                               UpdateOptions updateOptions,
                                               CancellationToken cancellationToken,
                                               ApiAccessOptions? apiAccessOptions = null,
                                               RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response;
            requestOptions ??= new RequestOptions();

            if (filterOptions is null || updateOptions is null)
            {
                return null;
            }

            Url url = new(apiAccessOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiAccessOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.UpdateMany.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiAccessOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiAccessOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");

            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiAccessOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            request.BeforeCall(RewriteBodyAsync);

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,

                filter = filterOptions.Filter,
                update = updateOptions.UpdateDefinition,
                upsert = updateOptions.IsUpsert,

            }, cancellationToken);

            return response;
        }

        public Task<IFlurlResponse> ReplaceOne(FilterOptions filterOptions,
                                               UpdateOptions updateOptions,
                                               CancellationToken cancellationToken,
                                               ApiAccessOptions? apiAccessOptions = null,
                                               RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response;
            requestOptions ??= new RequestOptions();

            if (filterOptions is null || updateOptions is null)
            {
                return null;
            }

            Url url = new(apiAccessOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiAccessOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.ReplaceOne.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiAccessOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiAccessOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");

            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiAccessOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            request.BeforeCall(RewriteBodyAsync);

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,

                filter = filterOptions.Filter,
                replacement = updateOptions.Replacement,
                upsert = updateOptions.IsUpsert

            }, cancellationToken);

            return response;
        }

        public Task<IFlurlResponse> DeleteOne(FilterOptions filterOptions,
                                              CancellationToken cancellationToken,
                                              ApiAccessOptions? apiAccessOptions = null,
                                              RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response;
            requestOptions ??= new RequestOptions();

            if (filterOptions is null)
            {
                return null;
            }

            Url url = new(apiAccessOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiAccessOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.DeleteOne.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiAccessOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiAccessOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");


            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiAccessOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            request.BeforeCall(RewriteBodyAsync);

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,

                filter = filterOptions.Filter

            }, cancellationToken);

            return response;
        }

        public Task<IFlurlResponse> DeleteMany(FilterOptions filterOptions,
                                               CancellationToken cancellationToken,
                                               ApiAccessOptions? apiAccessOptions = null,
                                               RequestOptions? requestOptions = null)
        {
            Task<IFlurlResponse>? response;
            requestOptions ??= new RequestOptions();

            if (filterOptions is null)
            {
                return null;
            }

            Url url = new(apiAccessOptions?.ApiUrl ?? ApiUrl);

            url.AppendPathSegment((apiAccessOptions?.ApiId ?? ApiId))
               .AppendPathSegment($"endpoint/{requestOptions.EndPoint}/" +
                                  $"{requestOptions.Version}" +
                                  $"/action/{ActionsEnum.DeleteMany.GetEnumDescription()}");

            IFlurlRequest request = null;

            if (string.IsNullOrEmpty(apiAccessOptions?.BearerToken) is false)
            {
                request = url.WithHeader("Authorization", $"Bearer {apiAccessOptions?.BearerToken}")
                             .WithHeader("Content-Type", "application/json");

            }
            else
            {
                request = url.WithHeader(ApiAuthOption, (apiAccessOptions?.ApiKey ?? ApiKey))
                             .WithHeader("Content-Type", "application/json");
            }

            request.BeforeCall(RewriteBodyAsync);

            response = request.PostJsonAsync(new
            {
                collection = Collection,
                database = DataBase,
                dataSource = DataSource,

                filter = filterOptions.Filter

            }, cancellationToken);

            return response;
        }


        /// <summary>
        /// Rewrites the call request body in order to be compliant with mongo data api syntax.
        /// </summary>
        /// <param name="call">The call.</param>
        private static async Task RewriteBodyAsync(FlurlCall call)
        {
            string? valueToRewrite = string.Empty;

            if (call?.HttpRequestMessage?.Content is not null)
                valueToRewrite = await call?.HttpRequestMessage?.Content?.ReadAsStringAsync()!;

            var contentReWrited = valueToRewrite.Replace("oid", "$oid").Replace("set", "$set").Replace("\"in\":", "\"$in\":");

            if (string.IsNullOrWhiteSpace(contentReWrited) || call is null)
                return;

            call.HttpRequestMessage.Content = new CapturedStringContent(contentReWrited);
        }
    }
}
