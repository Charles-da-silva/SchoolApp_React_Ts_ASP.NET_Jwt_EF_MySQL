// Usings necessários para trabalhar com ASP.NET MVC
using Microsoft.AspNetCore.Mvc;

// Usings para trabalhar com EF Core (LINQ, DbContext, etc.)
using Microsoft.EntityFrameworkCore;

// Importa o DbContext da aplicação
using School.Api.Infrastructure.Data;

// Importa a entidade Student
using School.Api.Domain.Entities;

namespace School.Api.Controllers
{
    // Indica que esta classe é um Controller de API. Diz ao ASP.NET: “Essa classe responde requisições HTTP e retorna JSON.”
    [ApiController]

    // Define a rota base:  /api/students
    // O que ele faz é substituir [controller] pelo nome da classe sem a palavra “Controller”.
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
         // Campo privado para armazenar o DbContext
        private readonly SchoolDbContext _context;
        
        /*
            CONSTRUTOR COM DEPENDENCY INJECTION (DI)

            O ASP.NET Core cria automaticamente uma instância de SchoolDbContext e injeta aqui para nós.

            Ou seja:
            - NÃO criamos o DbContext manualmente
            - NÃO usamos "new SchoolDbContext(...)"
            - O framework cuida do ciclo de vida
        */
        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        /*
            ENDPOINT: GET /api/students

            Objetivo:
            - Buscar todos os alunos ativos no banco
            - Retornar uma lista em formato JSON
        */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            /* _context.Students
                significa:
                    - Acessar a tabela Students;
                    - Trabalhar com objetos C#
                    - Sem escrever SQL diretamente */
                    
            // Busca todos os alunos ativos (IsActive = true)
            var students = await _context.Students
                .Where(s => s.IsActive)
                .ToListAsync();

            // Retorna HTTP 200 (OK) com os dados
            return Ok(students);
        }
    }
}
