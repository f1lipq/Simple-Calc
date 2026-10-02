
using Calculator.Resources.Styles.Themes;
using System.Diagnostics;

namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        ResourceDictionary[] themes =
        {
            new DarkTheme(),
            new LightTheme(),
            new OrangeTheme(),
            new BlueTheme(),
            new GreenTheme(),
            new MacOsTheme()
        };

        List<string> calcList = new List<string>();
        string currentNumber = "";

        int result = 0;

        public MainPage()
        {
            InitializeComponent();

            styles.SelectedIndex = 0;
        }

        private void ChangeStyle(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null && selectedIndex >= 0 && selectedIndex < themes.Length)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(themes[selectedIndex]);
            }

        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            calcList.Clear();
            showList(calcList);
            OutputResult.Text = "";
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
                OutputEntry.Text += " " + clickedButton.Text + " ";
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

            calcList = sortOperation(calcList);

            if (int.TryParse(calcList[0], out int firstNum1))
            {
                result = firstNum1;

                for (int i = 1; i < calcList.Count - 1; i += 2)
                {
                    addToTheResult(calcList[i], calcList[i + 1]);
                }
            }
            else if (int.TryParse(calcList[0] + calcList[1], out int firstNum2))
            {
                result = firstNum2;

                for (int i = 2; i < calcList.Count - 1; i += 2)
                {
                    addToTheResult(calcList[i], calcList[i + 1]);
                }
            }
            else result = 99999;

            for (int i = 0; i < calcList.Count; i++)
            {
                Debug.WriteLine(calcList[i]);
            }
            OutputEntry.Text += " = ";
            OutputResult.Text = $"{result}";
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

        private List<string> sortOperation(List<string> list) // ["1", "+", "4", "X" "2"]
        {
            List<string> sortedEq = new List<string>();
            if (list.Count > 3)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == "X" || list[i] == "/")
                    {
                        if (i > 1 && list[i - 2] == "-") sortedEq.Add(list[i - 2]);
                        if (int.TryParse(list[i - 1], out int v1) && int.TryParse(list[i + 1], out int v2))
                        {
                            if (list[i] == "X") sortedEq.Add((v1 * v2).ToString());
                            else if (list[i] == "/") sortedEq.Add((v1 / v2).ToString());
                        }
                        sortedEq.Add("+");
                        if (i > 1) { list.RemoveRange(i - 2, 4); i -= 4; }
                        else { list.RemoveRange(i - 1, 3); i -= 3; }

                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                sortedEq.Add(list[i]);
            }

            deleteEqChar(sortedEq);

            return sortedEq;
        }

        private void deleteEqChar(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i] == "+" && list[i + 1] == "-") || (list[i] == "-" && list[i + 1] == "+"))
                {
                    list[i + 1] = "-";
                    list.RemoveAt(i);
                    i -= 1;
                }

                else if ((list[i] == "-" && list[i + 1] == "-") || (list[i] == "+" && list[i + 1] == "+"))
                {
                    list.RemoveAt(i);
                    i -= 1;
                }
            }
        }
    }
}
