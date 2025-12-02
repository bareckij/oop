using CourseManagement.Domain;
using System;
using System.Linq;

const string OnlineType = "Online";
const string OfflineType = "Offline";

var service = new CourseManagementService();
var isRunning = true;

Console.WriteLine("Система управления курсами университета");

while (isRunning)
{
	ShowMainMenu();
	Console.Write("Выберите действие: ");
	var choice = Console.ReadLine()?.Trim();

	try
	{
		switch (choice)
		{
			case "1":
				AddTeacher();
				break;
			case "2":
				AddCourse();
				break;
			case "3":
				RemoveCourse();
				break;
			case "4":
				AssignTeacherToCourse();
				break;
			case "5":
				EnrollStudentToCourse();
				break;
			case "6":
				ShowCoursesByTeacher();
				break;
			case "7":
				ShowCourses(service.GetAllCourses(), "Все курсы");
				break;
			case "8":
				ShowCourses(service.GetCoursesByType(OnlineType), "Онлайн курсы");
				break;
			case "9":
				ShowCourses(service.GetCoursesByType(OfflineType), "Офлайн курсы");
				break;
			case "0":
				isRunning = false;
				break;
			default:
				Console.WriteLine("Неизвестная команда. Попробуйте снова.");
				break;
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Ошибка: {ex.Message}");
	}
}

Console.WriteLine("Работа программы завершена.");

void ShowMainMenu()
{
	Console.WriteLine();
	Console.WriteLine("Меню:");
	Console.WriteLine("1. Добавить преподавателя");
	Console.WriteLine("2. Добавить курс");
	Console.WriteLine("3. Удалить курс");
	Console.WriteLine("4. Назначить преподавателя курсу");
	Console.WriteLine("5. Записать студента на курс");
	Console.WriteLine("6. Показать курсы преподавателя");
	Console.WriteLine("7. Показать все курсы");
	Console.WriteLine("8. Показать онлайн курсы");
	Console.WriteLine("9. Показать офлайн курсы");
	Console.WriteLine("0. Выход");
}

void AddTeacher()
{
	Console.Write("Введите ФИО преподавателя: ");
	var fullName = ReadNonEmptyLine();
	var teacher = service.RegisterTeacher(fullName);
	Console.WriteLine($"Преподаватель добавлен: {teacher.FullName} (ID: {teacher.Id})");
}

void AddCourse()
{
	Console.WriteLine("Выберите тип курса: 1 - онлайн, 2 - офлайн");
	Console.Write("Ваш выбор: ");
	var typeChoice = Console.ReadLine()?.Trim();

	Console.Write("Введите название курса: ");
	var title = ReadNonEmptyLine();

	Course course = typeChoice switch
	{
		"1" => CreateOnlineCourse(title),
		"2" => CreateOfflineCourse(title),
		_ => throw new InvalidOperationException("Неизвестный тип курса")
	};

	service.AddCourse(course);
	Console.WriteLine($"Курс добавлен: {course.Title} (ID: {course.Id})");
}

OnlineCourse CreateOnlineCourse(string title)
{
	Console.Write("Введите платформу проведения: ");
	var platform = ReadNonEmptyLine();
	return new OnlineCourse(title, platform);
}

OfflineCourse CreateOfflineCourse(string title)
{
	Console.Write("Введите аудиторию/локацию: ");
	var location = ReadNonEmptyLine();
	return new OfflineCourse(title, location);
}

void RemoveCourse()
{
	if (!service.GetAllCourses().Any())
	{
		Console.WriteLine("Курсы отсутствуют.");
		return;
	}

	ShowCourses(service.GetAllCourses(), "Доступные курсы");
	var courseId = PromptId("Введите ID курса для удаления: ");

	if (service.RemoveCourse(courseId))
	{
		Console.WriteLine("Курс удален.");
	}
	else
	{
		Console.WriteLine("Курс с указанным ID не найден.");
	}
}

void AssignTeacherToCourse()
{
	var courses = service.GetAllCourses();
	var teachers = service.GetAllTeachers();

	if (!courses.Any())
	{
		Console.WriteLine("Курсы отсутствуют.");
		return;
	}

	if (!teachers.Any())
	{
		Console.WriteLine("Нет зарегистрированных преподавателей.");
		return;
	}

	ShowCourses(courses, "Доступные курсы");
	var courseId = PromptId("Введите ID курса: ");

	ShowTeachers();
	var teacherId = PromptId("Введите ID преподавателя: ");

	service.AssignTeacherToCourse(courseId, teacherId);
	Console.WriteLine("Преподаватель назначен.");
}

void EnrollStudentToCourse()
{
	var courses = service.GetAllCourses();

	if (!courses.Any())
	{
		Console.WriteLine("Курсы отсутствуют.");
		return;
	}

	ShowCourses(courses, "Доступные курсы");
	var courseId = PromptId("Введите ID курса: ");

	Console.Write("Введите ФИО студента: ");
	var studentName = ReadNonEmptyLine();

	var student = new Student(studentName);
	service.EnrollStudent(courseId, student);
	Console.WriteLine("Студент записан на курс.");
}

void ShowCoursesByTeacher()
{
	if (!service.GetAllTeachers().Any())
	{
		Console.WriteLine("Нет зарегистрированных преподавателей.");
		return;
	}

	ShowTeachers();
	var teacherId = PromptId("Введите ID преподавателя: ");

	var courses = service.GetCoursesByTeacher(teacherId);

	if (!courses.Any())
	{
		Console.WriteLine("У этого преподавателя пока нет курсов.");
		return;
	}

	ShowCourses(courses, "Курсы преподавателя");
}

void ShowTeachers()
{
	var teachers = service.GetAllTeachers();

	Console.WriteLine("Преподаватели:");
	foreach (var teacher in teachers)
	{
		Console.WriteLine($"- {teacher.FullName} (ID: {teacher.Id})");
	}
}

void ShowCourses(System.Collections.Generic.IEnumerable<Course> courses, string header)
{
	var courseList = courses.ToList();

	Console.WriteLine(header + ":");
	if (!courseList.Any())
	{
		Console.WriteLine("- Нет доступных курсов");
		return;
	}

	foreach (var course in courseList)
	{
		PrintCourse(course);
	}
}

void PrintCourse(Course course)
{
	Console.WriteLine($"- {course.Title} [{course.CourseType}] (ID: {course.Id})");
	Console.WriteLine($"  Преподаватель: {(course.Teacher != null ? course.Teacher.FullName : "не назначен")}");

	switch (course)
	{
		case OnlineCourse online:
			Console.WriteLine($"  Платформа: {online.Platform}");
			break;
		case OfflineCourse offline:
			Console.WriteLine($"  Локация: {offline.Location}");
			break;
	}

	if (!course.Students.Any())
	{
		Console.WriteLine("  Студенты: нет записанных студентов");
	}
	else
	{
		var studentNames = string.Join(", ", course.Students.Select(s => $"{s.FullName} (ID: {s.Id})"));
		Console.WriteLine($"  Студенты: {studentNames}");
	}
}

int PromptId(string prompt)
{
	while (true)
	{
		Console.Write(prompt);
		var input = Console.ReadLine()?.Trim();
		if (int.TryParse(input, out var result) && result > 0)
		{
			return result;
		}

		Console.WriteLine("Некорректный формат идентификатора. Попробуйте снова.");
	}
}

string ReadNonEmptyLine()
{
	while (true)
	{
		var input = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(input))
		{
			return input.Trim();
		}

		Console.WriteLine("Значение не может быть пустым. Попробуйте снова:");
		Console.Write("> ");
	}
}
