# WPF Controls
- *EyePasswordBox*: Password edit control with an eye-button to view the plain-text password.
  - Support binding to SecurePassword
  - Plain password is only stored temporarily while viewing it.
- *WaitableButton*: Button with "IsWaiting" property for use in commands that take time to complete.
- *TextBoxWithButton*: Like for file path with browse button.

# WPF Converters
- *EqualityConverter*: Compare value to a supplied object collection and to ConverterParameter. All objects are expected to compare equal
- *InverseBooleanConverter*: Inverse boolean
- *IsInCollectionConverter*: Compare value to a supplied object collection and to ConverterParameter. Any of the objects is expected to compare equal
- *RegexConverter*: Apply regular expression on value

# WPF Commands
- *BrowseFileCommand*: Open an OpenFileDialog
- *BrowseFolderCommand* Open a FolderBrowserDialog