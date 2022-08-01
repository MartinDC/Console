using Console.Command;
using Console.Extension;

namespace Console.Renderer;

public class MenuTree {
    public List<ConsoleTreeNode> Nodes { get; set; } = default!;

    public List<ConsoleTreeNode> BuildTreeNodesFromMenuStructure(Dictionary<int, ICommand> menuStructure) {
        return Nodes = menuStructure.Values.Select(x => x.TranslateTreeNode()).ToList();
    }
}