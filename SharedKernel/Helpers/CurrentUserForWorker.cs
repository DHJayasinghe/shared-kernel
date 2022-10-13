using SharedKernel.Entity;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace SharedKernel.Helpers;

public sealed class CurrentUserForWorker : ICurrentUser
{
    public Guid Id { get; }
    public Guid OrganizationId { get; }
    public string Email { get; }
    public List<string> Roles { get; }
    public bool IsAuthenticated { get; }
    public ChannelType ChannelType { get; }
    public bool IsAdmin { get; }
    public Guid ClientId { get; }
    public string Actor { get; }
    public CountryClaim Country { get; }
    public Guid UserReferenceId { get; }
}