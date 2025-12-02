using System;

namespace CourseManagement.Models;

public class OnlineCourse : Course
{
    public OnlineCourse(string title, string platform)
        : base(title)
    {
        if (string.IsNullOrWhiteSpace(platform))
        {
            throw new ArgumentException("Platform cannot be empty", nameof(platform));
        }

        Platform = platform.Trim();
    }

    public string Platform { get; }

    public override string CourseType => "Online";
}
