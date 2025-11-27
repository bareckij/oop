using System;
using System.Collections.Generic;
using System.Linq;
using CourseManagement.Models;

namespace CourseManagement.Services;

public class CourseManagementService
{
    private readonly Dictionary<int, Course> _courses = new();
    private readonly Dictionary<int, Teacher> _teachers = new();

    public Course AddCourse(Course course)
    {
        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        if (_courses.ContainsKey(course.Id))
        {
            throw new InvalidOperationException("Course with the same id already exists");
        }

        _courses.Add(course.Id, course);
        return course;
    }

    public bool RemoveCourse(int courseId)
    {
        return _courses.Remove(courseId);
    }

    public Course? GetCourseById(int courseId)
    {
        _courses.TryGetValue(courseId, out var course);
        return course;
    }

    public IReadOnlyCollection<Course> GetAllCourses()
    {
        return _courses.Values
            .OrderBy(c => c.Title)
            .ToList();
    }

    public IReadOnlyCollection<Course> GetCoursesByTeacher(int teacherId)
    {
        return _courses.Values
            .Where(c => c.Teacher?.Id == teacherId)
            .OrderBy(c => c.Title)
            .ToList();
    }

    public IReadOnlyCollection<Course> GetCoursesByType(string courseType)
    {
        if (string.IsNullOrWhiteSpace(courseType))
        {
            return Array.Empty<Course>();
        }

        return _courses.Values
            .Where(c => string.Equals(c.CourseType, courseType, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.Title)
            .ToList();
    }

    public void AssignTeacherToCourse(int courseId, int teacherId)
    {
        if (!_courses.TryGetValue(courseId, out var course))
        {
            throw new KeyNotFoundException("Course not found");
        }

        if (!_teachers.TryGetValue(teacherId, out var teacher))
        {
            throw new KeyNotFoundException("Teacher not found");
        }

        course.AssignTeacher(teacher);
    }

    public void EnrollStudent(int courseId, Student student)
    {
        if (!_courses.TryGetValue(courseId, out var course))
        {
            throw new KeyNotFoundException("Course not found");
        }

        course.EnrollStudent(student);
    }

    public IReadOnlyCollection<Teacher> GetAllTeachers()
    {
        return _teachers.Values
            .OrderBy(t => t.FullName)
            .ToList();
    }

    public Teacher? GetTeacherById(int teacherId)
    {
        _teachers.TryGetValue(teacherId, out var teacher);
        return teacher;
    }

    public Teacher RegisterTeacher(string fullName)
    {
        var teacher = new Teacher(fullName);
        AddTeacher(teacher);
        return teacher;
    }

    public void AddTeacher(Teacher teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        if (_teachers.ContainsKey(teacher.Id))
        {
            throw new InvalidOperationException("Teacher with the same id already exists");
        }

        _teachers.Add(teacher.Id, teacher);
    }
}
