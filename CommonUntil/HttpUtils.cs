using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommonUntil
{
    public static class HttpUtils
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // 静态 Token 属性，用于存储当前 Token
        public static string AuthToken { get; set; }

        // 请求拦截器委托，允许自定义请求处理
        public static Action<HttpRequestMessage> RequestInterceptor { get; set; }

        static HttpUtils()
        {
            // 设置默认请求拦截器，添加 Token 到请求头
            RequestInterceptor = request =>
            {
                if (!string.IsNullOrEmpty(AuthToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthToken);
                }
            };
        }

        /*//帮我一一个关于HttP请求的Get方法
        public static string Get(string endpoint,Dictionary<string, string> queryParams = null)
        {
            try
            {
                // 1. 拼接查询参数到 URL
                if (queryParams != null && queryParams.Count > 0)
                {
                    // 转成 ?key1=value1&key2=value2 格式
                    var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
                    endpoint = $"{endpoint}?{queryString}";
                }

                HttpResponseMessage response = _httpClient.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode(); // 抛出异常如果 HTTP 状态码不是 2xx

                string responseBody = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<string>(responseBody);
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine($"GET 请求失败: {ex.Message}");
                return default;
            }
        }

        public static async Task<string> GetAsync(string endpoint, Dictionary<string, string> queryParams = null)
        {
            try
            {
                // 1. 拼接查询参数到 URL
                if (queryParams != null && queryParams.Count > 0)
                {
                    // 转成 ?key1=value1&key2=value2 格式
                    var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
                    endpoint = $"{endpoint}?{queryString}";
                }

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode(); // 抛出异常如果 HTTP 状态码不是 2xx

                string responseBody = await response.Content.ReadAsStringAsync();
                Log.WriteLog("Get返回请求：" + responseBody);

                return JsonConvert.DeserializeObject<string>(responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GET 请求失败: {ex.Message}");
                return default;
            }
            finally
            {
            }

        }*/

        // 发送 GET 请求
        public static string Get(string endpoint, Dictionary<string, string> queryParams = null)
            => SendRequestSync(HttpMethod.Get, endpoint, queryParams);

        // 异步发送 GET 请求
        public static async Task<string> GetAsync(string endpoint, Dictionary<string, string> queryParams = null)
            => await SendRequestAsync(HttpMethod.Get, endpoint, queryParams);

        // 发送 POST 请求
        public static string Post(string endpoint, object content = null, Dictionary<string, string> queryParams = null)
            => SendRequestSync(HttpMethod.Post, endpoint, queryParams, content);

        // 异步发送 POST 请求
        public static async Task<string> PostAsync(string endpoint, object content = null, Dictionary<string, string> queryParams = null)
            => await SendRequestAsync(HttpMethod.Post, endpoint, queryParams, content);

        // 发送 PUT 请求
        public static string Put(string endpoint, object content = null, Dictionary<string, string> queryParams = null)
            => SendRequestSync(HttpMethod.Put, endpoint, queryParams, content);

        // 异步发送 PUT 请求
        public static async Task<string> PutAsync(string endpoint, object content = null, Dictionary<string, string> queryParams = null)
            => await SendRequestAsync(HttpMethod.Put, endpoint, queryParams, content);

        // 发送 DELETE 请求
        public static string Delete(string endpoint, Dictionary<string, string> queryParams = null)
            => SendRequestSync(HttpMethod.Delete, endpoint, queryParams);

        // 异步发送 DELETE 请求
        public static async Task<string> DeleteAsync(string endpoint, Dictionary<string, string> queryParams = null)
            => await SendRequestAsync(HttpMethod.Delete, endpoint, queryParams);

        // 同步发送请求的通用方法
        private static string SendRequestSync(HttpMethod method, string endpoint, Dictionary<string, string> queryParams = null, object content = null)
        {
            try
            {
                // 构建完整 URL
                string fullUrl = BuildUrlWithQueryParams(endpoint, queryParams);

                // 创建请求消息
                var request = new HttpRequestMessage(method, fullUrl);

                // 设置请求内容
                if (content != null)
                {
                    string jsonContent = JsonConvert.SerializeObject(content);
                    request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }

                // 应用请求拦截器
                RequestInterceptor?.Invoke(request);

                // 发送请求
                using (var response = _httpClient.SendAsync(request).Result)
                {
                    // 检查 HTTP 状态码
                    if (!response.IsSuccessStatusCode)
                    {
                        // 读取错误响应内容
                        string errorContent =  response.Content.ReadAsStringAsync().Result;
                        ErrorUntil errorUntil = JsonConvert.DeserializeObject<ErrorUntil>(errorContent);
                        
                        // 记录错误日志
                        Log.WriteLog($"HTTP 请求失败 ({response.StatusCode}): {errorUntil.error}");

                        // 抛出异常
                        throw new HttpRequestException(errorUntil.error.message);
                    }

                    // 成功响应
                    string responseBody =  response.Content.ReadAsStringAsync().Result;
                    Log.WriteLog(method.ToString() + "返回请求：" + responseBody);
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{method} 请求失败: {ex.Message}");
                throw;
            }
        }

        // 异步发送请求的通用方法
        private static async Task<string> SendRequestAsync(HttpMethod method, string endpoint, Dictionary<string, string> queryParams = null, object content = null)
        {
            try
            {
                // 构建完整 URL
                string fullUrl = BuildUrlWithQueryParams(endpoint, queryParams);

                // 创建请求消息
                var request = new HttpRequestMessage(method, fullUrl);

                // 设置请求内容
                if (content != null)
                {
                    string jsonContent = JsonConvert.SerializeObject(content);
                    request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }

                // 应用请求拦截器
                RequestInterceptor?.Invoke(request);

                // 发送请求
                using (var response = await _httpClient.SendAsync(request))
                {
                    // 检查 HTTP 状态码
                    if (!response.IsSuccessStatusCode)
                    {
                        // 读取错误响应内容
                        string errorContent = await response.Content.ReadAsStringAsync();
                        ErrorUntil errorUntil = JsonConvert.DeserializeObject<ErrorUntil>(errorContent);

                        // 记录错误日志
                        Log.WriteLog($"HTTP 请求失败 ({response.StatusCode}): {errorUntil.error}");

                        // 抛出异常
                        throw new HttpRequestException(errorUntil.error.message);
                    }

                    // 成功响应
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Log.WriteLog(method.ToString() + "返回请求：" + responseBody);
                    return responseBody;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // 构建带查询参数的 URL
        private static string BuildUrlWithQueryParams(string endpoint, Dictionary<string, string> queryParams)
        {
            if (queryParams == null || queryParams.Count == 0)
                return endpoint;

            var queryString = string.Join("&", queryParams.Select(kv =>
                $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

            return $"{endpoint}?{queryString}";
        }
    


    }
}
