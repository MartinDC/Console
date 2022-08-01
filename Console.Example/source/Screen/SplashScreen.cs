using Console.Renderer;
using Console.Screen;

public class SplashScreen : IScreen {
    
    public void Draw() {
        ConsolePrompt.GetRenderer().DrawText(Common.Art.HammerMan)
            .DrawText(string.Empty).DrawText(string.Empty);
    }
}