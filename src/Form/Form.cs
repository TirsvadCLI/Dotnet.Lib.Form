namespace TirsvadCLI.Form;

using TirsvadCLI.Form.Model;

public class Form
{
    private List<FormField> _fields;
    private string[] _inputs;
    private int _currentField;
    private int _maxLengthOfField;
    private int _cTop;
    private ConsoleColor _fieldColorFg = ConsoleColor.DarkCyan;
    private ConsoleColor _fieldColorBg = ConsoleColor.Black;
    private ConsoleColor _inputColorFg = ConsoleColor.DarkYellow;
    private ConsoleColor _inputColorBg = ConsoleColor.DarkBlue;
    private ConsoleColor _inputActiveColorFg = ConsoleColor.DarkYellow;
    private ConsoleColor _inputActiveColorBg = ConsoleColor.DarkBlue;
    public IEnumerable<string>? ErrorMessage { get; set; } = [];// Error message to display

    public Form(List<FormField> fields)
    {
        _fields = fields;
        _inputs = new string[fields.Count];

        for (int i = 0; i < fields.Count; i++)
            _inputs[i] = fields[i].Value;
    }

    public List<FormField> Run()
    {
        for (int i = 0; i < _fields.Count; i++)
        {
            if (_fields[i].Name.Length > _maxLengthOfField)
                _maxLengthOfField = _fields[i].Name.Length; // Find the maximum length of the fields
        }
        //DisplayForm();
        InputField();
        DisplayFinalInput();
        for (int i = 0; i < _fields.Count; i++)
        {
            _fields[i].Value = _inputs[i]; // Update the default value with the input
        }
        return _fields;
    }

    private void DisplayForm()
    {
        Console.Clear();

        if (ErrorMessage.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var line in ErrorMessage)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        _cTop = Console.CursorTop;

        for (int i = 0; i < _fields.Count; i++)
        {
            Console.ForegroundColor = _fieldColorFg;
            Console.BackgroundColor = _fieldColorBg;
            Console.Write($"{_fields[i].Name}: ");
            Console.ResetColor();

            if (i == _currentField)
            {
                Console.ForegroundColor = _inputActiveColorFg;
                Console.BackgroundColor = _inputActiveColorBg;
            }
            else
            {
                Console.ForegroundColor = _inputColorFg;
                Console.BackgroundColor = _inputColorBg;
            }

            string displayValue = _fields[i].IsPassword ? new string('*', _inputs[i]?.Length ?? 0) : _inputs[i];
            Console.CursorLeft = _maxLengthOfField + 2; // Move cursor to the right of the field name
            Console.Write(displayValue.PadRight(_fields[i].MaxLength, ' ')); // Fixed size input field
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine("Press F10 to save and exit, or ESC to cancel.");
    }

    private bool InputField()
    {
        while (true)
        {
            DisplayForm(); // Refresh view
            Console.SetCursorPosition(_maxLengthOfField + 2 + _inputs[_currentField].Length, _cTop + _currentField);
            Console.CursorVisible = true;
            ConsoleKeyInfo key = Console.ReadKey(true);
            Console.CursorVisible = false;

            switch (key.Key)
            {
                case ConsoleKey.F1:
                    Console.Clear();
                    Console.WriteLine("Help: Use arrow keys to navigate, Enter to select, Backspace to delete, F10 to save and exit, ESC to cancel.");
                    Console.ReadKey(true); // Wait for a key press
                    break;
                case ConsoleKey.F10:
                    return true; // Save and exit
                case ConsoleKey.Escape:
                    return false; // Cancel
                case ConsoleKey.Enter:
                case ConsoleKey.Tab:
                case ConsoleKey.DownArrow:
                    _currentField = (_currentField < _fields.Count - 1) ? _currentField + 1 : 0;
                    break;
                case ConsoleKey.UpArrow:
                    _currentField = (_currentField > 0) ? _currentField - 1 : _fields.Count - 1;
                    break;
                case ConsoleKey.Backspace:
                    if (_inputs[_currentField].Length > 0)
                    {
                        _inputs[_currentField] = _inputs[_currentField][..^1]; // Remove last character
                    }
                    break;
                default:
                    if (!char.IsControl(key.KeyChar) && _inputs[_currentField].Length < _fields[_currentField].MaxLength)
                    {
                        _inputs[_currentField] += key.KeyChar;
                    }
                    break;
            }
        }
    }

    public static string ReadLimitedInput(int maxLength)
    {
        string input = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;

            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar) && input.Length < maxLength)
            {
                input += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        }
        return input;
    }

    public static string ReadPassword(int maxLength)
    {
        string password = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar) && password.Length < maxLength)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
        return password;
    }

    private void DisplayFinalInput()
    {
        Console.Clear();
        Console.WriteLine("Final Input:");
        for (int i = 0; i < _fields.Count; i++)
            Console.WriteLine($"{_fields[i].Name}: {(_fields[i].IsPassword ? "********" : _inputs[i])}");
    }

    public void SetFieldColorFg(ConsoleColor color)
    {
        _fieldColorFg = color;
    }
    public void SetFieldColorBg(ConsoleColor color)
    {
        _fieldColorBg = color;
    }
    public void SetInputColorFg(ConsoleColor color)
    {
        _inputColorFg = color;
    }
    public void SetInputColorBg(ConsoleColor color)
    {
        _inputColorBg = color;
    }
    public void SetInputActiveColorFg(ConsoleColor color)
    {
        _inputActiveColorFg = color;
    }
    public void SetInputActiveColorBg(ConsoleColor color)
    {
        _inputActiveColorBg = color;
    }
    public void SetFieldColor(ConsoleColor fg, ConsoleColor bg)
    {
        _fieldColorFg = fg;
        _fieldColorBg = bg;
    }
    public void SetInputColor(ConsoleColor fg, ConsoleColor bg)
    {
        _inputColorFg = fg;
        _inputColorBg = bg;
    }

}
