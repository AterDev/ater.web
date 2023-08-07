﻿namespace Share;
internal class Const
{
    public const string PasswordRegex = @"^(?!\d+$).{6,60}$";
    public const string StrongPasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,60}$";
}
