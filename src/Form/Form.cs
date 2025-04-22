namespace TirsvadCLI.Form;

using TirsvadCLI.Form.Model;

public class Form
{
    private List<FormField> _fields;
    private string[] _inputs;
    private int _currentField;
    private int _maxLengthOfField;
    private int _cTop;
    private int _cTopInoutFields;
    private ConsoleColor _fieldColorFg = ConsoleColor.DarkCyan;
    private ConsoleColor _fieldColorBg = ConsoleColor.Black;
    private ConsoleColor _inputColorFg = ConsoleColor.DarkYellow;
    private ConsoleColor _inputColorBg = ConsoleColor.DarkBlue;
    private ConsoleColor _inputActiveColorFg = ConsoleColor.DarkYellow;
    private ConsoleColor _inputActiveColorBg = ConsoleColor.DarkBlue;
    public ICollection<string>? ErrorMessage { get; set; } = []; // Error message to display.

    public Form(List<FormField> fields)
    {
        _fields = fields;
        _inputs = new string[fields.Count];
        ErrorMessage = [];

        for (int i = 0; i < fields.Count; i++)
            _inputs[i] = fields[i].Value;
    }

    public List<FormField> Run()
    {
        _cTop = Console.CursorTop; // Save the current cursor position.
        // Get the maximum length of the field names.
        for (int i = 0; i < _fields.Count; i++)
        {
            if (_fields[i].Name.Length > _maxLengthOfField)
                _maxLengthOfField = _fields[i].Name.Length; // Find the maximum length of the fields.
        }
        // The form and input fields logic.
        do
        {
            Console.SetCursorPosition(0, _cTop); // Set cursor position to the top of the console.
            // Clear from the current line to the bottom of the console.
            for (int i = _cTop; i < Console.WindowHeight; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth - 1));
            }
            Console.SetCursorPosition(0, _cTop); // Reset cursor position to the top.

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

            _cTopInoutFields = Console.CursorTop + 1; // Set the top position for input fields.

            DisplayForm();
        } while (!InputField());

        for (int i = 0; i < _fields.Count; i++)
        {
            _fields[i].Value = _inputs[i]; // Update the default value with the input.
        }
        return _fields;
    }

    private void DisplayForm()
    {
        for (int i = 0; i < _fields.Count; i++)
        {
            Console.SetCursorPosition(0, _cTopInoutFields + i); // Set cursor position for each field.
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
        Console.SetCursorPosition(_maxLengthOfField + 2 + _inputs[_currentField].Length, _cTopInoutFields + _currentField);
        Console.CursorVisible = true;
        ConsoleKeyInfo key = Console.ReadKey(true);
        Console.CursorVisible = false;

        switch (key.Key)
        {
            case ConsoleKey.F1:
                Console.Clear();
                Console.WriteLine("Help: Use arrow keys to navigate, Enter to select, Backspace to delete, F10 to save and exit, ESC to cancel.");
                Console.ReadKey(true); // Wait for a key press
                Console.SetCursorPosition(0, _cTop); // Reset cursor position to the top.
                return false;
            case ConsoleKey.F10:
                return true; // Save and exit
            case ConsoleKey.Escape:
                for (int i = 0; i < _fields.Count; i++)
                {
                    _inputs[i] = _fields[i].Value; // Restore original values
                }
                return true; // Cancel
            case ConsoleKey.Enter:
            case ConsoleKey.Tab:
            case ConsoleKey.DownArrow:
                _currentField = (_currentField < _fields.Count - 1) ? _currentField + 1 : 0;
                Console.SetCursorPosition(0, _cTop); // Reset cursor position to the top.
                return false;
            case ConsoleKey.UpArrow:
                _currentField = (_currentField > 0) ? _currentField - 1 : _fields.Count - 1;
                Console.SetCursorPosition(0, _cTop); // Reset cursor position to the top.
                return false;
            case ConsoleKey.Backspace:
                if (_inputs[_currentField].Length > 0)
                {
                    _inputs[_currentField] = _inputs[_currentField][..^1]; // Remove last character
                }
                Console.SetCursorPosition(0, _cTop); // Reset cursor position to the top.
                return false;
            default:
                if (!char.IsControl(key.KeyChar) && _inputs[_currentField].Length < _fields[_currentField].MaxLength)
                {
                    _inputs[_currentField] += key.KeyChar;
                }
                Console.SetCursorPosition(0, _cTop); // Reset cursor position to the top.
                return false;
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

    public void DisplayFinalInput()
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
