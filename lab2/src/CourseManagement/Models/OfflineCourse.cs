using System;

namespace CourseManagement.Models;

public class OfflineCourse : Course
{
    public OfflineCourse(string title, string location)
        : base(title)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location cannot be empty", nameof(location));
        }

        Location = location.Trim();
    }

    public string Location { get; }

    public override string CourseType => "Offline";
}
