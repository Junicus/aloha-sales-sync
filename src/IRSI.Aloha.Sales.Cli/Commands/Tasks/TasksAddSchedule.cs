using IRSI.Aloha.Sales.Cli.Application.Services;
using IRSI.Aloha.Sales.Cli.Options;
using Microsoft.Extensions.Options;
using Microsoft.Win32.TaskScheduler;
using Spectre.Console;
using Spectre.Console.Cli;

namespace IRSI.Aloha.Sales.Cli.Commands.Tasks;

public class TasksAddSchedule : Command
{
    private readonly IAnsiConsole _ansiConsole;
    private readonly TaskOptions _taskOptions;
    private readonly IEnvironment _environment;

    public TasksAddSchedule(IEnvironment environment, IOptions<TaskOptions> taskOptions, IAnsiConsole ansiConsole)
    {
        _environment = environment;
        _ansiConsole = ansiConsole;
        _taskOptions = taskOptions.Value;
    }

    public override int Execute(CommandContext context)
    {
        _ansiConsole.Status()
            .Spinner(Spinner.Known.Balloon)
            .Start("Waiting...", _ =>
            {
                using var taskService = new TaskService();
                var task = taskService.NewTask();
                task.RegistrationInfo.Description = "Aloha Sales CLI";
                task.RegistrationInfo.Author = "IRSI";
                task.RegistrationInfo.Source = "IRSI.Aloha.Sales.Cli";

                task.Triggers.Add(new DailyTrigger
                    { StartBoundary = DateTime.Today + TimeSpan.FromHours(6), DaysInterval = 1 });

                task.Actions.Add(new ExecAction(_environment.CurrentDirectory + "\\IRSI.Aloha.Sales.Cli.exe",
                    "sql sales",
                    _environment.CurrentDirectory));

                taskService.RootFolder.RegisterTaskDefinition("IRSI.Aloha.Sales.Cli", task, TaskCreation.CreateOrUpdate,
                    _taskOptions.UserName, _taskOptions.Password, TaskLogonType.Password);
            });

        return 0;
    }
}