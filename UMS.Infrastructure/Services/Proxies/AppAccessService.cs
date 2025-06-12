using AW.Core.DTOs;
using AW.Infrastructure.Interfaces.Repositories;
using AW.Infrastructure.Interfaces.Services;
using AW.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Contexts;
using UMS.Core.DTOs;
using UMS.Infrastructure.Interfaces.Services.Proxies;

namespace UMS.Infrastructure.Services.Proxies
{
    public class AppAccessService : BaseService<UMSDbContext, AppAccessDto>, IAppAccessService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AppAccessService> _logger;
        private const string _baseEndPoint = "/api/v1/appaccess";
        private const double defaultMinute = 120;

        public AppAccessService(IBaseRepository<UMSDbContext, AppAccessDto> baseRepo, HttpClient httpClient, ICacheService cacheService, ILogger<AppAccessService> logger) : base(baseRepo)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _logger = logger;
        }

        public virtual async Task<MessageGetList<AppAccessDto>> GetAllDataProxy(QueryObject query, string additionalName = "")
        {
            string cacheKey = $"UMS-{_baseEndPoint}/page-with-disabled";
            if (string.IsNullOrEmpty(additionalName)) cacheKey += $"-{additionalName}";
            var cached = _cacheService.Get<MessageGetList<AppAccessDto>>(cacheKey);
            if (cached != null) return cached;

            try
            {
                string endPoint = $"{_baseEndPoint}/page-with-disabled";
                string token = _cacheService.GetAuthorization();
                if (token != null) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync(endPoint, new StringContent(System.Text.Json.JsonSerializer.Serialize(query), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    MessageGetList<AppAccessDto> messageGetList = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageGetList<AppAccessDto>>(content) ?? new MessageGetList<AppAccessDto>();
                    _cacheService.Set(cacheKey, messageGetList, TimeSpan.FromMinutes(defaultMinute));
                    return messageGetList;
                }
                return new MessageGetList<AppAccessDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to call App Module Service.");
            }
            return new MessageGetList<AppAccessDto>();
        }

        public virtual async Task<AppAccessDto?> GetByIdProxy(string Id, QueryObject query)
        {
            string cacheKey = $"UMS-{_baseEndPoint}/{Id}";
            var cached = _cacheService.Get<AppAccessDto>(cacheKey);
            if (cached != null) return cached;

            try
            {
                string endPoint = $"{_baseEndPoint}/{Id}";
                string token = _cacheService.GetAuthorization();
                if (token != null) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync(endPoint, new StringContent(System.Text.Json.JsonSerializer.Serialize(query), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    AppAccessDto? data = Newtonsoft.Json.JsonConvert.DeserializeObject<AppAccessDto>(content);
                    _cacheService.Set(cacheKey, data, TimeSpan.FromMinutes(defaultMinute));
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to call App Module Service.");
            }
            return null;
        }
    }
}
