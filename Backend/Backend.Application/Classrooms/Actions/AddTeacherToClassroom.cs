﻿using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;

public record AddTeacherToClassroom(int teacherId, int classroomId) : IRequest<ClassroomDto>;

public class AddTeacherToClassroomHandler : IRequestHandler<AddTeacherToClassroom, ClassroomDto>
{

    private readonly ITeacherRepository _teacherRepository;
    private readonly IClassroomRepository _classroomRepository;

    public AddTeacherToClassroomHandler( ITeacherRepository teacherRepository, IClassroomRepository classroomRepository)
    {
        _teacherRepository = teacherRepository;
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(AddTeacherToClassroom request, CancellationToken cancellationToken)
    {
        var teacher=_teacherRepository.GetById(request.teacherId);
        var classroom=_classroomRepository.GetById(request.classroomId);

        if (teacher == null)
        {
            throw new TeacherNotFoundException($"The teacher with id: {request.teacherId} was not found");
        }
        if( classroom == null )
        {
            throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
        }

        _classroomRepository.AddTeacher(teacher,classroom);
        //_classroomRepository.UpdateClassroom(classroom,classroom.ID);
        _teacherRepository.UpdateTeacher(teacher,teacher.ID);

        return Task.FromResult(ClassroomDto.FromClassroom(classroom));
    }
}
