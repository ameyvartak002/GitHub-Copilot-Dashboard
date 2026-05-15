using System.Text.Json.Serialization;

namespace CopilitDashboard.NET.Models
{
    // Models
    public class MetricsReport
    {
        [JsonPropertyName("download_links")]
        public List<string> DownloadLinks { get; set; } = new();

        [JsonPropertyName("report_start_day")]
        public string ReportStartDay { get; set; } = string.Empty;

        [JsonPropertyName("report_end_day")]
        public string ReportEndDay { get; set; } = string.Empty;
    }

    public class UserMetrics
    {
        [JsonPropertyName("report_start_day")]
        public string ReportStartDay { get; set; } = string.Empty;

        [JsonPropertyName("report_end_day")]
        public string ReportEndDay { get; set; } = string.Empty;

        [JsonPropertyName("day")]
        public string Day { get; set; } = string.Empty;

        [JsonPropertyName("enterprise_id")]
        public string EnterpriseId { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("user_login")]
        public string UserLogin { get; set; } = string.Empty;

        [JsonPropertyName("user_initiated_interaction_count")]
        public int UserInitiatedInteractionCount { get; set; }

        [JsonPropertyName("code_generation_activity_count")]
        public int CodeGenerationActivityCount { get; set; }

        [JsonPropertyName("code_acceptance_activity_count")]
        public int CodeAcceptanceActivityCount { get; set; }

        [JsonPropertyName("totals_by_ide")]
        public List<TotalsByIde> TotalsByIde { get; set; } = new();

        [JsonPropertyName("totals_by_feature")]
        public List<TotalsByFeature> TotalsByFeature { get; set; } = new();

        [JsonPropertyName("totals_by_language_feature")]
        public List<TotalsByLanguageFeature> TotalsByLanguageFeature { get; set; } = new();

        [JsonPropertyName("totals_by_language_model")]
        public List<TotalsByLanguageModel> TotalsByLanguageModel { get; set; } = new();

        [JsonPropertyName("totals_by_model_feature")]
        public List<TotalsByModelFeature> TotalsByModelFeature { get; set; } = new();

        [JsonPropertyName("used_agent")]
        public bool UsedAgent { get; set; }

        [JsonPropertyName("used_chat")]
        public bool UsedChat { get; set; }

        [JsonPropertyName("loc_suggested_to_add_sum")]
        public int LocSuggestedToAddSum { get; set; }

        [JsonPropertyName("loc_suggested_to_delete_sum")]
        public int LocSuggestedToDeleteSum { get; set; }

        [JsonPropertyName("loc_added_sum")]
        public int LocAddedSum { get; set; }

        [JsonPropertyName("loc_deleted_sum")]
        public int LocDeletedSum { get; set; }

        [JsonPropertyName("used_cli")]
        public bool UsedCli { get; set; }
    }

    public class TotalsByIde
    {
        [JsonPropertyName("ide")]
        public string Ide { get; set; } = string.Empty;

        [JsonPropertyName("user_initiated_interaction_count")]
        public int UserInitiatedInteractionCount { get; set; }

        [JsonPropertyName("code_generation_activity_count")]
        public int CodeGenerationActivityCount { get; set; }

        [JsonPropertyName("code_acceptance_activity_count")]
        public int CodeAcceptanceActivityCount { get; set; }

        [JsonPropertyName("loc_suggested_to_add_sum")]
        public int LocSuggestedToAddSum { get; set; }

        [JsonPropertyName("loc_suggested_to_delete_sum")]
        public int LocSuggestedToDeleteSum { get; set; }

        [JsonPropertyName("loc_added_sum")]
        public int LocAddedSum { get; set; }

        [JsonPropertyName("loc_deleted_sum")]
        public int LocDeletedSum { get; set; }

        [JsonPropertyName("last_known_plugin_version")]
        public PluginVersion LastKnownPluginVersion { get; set; } = new();

        [JsonPropertyName("last_known_ide_version")]
        public IdeVersion LastKnownIdeVersion { get; set; } = new();
    }

    public class PluginVersion
    {
        [JsonPropertyName("sampled_at")]
        public string SampledAt { get; set; } = string.Empty;

