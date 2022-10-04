using WatchDog;
using WatchDog.src.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = true;
    // По умолчанию очистка каждую неделю
    opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Daily;
    // Все миграции уже в ядре пакета, поэтому нужно просто создать руками базу данных, к которой коннект прописан, и после просто запускать проект
    opt.SetExternalDbConnString = builder.Configuration.GetConnectionString("DbConnection");
    opt.SqlDriverOption = WatchDogSqlDriverEnum.PostgreSql;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseWatchDogExceptionLogger();
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "Pass123$";
});

app.Run();