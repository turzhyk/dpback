using DPBack.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace DPBack.Application.Abstractions;

public interface ITokenProvider
{
    string Create(User user);
}