        [JsonPropertyName("plugin")]
        public string Plugin { get; set; } = string.Empty;

        [JsonPropertyName("plugin_version")]
        public string PluginVersionValue { get; set; } = string.Empty;
    }

    public class IdeVersion
    {
        [JsonPropertyName("sampled_at")]
        public string SampledAt { get; set; } = string.Empty;

        [JsonPropertyName("ide_version")]
        public string IdeVersionValue { get; set; } = string.Empty;
    }

    public class TotalsByFeature
    {
        [JsonPropertyName("feature")]
        public string Feature { get; set; } = string.Empty;

        [JsonPropertyName("user_initiated_interaction_count")]
        public int UserInitiatedInteractionCount { get; set; }

        [JsonPropertyName("code_generation_activity_count")]
        public int CodeGenerationActivityCount { get; set; }

        [JsonPropertyName("code_acceptance_activity_count")]
        public int CodeAcceptanceActivityCount { get; set; }

        [JsonPropertyName("loc_suggested_to_add_sum")]
        public int LocSuggestedToAddSum { get; set; }

        [JsonPropertyName("loc_suggested_to_delete_sum")]
        public int LocSuggestedToDeleteSum { get; set; }

        [JsonPropertyName("loc_added_sum")]
        public int LocAddedSum { get; set; }

        [JsonPropertyName("loc_deleted_sum")]
        public int LocDeletedSum { get; set; }
    }

    public class TotalsByLanguageFeature
    {
        [JsonPropertyName("language")]
        public string Language { get; set; } = string.Empty;

        [JsonPropertyName("feature")]
        public string Feature { get; set; } = string.Empty;

        [JsonPropertyName("code_generation_activity_count")]
        public int CodeGenerationActivityCount { get; set; }

        [JsonPropertyName("code_acceptance_activity_count")]
        public int CodeAcceptanceActivityCount { get; set; }

        [JsonPropertyName("loc_suggested_to_add_sum")]
        public int LocSuggestedToAddSum { get; set; }

        [JsonPropertyName("loc_suggested_to_delete_sum")]
        public int LocSuggestedToDeleteSum { get; set; }

        [JsonPropertyName("loc_added_sum")]
        public int LocAddedSum { get; set; }

        [JsonPropertyName("loc_deleted_sum")]
        public int LocDeletedSum { get; set; }
    }

    public class TotalsByLanguageModel
    {
        [JsonPropertyName("language")]
        public string Language { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("code_generation_activity_count")]
        public int CodeGenerationActivityCount { get; set; }

        [JsonPropertyName("code_acceptance_activity_count")]
        public int CodeAcceptanceActivityCount { get; set; }

        [JsonPropertyName("loc_suggested_to_add_sum")]
        public int LocSuggestedToAddSum { get; set; }

        [JsonPropertyName("loc_suggested_to_delete_sum")]
        public int LocSuggestedToDeleteSum { get; set; }

        [JsonPropertyName("loc_added_sum")]
        public int LocAddedSum { get; set; }

        [JsonPropertyName("loc_deleted_sum")]
        public int LocDeletedSum { get; set; }
    }

    public class TotalsByModelFeature
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("feature")]
        public string Feature { get; set; } = string.Empty;

        [JsonPropertyName("user_initiated_interaction_count")]
        public int UserInitiatedInteractionCount { get; set; }

        [JsonPropertyName("code_generation_activity_count")]
        public int CodeGenerationActivityCount { get; set; }

        [JsonPropertyName("code_acceptance_activity_count")]
        public int CodeAcceptanceActivityCount { get; set; }

        [JsonPropertyName("loc_suggested_to_add_sum")]
        public int LocSuggestedToAddSum { get; set; }

        [JsonPropertyName("loc_suggested_to_delete_sum")]
        public int LocSuggestedToDeleteSum { get; set; }

        [JsonPropertyName("loc_added_sum")]
        public int LocAddedSum { get; set; }

        [JsonPropertyName("loc_deleted_sum")]
        public int LocDeletedSum { get; set; }
    }
}
