namespace TirsvadCLI.Tests;

using TirsvadCLI.Form;
using TirsvadCLI.Form.Model;

[TestClass]
public class FormTests
{
    [TestMethod]
    public void ErrorMessage_ShouldBeStoredCorrectly()
    {
        // Arrange
        var fields = new List<FormField> { new("Age", "invalid") };
        var form = new Form(fields);
        form.ErrorMessage = new List<string> { "Age must be a number." };

        // Assert
        Assert.AreEqual(1, form.ErrorMessage.Count());
        Assert.AreEqual("Age must be a number.", form.ErrorMessage.First());
    }
}
