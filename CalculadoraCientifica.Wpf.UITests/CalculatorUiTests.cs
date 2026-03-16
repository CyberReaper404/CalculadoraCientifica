using System.Diagnostics;
using System.IO;
using System.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculadoraCientifica.Wpf.UITests;

[TestClass]
[DoNotParallelize]
public sealed class CalculatorUiTests
{
    private Process? _process;
    private AutomationElement? _mainWindow;
    private string? _stateDirectory;

    [TestInitialize]
    public void TestInitialize()
    {
        _stateDirectory = Path.Combine(Path.GetTempPath(), "MMCalcCientifica.UITests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_stateDirectory);
        _process = StartApplication();
        _mainWindow = WaitForMainWindow(_process);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        try
        {
            if (_process is { HasExited: false })
            {
                _process.Kill(entireProcessTree: true);
                _process.WaitForExit(3000);
            }
        }
        catch
        {
            // Best effort cleanup for UI test runs.
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(_stateDirectory) && Directory.Exists(_stateDirectory))
            {
                Directory.Delete(_stateDirectory, recursive: true);
            }
        }
        catch
        {
            // Best effort cleanup for test state files.
        }
    }

    [TestMethod]
    public void Launch_ShouldShowInitialResult()
    {
        Assert.IsNotNull(_mainWindow);
        Assert.AreEqual("MM CALC Científica", _mainWindow.Current.Name);

        var resultText = FindByAutomationId("current_result_text");
        Assert.AreEqual("0", resultText.Current.Name);
    }

    [TestMethod]
    public void BasicAddition_ShouldUpdateDisplayAndHistory()
    {
        SetText("basic_first_input", "2");
        SetText("basic_second_input", "2");
        Click("basic_add_button");

        WaitForCondition(() => GetElementText("current_result_text") == "4");
        Assert.AreEqual("4", GetElementText("current_result_text"));
        Assert.AreEqual("2 + 2", GetElementText("last_expression_text"));
        Assert.AreEqual(1, GetHistoryItemCount());
    }

    [TestMethod]
    public void PowerOperation_ShouldCalculateExponentiation()
    {
        SetText("power_base_input", "2");
        SetText("power_exponent_input", "10");
        Click("power_button");

        WaitForCondition(() => GetElementText("current_result_text") == "1024");
        Assert.AreEqual("1024", GetElementText("current_result_text"));
        Assert.AreEqual("2^10", GetElementText("last_expression_text"));
    }

    [TestMethod]
    public void SquareRoot_ShouldCalculateValue()
    {
        SetText("root_input", "81");
        Click("square_root_button");

        WaitForCondition(() => GetElementText("current_result_text") == "9");
        Assert.AreEqual("9", GetElementText("current_result_text"));
        Assert.AreEqual("sqrt(81)", GetElementText("last_expression_text"));
    }

    [TestMethod]
    public void NaturalLog_ShouldReturnZero()
    {
        SelectTab("scientific_tab");
        SetText("log_input", "1");
        Click("natural_log_button");

        WaitForCondition(() => GetElementText("current_result_text") == "0");
        Assert.AreEqual("0", GetElementText("current_result_text"));
    }

    [TestMethod]
    public void CustomLog_ShouldUseProvidedBase()
    {
        SelectTab("scientific_tab");
        SetText("log_input", "8");
        SetText("log_base_input", "2");
        Click("custom_log_button");

        WaitForCondition(() => GetElementText("current_result_text") == "3");
        Assert.AreEqual("3", GetElementText("current_result_text"));
        Assert.AreEqual("log na base 2 de 8", GetElementText("last_expression_text"));
    }

    [TestMethod]
    public void SineInDegrees_ShouldReturnOne()
    {
        SelectTab("scientific_tab");
        SetText("trig_input", "90");
        Click("sine_button");

        WaitForCondition(() => GetElementText("current_result_text") == "1");
        Assert.AreEqual("1", GetElementText("current_result_text"));
        Assert.AreEqual("sen(90 graus)", GetElementText("last_expression_text"));
    }

    [TestMethod]
    public void SineInRadians_ShouldUseSelectedAngleUnit()
    {
        SelectComboBoxItem("angle_unit_combo", "Radianos");
        WaitForCondition(() => GetComboBoxSelection("angle_unit_combo") == "Radianos");
        SelectTab("scientific_tab");
        SetText("trig_input", "1.5707963267948966");
        Click("sine_button");

        WaitForCondition(() => GetElementText("current_result_text") == "1");
        Assert.AreEqual("1", GetElementText("current_result_text"));
        StringAssert.Contains(GetElementText("last_expression_text"), "rad");
    }

