
using System.Diagnostics;

namespace Calculator
{
    public partial class MainPage : ContentPage
    {

        List<string> calcList = new List<string>();
        string currentNumber = "";

        int result = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            calcList.Clear();
            showList(calcList);
        }

        private void OnParenthesesClicked(object sender, EventArgs e)
        {

        }

        private void OnPercentClicked(object sender, EventArgs e)
        {

        }

        private void OnOperatorClicked(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                calcList.Add(currentNumber);
                currentNumber = "";
                OutputEntry.Text += clickedButton.Text;
                calcList.Add(clickedButton.Text);
            }
        }

        private void OnDigitClicked(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                OutputEntry.Text += clickedButton.Text;
                currentNumber += clickedButton.Text;
            }
        }

        private void OnDecimalClicked(object sender, EventArgs e)
        {

        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {

        }

        private void OnEqualsClicked(object sender, EventArgs e)
        {
            
            Debug.WriteLine($"Result before: {result}");
            if (sender is Button clickedButton)
            {
                calcList.Add(currentNumber);
                currentNumber = "";
            }

            result = int.Parse(calcList[0]);

            Debug.WriteLine(calcList.Count);

            for (int i = 1; i < calcList.Count - 1; i+=2)
            {
                addToTheResult(calcList[i], calcList[i + 1]);
            }
            

            for (int i = 0; i < calcList.Count; i++)
            {
                Debug.WriteLine(calcList[i]);
            }
            OutputEntry.Text += $"={result}";
        }


        private int addToTheResult(string o, string strNumber)
        {
            int number = int.Parse(strNumber);

            if (o == "+")
            {
                return result += number;
            }
            else if (o == "-")
            {
                return result -= number;
            }
            else if (o == "X")
            {
                return result *= number;
            }
            else if (o == "/")
            {
                return result /= number;
            }
            else
            {
                return 0;
            }
        }

        private void showList<T>(List<T> list)
        {
            OutputEntry.Text = "";
            for (int i = 0; i < list.Count; i++)
            {
                OutputEntry.Text += list[i];
            }
        }


    }
}
