
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
        bool isOperator = false;
        bool isNumber = false;
        bool isResultOn = false;

        float result = 0;

        public MainPage()
        {
            InitializeComponent();

            styles.SelectedIndex = 0;
            /*
            int currentStyleIndex = Preferences.Get("CurrentStyleIndex", 0);
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null && currentStyleIndex >= 0 && currentStyleIndex < themes.Length)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(themes[currentStyleIndex]);
            }
            */
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
                //Preferences.Set("CurrentStyleIndex", selectedIndex);
            }

            //Debug.WriteLine(Preferences.Get("CurrentStyleIndex", 0));

        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            calcList.Clear();
            showList(calcList);
            OutputResult.Text = "";
            isResultOn = false;
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
                if (!isOperator)
                {
                    calcList.Add(clickedButton.Text);
                    showList(calcList);
                    isOperator = true;
                    isNumber = false;
                    isResultOn = false;
                    for (int i = 0; i < calcList.Count; i++)
                    {
                        Debug.Write($"[{calcList[i]}], ");
                    }
                    Debug.Write($"\ncalcList.Count = {calcList.Count}");
                }
            }
        }

        private void OnDigitClicked(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                int eqLength = calcList.Count;
                isOperator = false;
                isNumber = true;
                isResultOn = false;
                if (calcList.Count > 0 && float.TryParse(calcList[eqLength - 1], out float res))
                {
                    if (clickedButton.Text == "," && calcList[eqLength - 1].Contains(","))
                    {
                        return;
                    }
                    else
                    {
                        OutputEntry.Text += clickedButton.Text;
                        calcList[eqLength - 1] += clickedButton.Text;
                    }
                }
                else if (clickedButton.Text != ",")
                {
                    OutputEntry.Text += clickedButton.Text;
                    calcList.Add(clickedButton.Text);
                }
                for (int i = 0; i < eqLength; i++)
                {
                    Debug.Write($"[{calcList[i]}], ");
                }
                Debug.Write($"\ncalcList.Count = {calcList.Count}");
                
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (calcList.Count > 0 && calcList[calcList.Count - 1].Length > 1)
            {
                calcList[calcList.Count - 1] = calcList[calcList.Count - 1].Substring(0, calcList[calcList.Count - 1].Length - 1);
                showList(calcList);
            }
            else if (calcList.Count > 0)
            {
                calcList.RemoveAt(calcList.Count - 1);
                showList(calcList);
            }
        }

        private void OnEqualsClicked(object sender, EventArgs e)
        {
            if (!isResultOn)
            {
                calcList = sortOperation(calcList);

                if (float.TryParse(calcList[0], out float firstNum1))
                {
                    Debug.WriteLine("calcList[0] IS int");
                    result = firstNum1;

                    for (int i = 1; i < calcList.Count - 1; i += 2)
                    {
                        if(!addToTheResult(calcList[i], calcList[i + 1])) return;
                    }
                }
                else if (float.TryParse((calcList[0] + calcList[1]).ToString(), out float firstNum2))
                {
                    Debug.WriteLine("calcList[0] is NOT an int");
                    result = firstNum2;

                    for (int i = 2; i < calcList.Count - 1; i += 2)
                    {
                        if(!addToTheResult(calcList[i], calcList[i + 1])) return;
                    }
                }
                else Debug.WriteLine($"ERROR");




                for (int i = 0; i < calcList.Count; i++)
                {
                    Debug.WriteLine(calcList[i]);
                }
                OutputEntry.Text += " = ";
                OutputResult.Text = $"{result}";

                isResultOn = true;
            }
        }


        private bool addToTheResult(string o, string strNumber)
        {
            float number = float.Parse(strNumber);
            Debug.WriteLine($"Number: {number}");

            if (o == "+")
            {
                result += number;
            }
            else if (o == "-")
            {
                result -= number;
            }
            else if (o == "X")
            {
                result *= number;
            }
            else if (o == "/" && number != 0)
            {
                result /= number;
            }
            else
            {
                OutputResult.Text = "ERROR";
                return false;
            }
            return true;
        }

        private void showList<T>(List<T> list)
        {
            OutputEntry.Text = "";
            for (int i = 0; i < list.Count; i++)
            {
                OutputEntry.Text += list[i] + " ";
            }
        }

        private List<string> sortOperation(List<string> list) // ["2", "X", "4"]
        {
            for (int i = 0; i < list.Count; i++)
            {
                Debug.WriteLine($"list object: [{list[i]}]");
            }
            List<string> sortedEq = new List<string>();
            if (list.Count > 3)
            {
                Debug.WriteLine($"list.Count = {list.Count}");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == "X" || list[i] == "/")
                    {
                        if (i > 1 && list[i - 2] == "-") sortedEq.Add(list[i - 2]);
                        if (float.TryParse(list[i - 1], out float v1) && float.TryParse(list[i + 1], out float v2))
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
