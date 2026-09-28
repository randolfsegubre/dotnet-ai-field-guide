using System.ComponentModel;

namespace FieldGuide.Core.Chat.Tools;

/// <summary>
/// A deliberately simple, honest tool: an LLM's training data has a cutoff date, so it
/// cannot know today's real date or time on its own. This gives it a way to ask for the
/// truth instead of guessing.
/// </summary>
/// <remarks>
/// Registered with <c>AIFunctionFactory.Create</c>, which reads the method name and the
/// <see cref="DescriptionAttribute"/> values to build the JSON schema the model sees.
/// Keep the description plain and specific; the model chooses whether to call the tool
/// based on that text, not on the C# code.
/// </remarks>
public static class DateTimeTools
{
    [Description("Gets the current date and time in a specific IANA time zone, for example 'Asia/Manila' or 'UTC'.")]
    public static string GetCurrentDateTime(
        [Description("An IANA time zone id, for example 'Asia/Manila' or 'UTC'.")] string timeZoneId)
    {
        try
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, zone);
            return $"{now:yyyy-MM-dd HH:mm} ({timeZoneId})";
        }
        catch (TimeZoneNotFoundException)
        {
            // STEP: fail honestly rather than silently defaulting to UTC. A tool that
            // quietly returns the wrong answer is worse than one that says it can't.
            return $"Unknown time zone id: '{timeZoneId}'. Try 'Asia/Manila' or 'UTC'.";
        }
    }
}
