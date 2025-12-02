using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseManagement.Domain;

public abstract class Course
{
    private readonly List<Student> _students = new();

    private static int _nextId = 1;

    protected Course(string title)
        : this(_nextId++, title)
    {
    }

    protected Course(int id, string title)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Course id must be positive", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Course title cannot be empty", nameof(title));
        }

        Id = id;
        Title = title.Trim();
    }

    public int Id { get; }

    public string Title { get; private set; }

    public Teacher? Teacher { get; private set; }

    public IReadOnlyCollection<Student> Students => _students.AsReadOnly();

    public abstract string CourseType { get; }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException("Course title cannot be empty", nameof(newTitle));
        }

        Title = newTitle.Trim();
    }

    public void AssignTeacher(Teacher teacher)
    {
        Teacher = teacher ?? throw new ArgumentNullException(nameof(teacher));
    }

    public void RemoveTeacher()
    {
        Teacher = null;
    }

    public void EnrollStudent(Student student)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (_students.Any(s => s.Id == student.Id))
        {
            throw new InvalidOperationException("Student already enrolled");
        }

        _students.Add(student);
    }

    public bool RemoveStudent(int studentId)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        if (student == null)
        {
            return false;
        }

        _students.Remove(student);
        return true;
    }
}
