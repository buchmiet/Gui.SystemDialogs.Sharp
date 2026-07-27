using System.Windows.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;

namespace Gui.SystemDialogs.Wpf.Sharp.SmokeTests;

internal static class CommonFileDialogAutomation
{
    public static T RunWithDialogAutomation<T>(
        string windowTitle,
        Func<T> showDialog,
        Action<Window> automate,
        TimeSpan? timeout = null)
    {
        var wait = timeout ?? TimeSpan.FromSeconds(20);
        Exception? automationError = null;
        Exception? dialogError = null;
        var resultBox = new StrongBox<T>();
        var automationReady = new ManualResetEventSlim(false);
        var dialogFinished = new ManualResetEventSlim(false);

        var automationThread = new Thread(() =>
        {
            try
            {
                using var automation = new UIA3Automation();
                automationReady.Set();

                var window = WaitForWindow(automation, windowTitle, wait);
                automate(window);
            }
            catch (Exception ex)
            {
                automationError = ex;
            }
        })
        {
            IsBackground = true,
            Name = "Gui.SystemDialogs.Smoke.Automation"
        };

        var staThread = new Thread(() =>
        {
            try
            {
                // Pump enough for Win32 common dialogs owned by this thread.
                _ = Dispatcher.CurrentDispatcher;
                if (!automationReady.Wait(wait))
                {
                    throw new TimeoutException("UI automation thread did not become ready.");
                }

                // Brief pause so the waiter is actively polling before ShowDialog blocks.
                Thread.Sleep(150);
                resultBox.Value = showDialog();
            }
            catch (Exception ex)
            {
                dialogError = ex;
            }
            finally
            {
                dialogFinished.Set();
            }
        })
        {
            IsBackground = true,
            Name = "Gui.SystemDialogs.Smoke.STA"
        };
        staThread.SetApartmentState(ApartmentState.STA);

        automationThread.Start();
        staThread.Start();

        if (!dialogFinished.Wait(wait + TimeSpan.FromSeconds(10)))
        {
            throw new TimeoutException($"Timed out waiting for dialog '{windowTitle}' to finish.");
        }

        automationThread.Join(TimeSpan.FromSeconds(5));

        if (automationError is not null)
        {
            throw new InvalidOperationException(
                $"UI automation failed for dialog '{windowTitle}'.",
                automationError);
        }

        if (dialogError is not null)
        {
            throw new InvalidOperationException(
                $"Dialog call failed for '{windowTitle}'.",
                dialogError);
        }

        return resultBox.Value!;
    }

    public static void Cancel(Window dialog)
    {
        var cancel = FindButton(dialog, automationId: "2")
                     ?? throw new InvalidOperationException("Cancel button not found.");
        cancel.Invoke();
    }

    public static void ConfirmOpenOrSave(Window dialog)
    {
        var confirm = FindButton(dialog, automationId: "1")
                      ?? throw new InvalidOperationException("Open/Save button not found.");
        confirm.Invoke();
    }

    public static void SetFileName(Window dialog, string fileName)
    {
        var edit = FindFileNameEdit(dialog)
                   ?? throw new InvalidOperationException("File name edit control not found.");

        edit.Focus();
        edit.Click();
        Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.KEY_A);
        Keyboard.Type(fileName);
        Wait.UntilInputIsProcessed();
    }

    private static Window WaitForWindow(
        UIA3Automation automation,
        string title,
        TimeSpan timeout)
    {
        var result = Retry.WhileNull(
            () =>
            {
                var desktop = automation.GetDesktop();
                return desktop.FindFirstChild(cf =>
                    cf.ByControlType(ControlType.Window).And(cf.ByName(title)));
            },
            timeout: timeout,
            interval: TimeSpan.FromMilliseconds(100),
            throwOnTimeout: false,
            ignoreException: true);

        if (result.Result is null)
        {
            throw new TimeoutException(
                $"Timed out waiting for dialog window titled '{title}'.");
        }

        return result.Result.AsWindow();
    }

    private static Button? FindButton(AutomationElement root, string automationId)
    {
        return root.FindFirstDescendant(cf =>
                cf.ByControlType(ControlType.Button).And(cf.ByAutomationId(automationId)))
            ?.AsButton();
    }

    private static TextBox? FindFileNameEdit(AutomationElement root)
    {
        // Classic common dialog: AutomationId 1148 (combo) + nested Edit.
        var classic = root.FindFirstDescendant(cf => cf.ByAutomationId("1148"));
        var classicEdit = classic?.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit))
            ?.AsTextBox();
        if (classicEdit is not null)
        {
            return classicEdit;
        }

        // Newer common dialog: FileNameControlHost.
        var host = root.FindFirstDescendant(cf => cf.ByAutomationId("FileNameControlHost"));
        return host?.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit))?.AsTextBox();
    }

    private sealed class StrongBox<T>
    {
        public T? Value { get; set; }
    }
}
