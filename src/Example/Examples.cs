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
        form.ErrorMessage ??= new List<string>();
        do
        {
            Console.WriteLine("Example 1 with validation for age\n");
            var result = form.Run(); // Run the form and get the result
            form.ErrorMessage.Clear(); // Clear the error message list
            // Loop through the fields, validate and set the values
            // Validation could be done in methods but for simplify it is done here
            foreach (var field in result)
            {
                switch (field.Name)
                {
                    case "Name":
                        _name = _ = field.Value;
                        break;
                    case "Age":
                        if (int.TryParse(field.Value, out var parsedAge))
                            _age = _ = parsedAge;
                        else
                            form.ErrorMessage.Add("Age is an invalid age format. Please enter a number.");
                        _age = parsedAge;
                        break;
                    case "Password":
                        _password = _ = field.Value;
                        break;
                }
            }
            // Check if any error messages where added
            if (form.ErrorMessage.Count == 0)
                return; // Exit the loop if no errors
            Console.Clear(); // Clear the console if there are errors
        } while (true); // Loop until the form is valid
    }

    static void Main(string[] args)
    {
        Example1(); // Call the example method
    }
}
