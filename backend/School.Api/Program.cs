// Namespace principal do ASP.NET Core
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Namespace do Entity Framework Core
using Microsoft.EntityFrameworkCore;

// Nosso DbContext
using School.Api.Infrastructure.Data;

// Cria o "builder" da aplicação.
// Ele é responsável por:
// - Ler configurações
// - Registrar serviços (Dependency Injection)
// - Preparar o pipeline HTTP
var builder = WebApplication.CreateBuilder(args);

/// ===============================
/// REGISTRO DE SERVIÇOS (Injeção de Dependência)
/// ===============================

// Usamos o builder.Services para registrar serviços na aplicação,
// registrando tudo o que a aplicação usa: Controllers, Banco, Autenticação, Services, Cache

// Adiciona suporte a Controllers (MVC Web API).
// Sem isso, o ASP.NET NÃO reconhece classes Controller.
builder.Services.AddControllers();

// Registra serviços necessários para o Swagger/OpenAPI.
// Isso permite documentar e testar a API via navegador.
builder.Services.AddEndpointsApiExplorer();

// Swagger (Swashbuckle) - documentação da API
builder.Services.AddSwaggerGen();

// =======================
// CONFIGURAÇÃO DO EF CORE
// =======================
//
// Aqui estamos dizendo:
// - Use o SchoolDbContext
// - Use PostgreSQL como banco
// - Use a connection string chamada "DefaultConnection"
//
builder.Services.AddDbContext<SchoolDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ); // GetConnectionString -> Lê o appsettings.json / permite trocar banco sem mudar código
});

/// ===============================
/// BUILD DA APLICAÇÃO
/// ===============================

// Aqui a aplicação é efetivamente construída.
// Depois disso, não é mais possível registrar serviços.
var app = builder.Build();

/// ===============================
/// CONFIGURAÇÃO DO PIPELINE HTTP (middlewares)
/// ===============================

// Em ambiente de desenvolvimento:
// - habilitamos Swagger
// - habilitamos a UI do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Gera o JSON do Swagger

    // Interface gráfica do Swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "School API v1");
    });
}

// Força redirecionamento HTTP → HTTPS
// Boa prática de segurança.
app.UseHttpsRedirection();

// Autorização (vamos usar quando entrar JWT)
app.UseAuthorization();

// Mapeia os Controllers.
// Isso faz com que:
// - [ApiController]
// - [HttpGet], [HttpPost], etc.
// comecem a funcionar.
app.MapControllers();

/// ===============================
/// START DA APLICAÇÃO
/// ===============================

// Inicia o servidor web / Sobe a aplicação
app.Run();

