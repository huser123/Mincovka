using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MoneyCalculator
{
    public partial class MainWindow : Window
    {
        private readonly int[] euroNotes = { 50000, 20000, 10000, 5000, 2000, 1000, 500 }; // Bankjegyek
        private readonly int[] euroCoins = { 200, 100, 50, 20, 10, 5 }; // Érmék

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void AmountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Calculate();
            }
        }

        private void Calculate()
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount))
            {
                // Megjelenítjük a teljes beírt összeget
                TotalAmountTextBlock.Text = $"Beírt összeg: {amount}€";

                int remainingCents = (int)(amount * 100);
                var results = new List<MoneyBreakdown>();

                // Bankjegyek számolása
                foreach (var note in euroNotes)
                {
                    int count = remainingCents / note;
                    if (count > 0)
                    {
                        results.Add(new MoneyBreakdown { Denomination = $"{note / 100}€", Count = count });
                        remainingCents %= note;
                    }
                }

                // Érmék számolása
                foreach (var coin in euroCoins)
                {
                    int count = remainingCents / coin;
                    if (count > 0)
                    {
                        results.Add(new MoneyBreakdown { Denomination = $"{coin / 100.0}€", Count = count });
                        remainingCents %= coin;
                    }
                }

                // Maradék cent
                if (remainingCents > 0)
                {
                    results.Add(new MoneyBreakdown { Denomination = "cent", Count = remainingCents });
                }

                // Eredmény megjelenítése a DataGrid-ben
                ResultDataGrid.ItemsSource = results;
                AmountTextBox.Clear(); // Töröljük a szövegdoboz tartalmát
            }
            else
            {
                TotalAmountTextBlock.Text = "Érvénytelen összeg";
                ResultDataGrid.ItemsSource = null;
                AmountTextBox.Clear(); // Töröljük a szövegdoboz tartalmát hibás bevitel esetén is
            }
        }
    }

    public class MoneyBreakdown
    {
        public string Denomination { get; set; }
        public int Count { get; set; }
    }
}