    [TestMethod]
    public void ComplexModulus_ShouldReturnFive()
    {
        SelectTab("complex_tab");
        SetText("complex_first_real_input", "3");
        SetText("complex_first_imaginary_input", "4");
        Click("complex_modulus_button");

        WaitForCondition(() => GetElementText("current_result_text") == "5");
        Assert.AreEqual("5", GetElementText("current_result_text"));
    }

    [TestMethod]
    public void ComplexAddition_ShouldCombineBothValues()
    {
        SelectTab("complex_tab");
        SetText("complex_first_real_input", "2");
        SetText("complex_first_imaginary_input", "3");
        SetText("complex_second_real_input", "4");
        SetText("complex_second_imaginary_input", "5");
        Click("complex_add_button");

        WaitForCondition(() => GetElementText("current_result_text") == "6 + 8i");
        Assert.AreEqual("6 + 8i", GetElementText("current_result_text"));
    }

    [TestMethod]
    public void ComplexPolar_ShouldRenderMagnitudeAndPhase()
    {
        SelectComboBoxItem("angle_unit_combo", "Radianos");
        SelectTab("complex_tab");
        SetText("complex_first_real_input", "0");
        SetText("complex_first_imaginary_input", "1");
        Click("complex_polar_button");

        WaitForCondition(() => GetElementText("current_result_text").Contains("fase"));
        StringAssert.Contains(GetElementText("current_result_text"), "módulo 1");
        StringAssert.Contains(GetElementText("current_result_text"), "rad");
    }

    [TestMethod]
    public void DivisionByZero_ShouldShowErrorFeedback()
    {
        SetText("basic_first_input", "10");
        SetText("basic_second_input", "0");
        Click("basic_divide_button");

        WaitForCondition(() => GetElementText("current_result_text") == "Erro");
        Assert.AreEqual("Erro", GetElementText("current_result_text"));
        StringAssert.Contains(GetElementText("status_text"), "Divisão por zero");
    }

    [TestMethod]
    public void SessionState_ShouldRestoreLatestResultAfterRelaunch()
    {
        SetText("basic_first_input", "7");
        SetText("basic_second_input", "8");
        Click("basic_add_button");

        WaitForCondition(() => GetElementText("current_result_text") == "15");
        RestartApplication();

        WaitForCondition(() => GetElementText("current_result_text") == "15");
        Assert.AreEqual("15", GetElementText("current_result_text"));
        Assert.AreEqual("7 + 8", GetElementText("last_expression_text"));
        Assert.AreEqual("Sessão anterior restaurada.", GetElementText("status_text"));
    }

    [TestMethod]
    public void ClearInputs_ShouldResetFieldsAndKeepHistory()
    {
        SetText("basic_first_input", "9");
        SetText("basic_second_input", "1");
        Click("basic_add_button");
        WaitForCondition(() => GetElementText("current_result_text") == "10");

        SetText("basic_first_input", "123");
        SetText("root_input", "64");
        Click("clear_inputs_button");

        WaitForCondition(() => GetElementText("current_result_text") == "0");
        Assert.AreEqual(string.Empty, GetTextBoxValue("basic_first_input"));
        Assert.AreEqual(string.Empty, GetTextBoxValue("root_input"));
        Assert.AreEqual("Entradas limpas", GetElementText("last_expression_text"));
        Assert.AreEqual(1, GetHistoryItemCount());
    }

    [TestMethod]
    public void ClearHistory_ShouldRemoveRegisteredOperations()
    {
        SetText("basic_first_input", "5");
        SetText("basic_second_input", "5");
        Click("basic_add_button");
        WaitForCondition(() => GetHistoryItemCount() == 1);

        Click("clear_history_button");

        WaitForCondition(() => GetHistoryItemCount() == 0);
        Assert.AreEqual("Histórico limpo", GetElementText("last_expression_text"));
        Assert.AreEqual("0", GetElementText("current_result_text"));
    }

    [TestMethod]
    public void AngleUnitSelection_ShouldPersistAcrossRelaunch()
    {
        SelectComboBoxItem("angle_unit_combo", "Radianos");
        Assert.AreEqual("Radianos", GetComboBoxSelection("angle_unit_combo"));

        RestartApplication();

        Assert.AreEqual("Radianos", GetComboBoxSelection("angle_unit_combo"));
    }

    [TestMethod]
    public void BasicTab_ShouldShowVerticalScrollWhenWindowIsReduced()
    {
        ResizeWindow(1080, 640);
        SelectTab("basic_tab");

        WaitForCondition(() => IsVerticallyScrollable("basic_tab_scroll_viewer"));
        Assert.IsTrue(IsVerticallyScrollable("basic_tab_scroll_viewer"));
    }

