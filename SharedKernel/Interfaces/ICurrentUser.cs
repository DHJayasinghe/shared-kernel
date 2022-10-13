using SharedKernel.Entity;
using System;
using System.Collections.Generic;

namespace SharedKernel.Interfaces;

public interface ICurrentUser
{
    Guid Id { get; }
    Guid OrganizationId { get; }
    Guid ClientId { get; }
    string Email { get; }
    List<string> Roles { get; }
    bool IsAuthenticated { get; }
    ChannelType ChannelType { get; }
    bool IsAdmin { get; }
    string Actor { get; }
    public CountryClaim Country { get; }

    /// <summary>
    /// If Current user is a PropertyManager this contains ManagerId
    /// </summary>
    public Guid UserReferenceId { get; }
}

public sealed class CountryClaim
{
    public Guid Id { get; set; }
    public string Code { get; set; }
}
