using System;

namespace CourseManagement.Models;

public class Student
{
    private static int _nextId = 1;

    public Student(string fullName)
        : this(_nextId++, fullName)
    {
    }

    public Student(int id, string fullName)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Student id must be positive", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Student name cannot be empty", nameof(fullName));
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