    [TestMethod]
    public void BasicTab_ShouldKeepLowerFieldsAccessibleAfterScroll()
    {
        ResizeWindow(1080, 640);
        SelectTab("basic_tab");
        ScrollToBottom("basic_tab_scroll_viewer");

        WaitForCondition(() => !FindByAutomationId("root_input").Current.IsOffscreen);
        SetText("root_input", "64");
        Click("square_root_button");

        WaitForCondition(() => GetElementText("current_result_text") == "8");
        Assert.AreEqual("8", GetElementText("current_result_text"));
    }

    [TestMethod]
    public void ScientificTab_ShouldShowVerticalScrollWhenWindowIsReduced()
    {
        ResizeWindow(1080, 640);
        SelectTab("scientific_tab");

        WaitForCondition(() => IsVerticallyScrollable("scientific_tab_scroll_viewer"));
        Assert.IsTrue(IsVerticallyScrollable("scientific_tab_scroll_viewer"));
    }

    [TestMethod]
    public void ComplexTab_ShouldShowVerticalScrollWhenWindowIsReduced()
    {
        ResizeWindow(1080, 640);
        SelectTab("complex_tab");

        WaitForCondition(() => IsVerticallyScrollable("complex_tab_scroll_viewer"));
        Assert.IsTrue(IsVerticallyScrollable("complex_tab_scroll_viewer"));
    }

