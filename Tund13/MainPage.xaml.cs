namespace Tund13;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object? sender, EventArgs e)
	{
		count++;

		// Loogika 1: Muuda teksti vastavalt arvule
		if (count == 1)
			CounterBtn.Text = $"Vajutatud {count} kord";
		else
			CounterBtn.Text = $"Vajutatud {count} korda";

		// Loogika 2: Pööra pilti iga vajutusega 15 kraadi
		BotImage.Rotation += 15;

		// Loogika 3: Muuda Labeli teksti
		CounterLabel.Text = $"Nuppu on vajutatud kokku: {count}";

		// Ülesanne 2: Juhuslik värv
		var random = new Random();
		var randomColor = Color.FromRgb(
			random.Next(0, 256), // Red
			random.Next(0, 256), // Green
			random.Next(0, 256)  // Blue
		);
		ResetBtn.BackgroundColor = randomColor;

		// Ülesanne 3: Elementide peitmine ja näitamine
		if (count >= 10)
		{
			BotImage.IsVisible = false; // Peidab pildi
			CounterLabel.Text = "Pilt kadus ära! Vajuta 'Tagasi nulli'.";
		}

		// Ülesanne 4: Nutikas nupu värv
		if (count >= 5)
		{
			CounterBtn.BackgroundColor = Colors.Red;
			CounterBtn.TextColor = Colors.White;
		}

		// Nurgad (CornerRadius): Muuda nupu nurgad ümaramaks, kui loendur on paarisarv
		if (count % 2 == 0)
			CounterBtn.CornerRadius = 10;
		else
			CounterBtn.CornerRadius = 0;

		// Ülesanne 6: Iseseisev uurimine (Properties) - Muudame pildi läbipaistvust
		BotImage.Scale += 0.05;

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	// UUS MEETOD: Reset nupu jaoks
	private void OnResetClicked(object? sender, EventArgs e)
	{
		count = 0;
		CounterBtn.Text = "Vajuta mind";
		CounterLabel.Text = "Alustame uuesti!";
		BotImage.Rotation = 0; // Pilt läheb otseks tagasi
		BotImage.IsVisible = true; // Toob pildi tagasi
		ResetBtn.BackgroundColor = Color.FromArgb("#512BD4"); // Taastame algse värvi

		// Ülesanne 4: Nupu värvide taastamine
		CounterBtn.BackgroundColor = Color.FromArgb("#512BD4");
		CounterBtn.TextColor = Colors.White;
		CounterBtn.CornerRadius = 0; // Taastame nurgad

		// Ülesanne 5: Elemendi asukoha muutmine (Toggle Start/End)
		if (BotImage.HorizontalOptions.Alignment == LayoutAlignment.Start)
		{
			BotImage.HorizontalOptions = LayoutOptions.End;
		}
		else
		{
			BotImage.HorizontalOptions = LayoutOptions.Start;
		}

		// Ülesanne 6: Taastame pildi läbipaistvuse ja suuruse
		BotImage.Scale = 1.0;
		MovingBot.TranslationY = 0; // Nullime ka liikuva roboti asukoha
		MovingBot.Opacity = 1.0; // Taastame läbipaistvuse
		UpBtn.IsEnabled = true; // Lubame nupud uuesti
		DownBtn.IsEnabled = true;
	}

	private async void OnUpClicked(object? sender, EventArgs e)
	{
		// Lisa: Muudame läbipaistvust
		MovingBot.Opacity -= 0.1;
		if (MovingBot.Opacity <= 0.1)
		{
			MovingBot.Opacity = 0;
			UpBtn.IsEnabled = false;
			DownBtn.IsEnabled = false;
		}

		// Hüppab üles ja põrkab tagasi (Bounce)
		await MovingBot.TranslateToAsync(0, -100, 200, Easing.CubicOut);
		await MovingBot.TranslateToAsync(0, 0, 500, Easing.BounceOut);
	}

	private async void OnDownClicked(object? sender, EventArgs e)
	{
		// Lisa: Muudame läbipaistvust
		MovingBot.Opacity -= 0.1;
		if (MovingBot.Opacity <= 0.1)
		{
			MovingBot.Opacity = 0;
			UpBtn.IsEnabled = false;
			DownBtn.IsEnabled = false;
		}

		// Hüppab korraks alla ja tagasi
		await MovingBot.TranslateToAsync(0, 100, 200, Easing.CubicOut);
		await MovingBot.TranslateToAsync(0, 0, 500, Easing.BounceOut);
	}
}
