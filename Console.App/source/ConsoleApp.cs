namespace Console;

using Console.Command;
using Console.Command.Exception;
using Console.Renderer;
using Console.Screen;
using Console.Extension;

public abstract class ConsoleApp {
    private CommandExecutor CommandExecutor { get; set; } = new();

    public ConsoleApp Run(ConsoleConfig config) {
        if (config?.Commands is null || config.Commands.Any() == false) {
            throw new ArgumentNullException(nameof(ConsoleConfig));
        }
        
        try {
            var selectedCommandName = string.Empty;
            if (config.RenderMenu) {
                selectedCommandName = ConsolePrompt.GetPrompt()
                    .DrawSelectionPrompt(config.MenuTitle, config.MenuDesc, config.Commands.TranslateMenuCommands());
            }

            CommandExecutor.Execute(selectedCommandName, config.Commands);
        } catch (NullReferenceException e) {
            ConsolePrompt.GetRenderer().DrawText(e.StackTrace!); throw;
        } catch (CommandException e) {
            ConsolePrompt.GetRenderer().DrawText(e.Message);
        }
        return this;
    }

    public ConsoleApp WithSplash(IScreen screen) {
        if (screen is not null) { screen.Draw(); }
        return this;
    }
}