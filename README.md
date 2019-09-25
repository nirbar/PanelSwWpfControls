Currently the collection only contains two control:

- EyePasswordBox: Password edit control with an eye-button to view the plain-text password.
  - Support binding to SecurePassword
  - Plain password is only stored temporarily while viewing it.
- WaitableButton: Button with "IsWaiting" property for use in commands that take time to complete.
