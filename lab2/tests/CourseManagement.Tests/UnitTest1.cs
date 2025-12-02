using System;
using System.Linq;
using CourseManagement.Domain;

namespace CourseManagement.Tests;

public class UnitTest1
{
    [Fact]
    public void AddCourse_ShouldStoreCourse()
    {
        var service = new CourseManagementService();
        var course = new OnlineCourse("C#", "Teams");

        service.AddCourse(course);

        var storedCourse = service.GetCourseById(course.Id);

        Assert.NotNull(storedCourse);
        Assert.Equal(course.Id, storedCourse!.Id);
    }

    [Fact]
    public void AssignTeacherToCourse_ShouldAssignTeacher()
    {
        var service = new CourseManagementService();
        var course = new OfflineCourse("OOP", "Аудитория 101");
        var teacher = service.RegisterTeacher("Иван Иванов");
        service.AddCourse(course);

        service.AssignTeacherToCourse(course.Id, teacher.Id);

        Assert.Equal(teacher.Id, course.Teacher?.Id);
    }

    [Fact]
    public void EnrollStudent_ShouldAddStudent()
    {
        var service = new CourseManagementService();
        var course = new OnlineCourse("Design Patterns", "Zoom");
        service.AddCourse(course);
        var student = new Student("Петр Петров");

        service.EnrollStudent(course.Id, student);

        Assert.Contains(course.Students, s => s.Id == student.Id);
    }

    [Fact]
    public void EnrollStudent_ShouldNotAllowDuplicates()
    {
        var service = new CourseManagementService();
        var course = new OfflineCourse("Algorithms", "Аудитория 202");
        service.AddCourse(course);
        var student = new Student("Анна Смирнова");
        service.EnrollStudent(course.Id, student);

        Assert.Throws<InvalidOperationException>(() =>
        {
            service.EnrollStudent(course.Id, student);
        });
    }

    [Fact]
    public void GetCoursesByTeacher_ShouldReturnOnlyAssignedCourses()
    {
        var service = new CourseManagementService();
        var teacher = service.RegisterTeacher("Мария Соколова");
        var anotherTeacher = service.RegisterTeacher("Сергей Орлов");

        var courseA = new OnlineCourse("Databases", "Moodle");
        var courseB = new OfflineCourse("Networks", "Аудитория 303");
        service.AddCourse(courseA);
        service.AddCourse(courseB);

        service.AssignTeacherToCourse(courseA.Id, teacher.Id);
        service.AssignTeacherToCourse(courseB.Id, anotherTeacher.Id);

        var courses = service.GetCoursesByTeacher(teacher.Id);

        Assert.Single(courses);
        Assert.Equal(courseA.Id, courses.First().Id);
    }

    [Fact]
    public void RemoveCourse_ShouldRemoveAndReturnTrue()
    {
        var service = new CourseManagementService();
        var course = new OnlineCourse("Math", "Teams");
        service.AddCourse(course);

        var result = service.RemoveCourse(course.Id);

        Assert.True(result);
        Assert.Null(service.GetCourseById(course.Id));
    }
}
