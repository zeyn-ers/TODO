using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

/// <summary>
/// TodoNote Service Implementation
/// </summary>
public class TodoNoteService : ITodoNoteService
{
    private readonly ITodoNoteRepository _todoNoteRepository;
    private readonly IMapper _mapper;

    public TodoNoteService(ITodoNoteRepository todoNoteRepository, IMapper mapper)
    {
        _todoNoteRepository = todoNoteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoNoteDto>> GetByTodoIdAsync(int todoId)
    {
        var notes = await _todoNoteRepository.GetByTodoIdAsync(todoId);
        return _mapper.Map<IEnumerable<TodoNoteDto>>(notes);
    }

    public async Task<TodoNoteDto> CreateAsync(int todoId, CreateTodoNoteDto dto)
    {
        var entity = new TodoNote
        {
            TodoId = todoId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _todoNoteRepository.AddAsync(entity);
        return _mapper.Map<TodoNoteDto>(created);
    }

    public async Task<bool> DeleteAsync(int todoId, int noteId)
    {
        var note = await _todoNoteRepository.GetByTodoIdAndNoteIdAsync(todoId, noteId);
        if (note == null) return false;

        await _todoNoteRepository.DeleteAsync(noteId);
        return true;
    }

    public async Task<TodoNoteDto?> UpdateAsync(int todoId, int noteId, UpdateTodoNoteDto dto)
    {
        var existing = await _todoNoteRepository.GetByTodoIdAndNoteIdAsync(todoId, noteId);
        if (existing == null) return null;

        existing.Content = dto.Content;
        await _todoNoteRepository.UpdateAsync(existing);
        
        return _mapper.Map<TodoNoteDto>(existing);
    }
}