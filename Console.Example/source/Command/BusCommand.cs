using Console.Renderer;
using Console.Command;
using Console.Annotation;

[CommandAttribute(@$"This is the 'Bussen' exercise, a simple school project")]
public class BusCommand : ConsoleCommand {
    public BusCommand() : base(nameof(BusCommand)) { }

    public override ICommandResult Execute() {
        new Bussen().Run();
        return new ConsoleCommandResult { 
            Data = null
        };
    }
}

internal interface IOccupant { }
internal record Passenger(string? Firstname, string? Lastname, string? Gender, int? Age, int? Seat) : IOccupant {
    public string GenderText => (Gender??"M").Equals("M") ? "He" : "She";
    public static Passenger PromptPassenger() => new Passenger(
        ConsolePrompt.PromptForInputText("Firstname:"),
        ConsolePrompt.PromptForInputText("Lastname:"),
        ConsolePrompt.PromptForInputText("Gender:"),
        ConsolePrompt.PromptForInputNumber("Age:"),
        ConsolePrompt.PromptForInputNumber("Seat:")
    );
};

internal record Driver(string? Firstname, string? Lastname, string? Gender, int? Age, bool? License) : IOccupant {
    public string GenderText => (Gender??"M").Equals("M") ? "He" : "She";
    public static Driver PromptDriver() => new Driver(
        ConsolePrompt.PromptForInputText("Firstname:"),
        ConsolePrompt.PromptForInputText("Lastname:"),
        ConsolePrompt.PromptForInputText("Gender:"),
        ConsolePrompt.PromptForInputNumber("Age:"),
        true
    );
}

internal class Bussen {
    public int PassengerCount { get; set; } = default!;
    public IOccupant[] Passengers { get; set; } = default!;
    public Driver Driver { get; set; } = default!;

    public void Run() {
        RenderDivider("The Bus");
        PassengerCount = ConsolePrompt.PromptForInputNumber("What is the passenger capacity for this vehicle?") ?? 0;
        Passengers = new Passenger[PassengerCount];

        RenderDivider("The Driver");
        Driver = PromptDriver();
        RenderDriver(Driver);

        while(true) {
            TryAndLogGenericError(() => {
                var menuChoise = RenderMenu();
                switch (menuChoise) {
                    case 1: RenderDriver(Driver); break;
                    case 2: AddPassenger(); break;
                    case 3: ListPassenger(); break;
                    case 4: PassengerAge(true); break;
                    case 5: PassengerAge(false); break;
                    case 6: PokePassenger(); break;
                }
            });
        }
    }

    private int RenderMenu() {
        RenderDivider("The Menu");
        ConsolePrompt.GetRenderer().DrawText(string.Empty);
        ConsolePrompt.GetRenderer().DrawText("Please select a option:");
        ConsolePrompt.GetRenderer().DrawText("1) Driver info");
        ConsolePrompt.GetRenderer().DrawText("2) Add passenger");
        ConsolePrompt.GetRenderer().DrawText("3) Passenger info");
        ConsolePrompt.GetRenderer().DrawText("4) Oldest passenger");
        ConsolePrompt.GetRenderer().DrawText("5) Youngest passenger");
        ConsolePrompt.GetRenderer().DrawText("6) Poke passenger");

        return ConsolePrompt.PromptForInputNumber("[dim blue]Select an action ->[/]") ?? 0;
    }

    private void AddPassenger() {
        try {
            Passengers[Passengers.FindFirstEmptySeat()] = PromptPassenger();
            ConsolePrompt.GetRenderer().DrawText("[green] Your passenger is now seated in the bus! [/]");
        } catch(Exception e) {
            ConsolePrompt.GetRenderer().DrawText("[red] You need to enter proper values. Try again [/]");
            ConsolePrompt.GetRenderer().DrawText(e.Message);
        }
    }

    private void ListPassenger() {
        foreach(var passenger in Passengers) {
            RenderPassenger(passenger as Passenger);
        }
    }

