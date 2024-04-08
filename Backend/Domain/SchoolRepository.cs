﻿using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using Backend.Application.Abstractions;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.Placeholders;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.TeacherException;

namespace Backend.Infrastructure;
public class SchoolRepository : ISchoolRepository
{

    private readonly List<School> _schools = new();
    //Db mock
    public void AddClassroom(Classroom classroom, School school)
    {
        if (classroom == null)
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(AddClassroom), false);
            throw new NullClassroomException($"Classroom is not valid");
        }
        else if (school.Classrooms.Contains(classroom))
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(AddClassroom), false);
            throw new ClassroomAlreadyRegisteredException($"Classroom {classroom.Name} is already registered");
        }
        Logger.LogMethodCall(nameof(AddClassroom), true);
        school.Classrooms.Add(classroom);
    }


    public void RemoveClassroom(Classroom classroom, School school)
    {
        if (classroom == null)
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(RemoveClassroom), false);

            throw new NullClassroomException("Classroom is not valid");
        }
        else if (!school.Classrooms.Contains(classroom))
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(RemoveClassroom), false);
            throw new ClassroomNotRegisteredException($"Classroom {classroom.Name} cannot be deleted because it is not registered");
        }
        Logger.LogMethodCall(nameof(RemoveClassroom), true);
        school.Classrooms.Remove(classroom);
    }

    public int GetLastId()
    {
        if (_schools.Count == 0) return 1;
        var lastId = _schools.Max(s => s.ID);
        return lastId + 1;
    }

    public School? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _schools.FirstOrDefault(s => s.ID == id);
    }

    public School Create(School school)
    {
        _schools.Add(school);
        Logger.LogMethodCall(nameof(Create), true);
        return school;
    }

    public void UpdateSchool(School school, int id)
    {
        var oldSchool = GetById(id);
        if (oldSchool == null)
        {
            Logger.LogMethodCall(nameof(UpdateSchool), false);
            throw new TeacherNotFoundException($"School with id {id} not found");
        }
        //implement mapper
        oldSchool.Name = school.Name;
        oldSchool.Classrooms = school.Classrooms;
        Logger.LogMethodCall(nameof(UpdateSchool), true);
    }

    public void Delete(School school)
    {
        _schools.Remove(school);
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public School Update(int schoolId, School school)
    {
        var oldSchool = _schools.FirstOrDefault(s => s.ID == schoolId);
        if (oldSchool != null)
        {
            oldSchool = school;

            return oldSchool;
        }
        else
        {
            throw new StudentNotFoundException($"The student with id: {schoolId} was not found");
        }
    }

    public IEnumerable<School> GetAll()
    {
        return _schools;
    }
}

