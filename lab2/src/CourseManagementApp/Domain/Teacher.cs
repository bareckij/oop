using System;

namespace CourseManagement.Domain;

public class Teacher
{
    private static int _nextId = 1;

    public Teacher(string fullName)
        : this(_nextId++, fullName)
    {
    }

    public Teacher(int id, string fullName)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Teacher id must be positive", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Teacher name cannot be empty", nameof(fullName));
        }

        Id = id;
        FullName = fullName.Trim();
    }

    public int Id { get; }

    public string FullName { get; }

    public override string ToString()
    {
        return FullName;
    }
}
