using Console.Renderer;
using Console.Extension;
using Console.Command;
using Console.Annotation;

[CommandAttribute("Translate a text to a different language")]
public class LanguageCommand : ConsoleCommand {
    public LanguageCommand() : base(nameof(LanguageCommand)) { }

    public override ICommandResult Execute() {
        return this.UseResult(() => ConsolePrompt.GetPrompt()
            .DrawPromptOptions("Choose a language", this.Composer!.Names()));
    }
}
