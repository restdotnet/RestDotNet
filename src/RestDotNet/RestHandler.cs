﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RestDotNet.Converters;

namespace RestDotNet
{
    public class RestHandler : IRestHandler
    {
        private readonly Func<CancellationToken, Task<HttpResponseMessage>> _request;
        private readonly Dictionary<HttpStatusCode, Action<string>> _сallbacks;
        private readonly IJsonConverter _jsonConverter;

        public RestHandler(Func<CancellationToken, Task<HttpResponseMessage>> request,
            IJsonConverter jsonConverter)
        {
            _request = request;
            _jsonConverter = jsonConverter;
            _сallbacks = new Dictionary<HttpStatusCode, Action<string>>();
        }
        
        public void RegisterCallback(HttpStatusCode code, Action action) 
            => _сallbacks.Add(code, content => action());

        public void RegisterCallback<TReponse>(HttpStatusCode code, Action<TReponse> action) 
            => _сallbacks.Add(code, content => action(_jsonConverter.Deserialize<TReponse>(content)));

        public Task HandleAsync()
            => HandleAsync(CancellationToken.None);

        public async Task HandleAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage message = await _request(cancellationToken);
            string content = message.Content != null
                ? await message.Content.ReadAsStringAsync()
                : string.Empty;
            HttpStatusCode code = message.StatusCode;

            if (!_сallbacks.ContainsKey(code)) throw new UnhandledResponseException(code, content);
            _сallbacks[code](content);
        }
    }
}