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
	public class CompanyService : BaseService<UMSDbContext, CompanyDto>, ICompanyService
	{
		private readonly HttpClient _httpClient;
		private readonly ICacheService _cacheService;
		private readonly ILogger<CompanyService> _logger;
		private const string _baseEndPoint = "/api/v1/company";
		private const double defaultMinute = 120;

		public CompanyService(IBaseRepository<UMSDbContext, CompanyDto> baseRepo, HttpClient httpClient, ICacheService cacheService, ILogger<CompanyService> logger) : base(baseRepo)
		{
			_httpClient = httpClient;
			_cacheService = cacheService;
			_logger = logger;
		}

		public virtual async Task<MessageGetList<CompanyDto>> GetAllDataProxy(QueryObject query, string additionalName = "")
		{
			string cacheKey = $"UMS-{_baseEndPoint}/page-with-disabled";
			if (string.IsNullOrEmpty(additionalName)) cacheKey += $"-{additionalName}";
			var cached = _cacheService.Get<MessageGetList<CompanyDto>>(cacheKey);
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
					MessageGetList<CompanyDto> messageGetList = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageGetList<CompanyDto>>(content) ?? new MessageGetList<CompanyDto>();
					_cacheService.Set(cacheKey, messageGetList, TimeSpan.FromMinutes(defaultMinute));
					return messageGetList;
				}
				return new MessageGetList<CompanyDto>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to call Company Service.");
			}
			return new MessageGetList<CompanyDto>();
		}

		public virtual async Task<CompanyDto?> GetByIdProxy(string Id, QueryObject query)
		{
			string cacheKey = $"UMS-{_baseEndPoint}/{Id}";
			var cached = _cacheService.Get<CompanyDto>(cacheKey);
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
					CompanyDto? data = Newtonsoft.Json.JsonConvert.DeserializeObject<CompanyDto>(content);
					_cacheService.Set(cacheKey, data, TimeSpan.FromMinutes(defaultMinute));
					return data;
				}
				return null;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to call Company Service.");
			}
			return null;
		}
	}
}
