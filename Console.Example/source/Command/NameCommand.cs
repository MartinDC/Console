using Console.Renderer;
using Console.Extension;
using Console.Command;
using Console.Annotation;

[CommandAttribute("A simple command to register a name")]
public class NameCommand : ConsoleCommand {
    public NameCommand() : base(nameof(NameCommand)) { }

    public override ICommandResult Execute() {
        return this.UseResult(() => ConsolePrompt.PromptForInputText("Please input your name?")).ForResult(result => {
            ConsolePrompt.GetRenderer().DrawText($"Got it, your name is {result.Data}");
        });
    }
}
