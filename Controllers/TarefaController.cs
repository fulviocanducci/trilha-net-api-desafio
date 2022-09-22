using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TarefaController : ControllerBase
  {
    private readonly OrganizadorContext _context;

    public TarefaController(OrganizadorContext context)
    {
      _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
      Tarefa tarefa = await _context.Tarefas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      if (tarefa == null)
      {
        return NotFound();
      }
      return Ok(tarefa);
    }

    [HttpGet("ObterTodos")]
    public async Task<IActionResult> ObterTodos()
    {
      List<Tarefa> tarefas = await _context.Tarefas.AsNoTracking().ToListAsync();
      return Ok(tarefas);
    }

    [HttpGet("ObterPorTitulo")]
    public async Task<IActionResult> ObterPorTitulo(string titulo)
    {
      List<Tarefa> tarefas = await _context.Tarefas.AsNoTracking().Where(x => x.Titulo.Contains(titulo)).ToListAsync();
      return Ok(tarefas);
    }

    [HttpGet("ObterPorData")]
    public async Task<IActionResult> ObterPorData(DateTime data)
    {
      List<Tarefa> tarefas = await _context.Tarefas.AsNoTracking().Where(x => x.Data.Date == data.Date).ToListAsync();
      return Ok(tarefas);
    }

    [HttpGet("ObterPorStatus")]
    public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
    {
      List<Tarefa> tarefas = await _context.Tarefas.Where(x => x.Status == status).AsNoTracking().ToListAsync();
      return Ok(tarefas);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Tarefa tarefa)
    {
      if (tarefa == null)
        return BadRequest(new { Erro = "A tarefa esta vazia" });

      if (tarefa.Data == DateTime.MinValue)
        return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

      await _context.Tarefas.AddAsync(tarefa);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
    {
      Tarefa tarefaBanco = _context.Tarefas.Find(id);

      if (tarefaBanco == null)
        return NotFound();

      if (tarefa.Data == DateTime.MinValue)
        return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

      tarefaBanco.Data = tarefa.Data;
      tarefaBanco.Descricao = tarefa.Descricao;
      tarefaBanco.Status = tarefa.Status;
      tarefaBanco.Titulo = tarefa.Titulo;
      await _context.SaveChangesAsync();

      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
      Tarefa tarefaBanco = _context.Tarefas.Find(id);

      if (tarefaBanco == null)
        return NotFound();

      _context.Tarefas.Remove(tarefaBanco);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
