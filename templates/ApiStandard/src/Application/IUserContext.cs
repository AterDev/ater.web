﻿using Microsoft.AspNetCore.Http;

namespace Application;
public interface IUserContext : IUserContextBase
{
    /// <summary>
    /// 用户是否存在
    /// </summary>
    /// <returns></returns>
    public Task<bool> ExistAsync<TUser>() where TUser : class, IEntityBase;
    public Task<TUser?> GetUserAsync<TUser>() where TUser : class, IEntityBase;
    public string? GetIpAddress();
    HttpContext? GetHttpContext();
}
