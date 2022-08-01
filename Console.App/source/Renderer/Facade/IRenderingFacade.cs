using Spectre.Console;
using Spectre.Console.Rendering;

namespace Console.Renderer.Facade;

public interface IRenderingFacade {
    IRenderingFacade ChainingFunction(Func<IRenderable> action, bool newline = true) {
        AnsiConsole.Write(action.Invoke());
        if(newline) { AnsiConsole.WriteLine(); }
        return this;
    }
    
    IRenderingFacade DrawText(string markup, bool newline = true);
    IRenderingFacade DrawTree(string root, List<ConsoleTreeNode> nodes);
    IRenderingFacade DrawLine(string message = "", string? color = "yellow");
    IRenderingFacade DrawCenteredTable(string title, List<string> cols, List<string> rows);
    IRenderingFacade DrawTable(string title, List<string> cols, List<string> rows);
}
