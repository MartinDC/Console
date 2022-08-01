using Console.Renderer;
using Console.Extension;
using Console.Command;

public class IcelandicCommand : ConsoleCommand {
    public string Translation { get; set; } = default!;

    public IcelandicCommand() : base(nameof(IcelandicCommand)) { }

    public override ICommandResult Execute() {
        return this.UseResult(() => ConsolePrompt.PromptForInputText("What do you want to translate?")).ForResult(result => {
            ConsolePrompt.GetRenderer().DrawText($"Your text translated to icelandic: ");
            ConsolePrompt.GetRenderer().DrawText(Translate(result.StringValue));
        });
    }

    private string Translate(string? orginal) {
        if (orginal is null || string.IsNullOrEmpty(orginal)) { 
            return "[red]Failed to translate this text[/]";
        }
        return $"[green]{string.Join(' ', orginal.Trim().Split(" ").Select(x => $"{x}ur"))}[/]";
    }
}
