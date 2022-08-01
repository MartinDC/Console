using Console;
using Console.Renderer;

new Main().BootstrapApplication();

public class Main : ConsoleApp {
    public void BootstrapApplication() {
        WithSplash(new SplashScreen()).Run(new() {
            Commands = new() {
                [1] = new BusCommand(),
                [2] = new NameCommand(),
                [3] = new LanguageCommand().Compose(c => {
                    c.AddCommand(new IcelandicCommand());
                    c.AddCommand(new LatinCommand());
                }),
            }
        });
    }
}
