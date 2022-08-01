using Console.Renderer;
using Console.Extension;
using Console.Command;

public class LatinCommand : ConsoleCommand {
    public LatinCommand() : base(nameof(LatinCommand)) { }

    public override ICommandResult Execute() {
        return this.UseResult(() => ConsolePrompt.PromptForInputText("What do you want to translate?")).ForResult(result => {
            ConsolePrompt.GetRenderer().DrawText($"[yellow]Your text translated to latin: [/]");
            ConsolePrompt.GetRenderer().DrawText(Translate(result.StringValue));
        });
    }

    private string Translate(string? orginal) {
        if (orginal.HasValue() == false) { 
            return "[red]Failed to translate this text[/]";
        }
        return $"[green]{string.Join(' ', orginal!.Trim().Split(" ").Select(x => $"{x}us"))}[/]";
    }
}
