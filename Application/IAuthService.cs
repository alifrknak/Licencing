﻿namespace Application;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
}