    private void PassengerAge(bool oldest) {
        var passenger = oldest ? Passengers.FindOldestPassenger() : Passengers.FindYoungestPassenger();
        if(passenger is not null) { 
            ConsolePrompt.GetRenderer()
                .DrawText(@$"The {(oldest ? "oldest" : "youngest")} passenger is 
                    {passenger.Lastname} {passenger.Firstname}with a age of {passenger.Age}");
        }
    }

    private void PokePassenger() {
        var seat = ConsolePrompt.PromptForInputNumber("You want to poke one of your passengers - What seat does it occupy?") ?? 0;
        var passenger = Passengers.FindPassengerWithSeat(seat);
        if(passenger is not null) { 
            ConsolePrompt.GetRenderer().DrawText(@$"You poke {passenger.Lastname} {passenger.Firstname}. Afterwards {passenger.GenderText} complaint about pain in the arm!");
         }
    }

    private void RenderDriver(Driver driver) {
        ConsolePrompt.GetRenderer().DrawText($"Driving today is {driver.Firstname}, {driver.Lastname}");
        ConsolePrompt.GetRenderer().DrawText(driver.License == true ? $"Apparently, {driver.GenderText} got a licence" 
            : $"Sadly, {driver.GenderText} is missing a license");
    }

    private void RenderPassenger(Passenger? passenger) {
        if (passenger is null) { return; }
        ConsolePrompt.GetRenderer()
            .DrawText($"Passenger in seat {passenger.Seat} is [bold green]{passenger.Lastname}, {passenger.Firstname}  ({passenger.Gender})[/]");
    }

    private void RenderDivider(string message) {
        ConsolePrompt.GetRenderer().DrawText(string.Empty);
        ConsolePrompt.GetRenderer().DrawLine(message);
    }
 
    private static Passenger PromptPassenger(){
        ConsolePrompt.GetRenderer().DrawText("Please enter the info of the new passenger: ");
        return Passenger.PromptPassenger();
    }

    private static Driver PromptDriver() {
        ConsolePrompt.GetRenderer().DrawText("Please enter the info of the driver: ");
        return Driver.PromptDriver();
    }

    private void TryAndLogGenericError(Action action) {
        try {
            if (action is null) { 
                throw new ArgumentNullException("You have to pass an action to TryAndLogGenericError"); 
            }
            action.Invoke();
        } catch(Exception) {
            ConsolePrompt.GetRenderer().DrawText("[red]Something went wrong- Try again but do it right this time! [/]");
        }
    }
}

internal static class BussenFuncExtensions {
    public static Passenger? FindOldestPassenger(this IOccupant[] item) {
        int? age = 0;
        return item.LinearSearchPassengerWithPred(x => {
            if (x.Age > age) { age = x.Age; }
            return age <= x.Age;
        });
    }

    public static Passenger? FindYoungestPassenger(this IOccupant[] item) {
        int? age = 999;
        return item.LinearSearchPassengerWithPred(x => {
            if (x.Age < age) { age = x.Age; }
            return age >= x.Age;
        });
    }
    
    public static Passenger? FindPassengerWithSeat(this IOccupant[] item, int seat) {
        return item.LinearFindPassengerWithPred((x) => x.Seat == seat);
    }

    public static int FindFirstEmptySeat(this IOccupant[] item) {
        for (var i = item.GetLowerBound(0); i < item.GetUpperBound(0); i++) {
            if (item[i] is null) { return i; }
        }
        return int.MinValue;
    }

    public static Passenger? LinearSearchPassengerWithPred(this IOccupant[] item, Func<Passenger, bool> pred) {
        Passenger? passengerFound = null;
        for (var i = item.GetLowerBound(0); i < item.GetUpperBound(0); i++) {
            if (item[i] is not null && pred((Passenger)item[i])) { 
                passengerFound = (Passenger?)item[i];
            }
        }
        return passengerFound;
    }

    public static Passenger? LinearFindPassengerWithPred(this IOccupant[] item, Func<Passenger, bool> pred) {
        for (var i = item.GetLowerBound(0); i < item.GetUpperBound(0); i++) {
            if (pred((Passenger)item[i])) { return (Passenger?)item[i]; }
        }
        return null;
    }
}