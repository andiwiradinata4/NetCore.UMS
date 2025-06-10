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
    public class AppSystemService : BaseService<UMSDbContext, AppSystemDto>, IAppSystemService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AppSystemService> _logger;
        private const string _baseEndPoint = "/api/v1/appsystem";
        private const double defaultMinute = 120;

        public AppSystemService(IBaseRepository<UMSDbContext, AppSystemDto> baseRepo, HttpClient httpClient, ICacheService cacheService, ILogger<AppSystemService> logger) : base(baseRepo)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _logger = logger;
        }

        public virtual async Task<MessageGetList<AppSystemDto>> GetAllDataProxy(QueryObject query, string additionalName = "")
        {
            string cacheKey = $"UMS-{_baseEndPoint}/page-with-disabled";
            if (string.IsNullOrEmpty(additionalName)) cacheKey += $"-{additionalName}";
            var cached = _cacheService.Get<MessageGetList<AppSystemDto>>(cacheKey);
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
                    MessageGetList<AppSystemDto> messageGetList = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageGetList<AppSystemDto>>(content) ?? new MessageGetList<AppSystemDto>();
                    _cacheService.Set(cacheKey, messageGetList, TimeSpan.FromMinutes(defaultMinute));
                    return messageGetList;
                }
                return new MessageGetList<AppSystemDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to call App System Service.");
            }
            return new MessageGetList<AppSystemDto>();
        }

        public virtual async Task<AppSystemDto?> GetByIdProxy(string Id, QueryObject query)
        {
            string cacheKey = $"UMS-{_baseEndPoint}/{Id}";
            var cached = _cacheService.Get<AppSystemDto>(cacheKey);
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
                    AppSystemDto? data = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSystemDto>(content);
                    _cacheService.Set(cacheKey, data, TimeSpan.FromMinutes(defaultMinute));
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to call App System Service.");
            }
            return null;
        }
    }
}
