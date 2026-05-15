using CopilitDashboard.NET.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CopilitDashboard.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CopilotController : ControllerBase
    {
        static Dictionary<string, UserMetrics> dict_userMetricsList = new Dictionary<string, UserMetrics>();
        static DateTime dte_LastFetchedDate = DateTime.MinValue;

        public CopilotController()
        {
            if (dict_userMetricsList.Any())
                return;

            var applicationStarupPath = AppDomain.CurrentDomain.BaseDirectory;
            var directoryPath = Path.Combine(applicationStarupPath, "MetricsReports");

            if (Directory.Exists(directoryPath))
            {
                Console.WriteLine($"{DateTime.Now} > Loading previous analytics from file.");

                var latestFile = Directory.GetFiles(directoryPath)
                    .Select(f => new { FilePath = f, CreationTime = System.IO.File.GetCreationTime(f) })
                    .OrderByDescending(f => f.CreationTime)
                    .FirstOrDefault();

                if (latestFile is not null)
                {
                    var deserializedData = JsonSerializer.Deserialize<Dictionary<string, UserMetrics>>(System.IO.File.ReadAllText(latestFile.FilePath));
                    if (deserializedData != null)
                    {
                        foreach (var kvp in deserializedData)
                        {
                            dict_userMetricsList.TryAdd(kvp.Key, kvp.Value);

                            // Update the last fetched date based on the report end day in the metrics
                            dte_LastFetchedDate = kvp.Value.ReportEndDay != null && DateTime.TryParse(kvp.Value.ReportEndDay, out var reportEndDate)
                                ? reportEndDate > dte_LastFetchedDate ? reportEndDate : dte_LastFetchedDate
                                : dte_LastFetchedDate;
                        }
                    }
                }

                Console.WriteLine($"{DateTime.Now} > Total [{dict_userMetricsList.Keys.Count}] items loaded from files and Last Fetched Date is set to {dte_LastFetchedDate}");
            }
        }

        [HttpGet("seats")]
        public async Task<IActionResult> GetSeatsAsync()
        {
            var GitHubToken = Environment.GetEnvironmentVariable("NS_GITHUB_API_TOKEN");
            var GitHubEnterprise = Environment.GetEnvironmentVariable("NS_GITHUB_ENTERPRISE");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GitHubToken);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.51.0");

            var seatsUrl = $"https://api.github.com/enterprises/{GitHubEnterprise}/copilot/billing";
            var response = await httpClient.GetAsync(seatsUrl);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch seats data.");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            int totalSeats = root.TryGetProperty("seat_breakdown", out var breakdown)
                && breakdown.TryGetProperty("total", out var total)
                ? total.GetInt32()
                : 0;

            return Ok(new { total_seats = totalSeats });
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetCopilotMetricsAsync()
        {
            if (dte_LastFetchedDate.Date.AddDays(1) != DateTime.Now.Date)
            {
                var GitHubEnterprise = Environment.GetEnvironmentVariable("NS_GITHUB_ENTERPRISE");
                var GitHubToken = Environment.GetEnvironmentVariable("NS_GITHUB_API_TOKEN");

                string MetricsUrl = $"https://api.github.com/enterprises/{GitHubEnterprise}/copilot/metrics/reports/users-28-day/latest";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GitHubToken);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.51.0");

                // 1. Get the main metrics report
                var metricsResponse = await httpClient.GetAsync(MetricsUrl);
                if (!metricsResponse.IsSuccessStatusCode)
                    return StatusCode((int)metricsResponse.StatusCode, "Failed to fetch metrics report.");

                var metricsJson = await metricsResponse.Content.ReadAsStringAsync();
                var metrics = JsonSerializer.Deserialize<MetricsReport>(metricsJson);

                if (metrics?.DownloadLinks == null || metrics.DownloadLinks.Count == 0)
                    return BadRequest("No download links found in metrics report.");

                // 2. For each download link, fetch and parse line-delimited JSON
                foreach (var link in metrics.DownloadLinks)
                {
                    using var httpClientDownload = new HttpClient();
                    var downloadResponse = await httpClientDownload.GetAsync(link);
                    if (!downloadResponse.IsSuccessStatusCode)
                        continue;

                    var downloadContent = await downloadResponse.Content.ReadAsStringAsync();
                    using var reader = new StringReader(downloadContent);
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var userMetrics = JsonSerializer.Deserialize<UserMetrics>(line);
                        if (userMetrics != null)
                            dict_userMetricsList.TryAdd($"{userMetrics.Day}|{userMetrics.UserLogin}", userMetrics);
                    }
                }

                var applicationStarupPath = AppDomain.CurrentDomain.BaseDirectory;
                var directoryPath = Path.Combine(applicationStarupPath, "MetricsReports");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                System.IO.File.WriteAllText(Path.Combine(directoryPath, DateTime.Now.ToString("yyyy-MM-dd") + ".json"), JsonSerializer.Serialize(dict_userMetricsList, new JsonSerializerOptions { WriteIndented = true }));

                dte_LastFetchedDate = DateTime.Now;
            }

            return Ok(dict_userMetricsList.Values);
        }
    }
}