    private Process StartApplication()
    {
        var executablePath = Environment.GetEnvironmentVariable("MM_CALC_EXE_PATH");

        if (string.IsNullOrWhiteSpace(executablePath))
        {
            executablePath = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "..",
                "CalculadoraCientifica.Wpf",
                "bin",
                "Debug",
                "net8.0-windows",
                "MMCalcCientifica.exe"));
        }

        if (!File.Exists(executablePath))
        {
            throw new FileNotFoundException($"Executável da calculadora não encontrado: {executablePath}");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            WorkingDirectory = Path.GetDirectoryName(executablePath) ?? AppContext.BaseDirectory,
            UseShellExecute = false
        };

        startInfo.Environment["MM_CALC_STATE_DIR"] = _stateDirectory ?? Path.Combine(Path.GetTempPath(), "MMCalcCientifica.UITests");

        return Process.Start(startInfo) ?? throw new InvalidOperationException("Não foi possível iniciar a aplicação WPF.");
    }

    private static AutomationElement WaitForMainWindow(Process process)
    {
        return WaitForCondition(() =>
        {
            process.Refresh();

            if (process.MainWindowHandle == IntPtr.Zero)
            {
                return null;
            }

            return AutomationElement.FromHandle(process.MainWindowHandle);
        }) ?? throw new AssertFailedException("A janela principal da calculadora não foi encontrada.");
    }

    private AutomationElement FindByAutomationId(string automationId)
    {
        Assert.IsNotNull(_mainWindow);

        return WaitForCondition(() =>
        {
            return _mainWindow.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
        }) ?? throw new AssertFailedException($"Elemento de automação não encontrado: {automationId}");
    }

    private void SetText(string automationId, string value)
    {
        var element = FindByAutomationId(automationId);

        if (!element.TryGetCurrentPattern(ValuePattern.Pattern, out var pattern))
        {
            throw new AssertFailedException($"Elemento não suporta ValuePattern: {automationId}");
        }

        ((ValuePattern)pattern).SetValue(string.Empty);
        ((ValuePattern)pattern).SetValue(value);
    }

    private void Click(string automationId)
    {
        var element = FindByAutomationId(automationId);

        if (!element.TryGetCurrentPattern(InvokePattern.Pattern, out var pattern))
        {
            throw new AssertFailedException($"Elemento não suporta InvokePattern: {automationId}");
        }

        ((InvokePattern)pattern).Invoke();
    }

    private void SelectTab(string automationId)
    {
        var element = FindByAutomationId(automationId);

        if (!element.TryGetCurrentPattern(SelectionItemPattern.Pattern, out var pattern))
        {
            throw new AssertFailedException($"A aba não suporta SelectionItemPattern: {automationId}");
        }

        ((SelectionItemPattern)pattern).Select();
    }

    private void SelectComboBoxItem(string comboAutomationId, string itemName)
    {
        var combo = FindByAutomationId(comboAutomationId);

        if (!combo.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out var expandPattern))
        {
            throw new AssertFailedException($"O ComboBox não suporta ExpandCollapsePattern: {comboAutomationId}");
        }

        ((ExpandCollapsePattern)expandPattern).Expand();

        var item = WaitForCondition(() =>
        {
            return combo.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.NameProperty, itemName));
        }) ?? throw new AssertFailedException($"Item do ComboBox não encontrado: {itemName}");

        if (!item.TryGetCurrentPattern(SelectionItemPattern.Pattern, out var selectionPattern))
        {
            throw new AssertFailedException($"Item do ComboBox não suporta SelectionItemPattern: {itemName}");
        }

        ((SelectionItemPattern)selectionPattern).Select();
        WaitForCondition(() => GetComboBoxSelection(comboAutomationId) == itemName);
    }

    private void RestartApplication()
    {
        Assert.IsNotNull(_process);

        if (_process is { HasExited: false })
        {
            _process.Kill(entireProcessTree: true);
            _process.WaitForExit(3000);
        }

        _process = StartApplication();
        _mainWindow = WaitForMainWindow(_process);
    }

    private void ResizeWindow(double width, double height)
    {
        Assert.IsNotNull(_mainWindow);

        if (!_mainWindow.TryGetCurrentPattern(TransformPattern.Pattern, out var pattern))
        {
            throw new AssertFailedException("A janela principal não suporta redimensionamento por automação.");
        }

        ((TransformPattern)pattern).Resize(width, height);
        Thread.Sleep(750);
    }

    private string GetElementText(string automationId)
    {
        var element = FindByAutomationId(automationId);
        return element.Current.Name;
    }

    private string GetTextBoxValue(string automationId)
    {
        var element = FindByAutomationId(automationId);

        if (!element.TryGetCurrentPattern(ValuePattern.Pattern, out var pattern))
        {
            throw new AssertFailedException($"Elemento não suporta ValuePattern: {automationId}");
        }

        return ((ValuePattern)pattern).Current.Value;
    }

    private int GetHistoryItemCount()
    {
        var historyList = FindByAutomationId("history_list");
        return historyList.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem)).Count;
    }

    private string GetComboBoxSelection(string automationId)
    {
        var combo = FindByAutomationId(automationId);

        if (combo.TryGetCurrentPattern(ValuePattern.Pattern, out var valuePattern))
        {
            return ((ValuePattern)valuePattern).Current.Value;
        }

        if (combo.TryGetCurrentPattern(SelectionPattern.Pattern, out var selectionPattern))
        {
            var selectedItems = ((SelectionPattern)selectionPattern).Current.GetSelection();
            if (selectedItems.Length > 0)
            {
                return selectedItems[0].Current.Name;
            }
        }

        return combo.Current.Name;
    }

    private bool IsVerticallyScrollable(string automationId)
    {
        var element = FindByAutomationId(automationId);

        if (element.TryGetCurrentPattern(ScrollPattern.Pattern, out var pattern))
        {
            return ((ScrollPattern)pattern).Current.VerticallyScrollable;
        }

        return FindVerticalScrollBar(element) is not null;
    }

    private void ScrollToBottom(string automationId)
    {
        var element = FindByAutomationId(automationId);

        if (!element.TryGetCurrentPattern(ScrollPattern.Pattern, out var pattern))
        {
            throw new AssertFailedException($"ScrollPattern não disponível: {automationId}");
        }

        var scrollPattern = (ScrollPattern)pattern;

        if (!scrollPattern.Current.VerticallyScrollable)
        {
            throw new AssertFailedException($"Rolagem vertical não disponível: {automationId}");
        }

        scrollPattern.SetScrollPercent(ScrollPattern.NoScroll, 100);
    }

    private static AutomationElement? FindVerticalScrollBar(AutomationElement element)
    {
        return element.FindFirst(
            TreeScope.Descendants,
            new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ScrollBar),
                new PropertyCondition(AutomationElement.OrientationProperty, OrientationType.Vertical)));
    }

    private static T? WaitForCondition<T>(Func<T?> action) where T : class
    {
        var timeout = DateTime.UtcNow.AddSeconds(15);

        while (DateTime.UtcNow < timeout)
        {
            var result = action();
            if (result is not null)
            {
                return result;
            }

            Thread.Sleep(200);
        }

        return null;
    }

    private static void WaitForCondition(Func<bool> predicate)
    {
        var timeout = DateTime.UtcNow.AddSeconds(15);

        while (DateTime.UtcNow < timeout)
        {
            if (predicate())
            {
                return;
            }

            Thread.Sleep(200);
        }

        throw new AssertFailedException("Condição não atendida dentro do tempo limite.");
    }
}
