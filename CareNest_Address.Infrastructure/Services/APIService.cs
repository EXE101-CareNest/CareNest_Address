using CareNest_Address.Application.Common;
using CareNest_Address.Application.Interfaces.Services;
using CareNest_Addressry.Application.Common.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CareNest_Address.Infrastructure.Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;

        private readonly APIServiceOption _option;


        public APIService(HttpClient httpClient, IOptions<APIServiceOption> option)
        {
            _httpClient = httpClient;
            _option = option.Value;
            _httpClient.BaseAddress = new Uri(option.Value.BaseUrlAccount);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ResponseResult<T>> GetAsync<T>(string serviceType, string endpoint)
        {
            try
            {
                string baseUrl = GetBaseUrl(serviceType);
                string fullUrl = $"{baseUrl}{endpoint}";

                HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    };
                    ApiResponse<T>? result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse, options);

                    if (result.Data == null)
                    {
                        return new ResponseResult<T>
                        {
                            IsSuccess = false,
                            Message = $"Not found: {response.ReasonPhrase}",
                            ErrorCode = (int)response.StatusCode
                        };
                    }
                    return new ResponseResult<T>(true, result, "Request successful.");
                }

                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"API Error: {response.ReasonPhrase}",
                    ErrorCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }
        public async Task<ResponseResult<T>> PostAsync<T>(string url, object data)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Nếu JSON sử dụng quy tắc camelCase
                        PropertyNameCaseInsensitive = true // Bỏ qua phân biệt chữ hoa chữ thường
                    };
                    ApiResponse<T>? result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse, options);
                    return new ResponseResult<T>(true, result, "Request successful.");
                }

                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"API Error: {response.ReasonPhrase}",
                    ErrorCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }
        public async Task<ResponseResult<T>> PutAsync<T>(string url, object data)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(url, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ApiResponse<T>? result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse);
                    return new ResponseResult<T>(true, result, "Request successful.");
                }

                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"API Error: {response.ReasonPhrase}",
                    ErrorCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }
        public async Task<ResponseResult<T>> DeleteAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ApiResponse<T>? result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse);
                    return new ResponseResult<T>(true, result, "Request successful.");
                }

                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"API Error: {response.ReasonPhrase}",
                    ErrorCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }



        public string GetBaseUrl(string serviceType)
        {
            return serviceType.ToLower() switch
            {
                "account" => _option.BaseUrlAccount,
                _ => throw new ArgumentException($"Service type '{serviceType}' không hợp lệ!", nameof(serviceType))
            };
        }
    }
}
