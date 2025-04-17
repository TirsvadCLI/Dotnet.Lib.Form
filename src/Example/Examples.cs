namespace Example;

using TirsvadCLI.Form;
using TirsvadCLI.Form.Model;

/// <summary>
/// This example show how to use From for console
/// It also show how to validate return input
/// </summary>
internal class Examples
{
    static string? _name;
    static int? _age;
    static string? _password;
    static List<string> _errorMessages = [];

    /// <summary>
    /// Simple example of how to use the form
    /// </summary>
    static void Example1()
    {
        var fields = new List<FormField>
           {
               new ("Name", _name ?? ""), // Added default value for name
               new ("Age", _age?.ToString() ?? "", 3), // Added default value for age and input size
               new ("Password", _password ?? "", 20, true) // Added default value for password and input size
           };

        var form = new Form(fields); // Create a new form with the fields
        var result = form.Run(); // Run the form and get the result

        // Loop through the fields, validate and set the values
        // Validation could be done in methods but for simplify it is done here
        foreach (var field in result)
        {
            switch (field.Name)
            {
                case "Name":
                    _name = field.Value;
                    break;
                case "Age":
                    if (int.TryParse(field.Value, out var parsedAge))
                        _age = parsedAge;
                    else
                        _errorMessages.Add("Age is an invalid age format. Please enter a number.");
                    _age = parsedAge;
                    break;
                case "Password":
                    _password = field.Value;
                    break;
            }
            // Check if any error messages were added
            if (_errorMessages.Count == 0)
                break; // Exit the loop if no errors
            else
                form.ErrorMessage = _errorMessages; // Set the error message to display
        }
    }

    static void Main(string[] args)
    {
        Example1(); // Call the example method
    }
}
