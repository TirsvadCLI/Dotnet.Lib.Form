namespace TirsvadCLI.Form.Model;

public class FormField
{
    public string Name { get; set; }
    public bool IsPassword { get; set; } = false;
    public int MaxLength { get; set; }
    public string? Value { get; set; }

    public FormField(string name, string defaultValue = "", int maxLength = 20, bool isPassword = false)
    {
        Name = name;
        MaxLength = maxLength;
        Value = defaultValue;
        IsPassword = isPassword;
    }
}
